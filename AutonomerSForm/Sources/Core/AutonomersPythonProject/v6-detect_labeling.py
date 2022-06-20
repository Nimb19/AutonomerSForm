# https://github.com/qfgaohao/pytorch-ssd
import base64
import requests
import json
import datetime
import uuid
import pyodbc # pip install pyodbc

import torch
import torch.nn as nn
import torchvision
import torchvision.transforms as transforms
from torch.utils.data import Dataset, DataLoader, random_split
import pytesseract
import cv2
from matplotlib import pyplot as plt

from torchvision import transforms
from utils import *
from PIL import Image, ImageDraw, ImageFont

import easyocr

import torch
from vision.ssd.vgg_ssd import create_vgg_ssd, create_vgg_ssd_predictor
from vision.ssd.mobilenetv1_ssd import create_mobilenetv1_ssd, create_mobilenetv1_ssd_predictor
from vision.ssd.mobilenetv1_ssd_lite import create_mobilenetv1_ssd_lite, create_mobilenetv1_ssd_lite_predictor
from vision.ssd.squeezenet_ssd_lite import create_squeezenet_ssd_lite, create_squeezenet_ssd_lite_predictor
from vision.datasets.voc_dataset import VOCDataset
from vision.datasets.open_images import OpenImagesDataset
from vision.utils import box_utils, measurements
from vision.utils.misc import str2bool, Timer
import pathlib
import numpy as np
import sys
from vision.ssd.mobilenet_v2_ssd_lite import create_mobilenetv2_ssd_lite, create_mobilenetv2_ssd_lite_predictor
from vision.ssd.mobilenetv3_ssd_lite import create_mobilenetv3_large_ssd_lite, create_mobilenetv3_small_ssd_lite

import imutils

from tqdm import tqdm

import pathlib
import sys

maindir = os.path.dirname(os.path.abspath(__file__))

numberrecognize_APIKEY = '60b84eaf-aa77-474b-96fe-ff7e0fbdcd2a' # https://data.av100.ru/docs/numberrecognize?yadclid=60899495&yadordid=161574810&yclid=17678525864806973439
numberrecognize_URL = f'https://data.av100.ru/numberrecognize.ashx?key={numberrecognize_APIKEY}'

# директория файла
BASE_DIR = os.path.abspath(os.path.dirname(__file__))
sys.path.append(BASE_DIR)

DEVICE = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")

# ---------------------------------- VARIABLES
# предобученные модели
MODEL_DIR = pathlib.Path(maindir)
MODEL_FILENAME = "mobilenet-v1-ssd-mp-0_675.pth"

# в этой папке исходное видео
DATA_DIR = pathlib.Path(maindir,"DATA_DIR")
# MPG_FILENAME = "camera16(s1_c16)[2022-05-20(00-00-02)_2022-05-20(23-59-59)].mp4"
mpg_path = sys.argv[1].replace('/video:', '').strip('"')
# сколько секунд видео обрабатывать
mpg_duration = 2900 # 3*60*60 + 8*60 # 3 часа 8 минут

# в этой папке храним обработанные изображения
IMAGES_DIR = pathlib.Path(maindir, "IMAGES_DIR")
# папка с изображениями автомобилей
AUTO_DIR = pathlib.Path(maindir, "AUTO_DIR")

TRESHOLD = 0.5 # точность, чем меньше, тем менее уверенно опознанные обьекты детектируются

MIN_CONTOUR_PERI = 500 # минимальный периметр контура, в который поместится (и распознается) номер
MAX_CONTOUR_PERI = 1000 # слишком большие тоже не надо

# время в секундах, если в течение этого времени номер уже был, в выходной файл мы его не добавляем
DELTA_TIME = 600

# ---------------------------------- / VARIABLES

# Начало подключения, формируем строку для подключения и пытаемся подключиться
# Много примеров подключения: https://stackoverflow.com/questions/34249304/pyodbc-cant-connect-to-database
appname = 'v6-detect_labeling'
sqlappnameformated = f'ApplicationName={appname}'
sqldrivernameformated = 'DRIVER={ODBC Driver 17 for SQL Server};'

servername = 'localhost' 
username = None # 'myusername'
password = None # 'mypassword'
# dbname = 'AutonomerSDB'

sqlconnstring = f'{sqldrivernameformated}{sqlappnameformated};database=master;SERVER={servername};'
if (username != None):
    sqlconnstring += f'UID={username};PWD={password}'
else:
    sqlconnstring += 'Trusted_Connection=Yes'

# Подключаемся к СУБД, передавая строку подключения и аргумент автовыполнения новых команд
sqlconn = pyodbc.connect(sqlconnstring, autocommit = True)
print('Connect completed!')

# # Читаем файл 'CreateDbScript.sql' и сплитим его по слову 'GO' 
# createdbscriptFile = open('CreateDbScript.sql')
# createdbscript = createdbscriptFile.read()
# createdbscriptFile.close()
# createdbscripts = createdbscript.split('GO')

# # Выполняем прочитанные скрипты создания БД по порядку
# for sqlscript in createdbscripts:
#     with sqlconn.cursor() as cursor:
#         cursor.execute(sqlscript)
# print('Db created!')

# Общие переменные
dbname = 'AutonomerSDB'
tablename = f'{dbname}.dbo.Records'

# ---------------------------------- Посекундное разбиение видео

# бинарник ffmpeg должен находиться в папке, добавленной в PATH (Windows)

import ffmpeg
print()
print("Decode mpg to img:")
if not os.path.isdir(IMAGES_DIR):
    os.mkdir(IMAGES_DIR)

# проверяем наличие видео
if not os.path.isfile(mpg_path):
    print("File " + mpg_path + "not exist")
    exit()

i = 0
for t in tqdm(range(0, mpg_duration)):
    i+=1
    width, height = 1222, 1080
    x, y = 320, 0
    out_filename = "%d.jpg" % t
    out_file = str(pathlib.Path(IMAGES_DIR, out_filename))
    stream = ffmpeg.input(mpg_path, ss=t)
    #[y:y+height, x:x+width]
    stream = ffmpeg.filter(stream, 'scale', 4000, -1) # уменьшение размера (при необходимости)
    stream = ffmpeg.output(stream, out_file, vframes=1, loglevel="fatal")
    if i == 5:
        break
    ffmpeg.run(stream)

# ---------------------------------- Поиск изображений с автомобилями (v5-detect.py)

print()
print("Search auto and labels:")
if not os.path.isdir(AUTO_DIR):
    os.mkdir(AUTO_DIR)


ocr = easyocr.Reader(['ru'])
# массив с распозанными номерами, формат: время в сек:распознано
out_str = ""
prev_labels = []
for time in tqdm(range(0, mpg_duration)):

    class_names = [name.strip() for name in open(pathlib.Path(BASE_DIR, "voc-model-labels.txt")).readlines()]
    net = create_mobilenetv1_ssd(len(class_names), is_test=True)
    net.load(pathlib.Path(MODEL_DIR, MODEL_FILENAME))
    net = net.to(DEVICE)
    predictor = create_mobilenetv1_ssd_predictor(net, nms_method="hard", device=DEVICE)

    # из vision/datasets/voc_dataset.py/def _read_image(self, image_id):
    in_filename = "%d.jpg" % time
    image_path = pathlib.Path(IMAGES_DIR, in_filename)
    image = cv2.imread(str(image_path))
    ###!!!https://pythobyte.com/how-to-crop-an-image-using-opencv-5a665f34/
    image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
    boxes, labels, probs = predictor.predict(image)

    for i in range(boxes.size(0)):
        if (float(probs[i]) > TRESHOLD) and (labels[i] == 7 or labels[i] == 6):
            box = boxes[i, :]
            top_left_x = int(box[0])
            top_left_y = int(box[1])
            bottom_right_x = int(box[2])
            bottom_right_y = int(box[3])

            cv2.rectangle(image, (int(box[0]), int(box[1])), (int(box[2]), int(box[3])), (255, 255, 0), 4)
            #label = f"""{voc_dataset.class_names[labels[i]]}: {probs[i]:.2f}"""
            label = f"{class_names[labels[i]]}: {probs[i]:.2f}"
            cv2.putText(image, label,
                        (int(box[0]) + 20, int(box[1]) + 40),
                        cv2.FONT_HERSHEY_SIMPLEX,
                        1,  # font scale
                        (0, 0, 255),
                        1)  # line type

            path = pathlib.Path(AUTO_DIR, in_filename) 
            #cv2.imwrite(path, image)
            
            # вырезаем прямоугольник с автомобилем и сохраняем в отдельный файл
            ii = Image.open(image_path)
            box = (top_left_x, top_left_y, bottom_right_x, bottom_right_y)
            img_cropped = ii.crop(box)
            
            out_filename = "%d-cropped.jpg" % time
            out_path = pathlib.Path(AUTO_DIR, out_filename) 
            img_cropped.save(out_path)

            # ---------------------------- Ищем и распознаем номера на обрезанных фото / v1-baseline.py
            encoded_string = ''
            with open(out_path, "rb") as img_cropped_bytestream:
                encoded_string = base64.b64encode(img_cropped_bytestream.read()).decode('utf-8')
            
            res = requests.post(numberrecognize_URL, encoded_string)
            print(res.text)

            jsonresult = json.loads(res.text)

            responeerror = jsonresult['error']
            if (responeerror != None):
                print(f'От сервиса распознавания номера автомобиля пришла ошибка: {responeerror}')
                continue

            responsecarnumbers = jsonresult['result']
            print(responsecarnumbers) # TODO: отладь убери 
            responsecarnumber = responsecarnumbers[0]

            if len(responsecarnumber) > 0:
                # ищем, был ли этот номер за последние DELTA_TIME
                is_good = True
                for prev in prev_labels:
                    prev_t, prev_text = prev
                    if prev_text == responsecarnumber:
                        if (time-prev_t) < DELTA_TIME:
                            is_good = False

                # если номер подходит под критерии (по времени например) мы его сохраняем
                if is_good:
                    out_str += f"{time}:{responsecarnumber}\n"

                    # Пример INSERT
                    uid = str(uuid.uuid4())
                    date = time.strftime('%d-%m-%Y %H:%M:%S')
                    carnumber = responsecarnumber

                    imagefile = open(out_path, 'rb')
                    imagebytes = pyodbc.Binary(imagefile.read())
                    imagefile.close()

                    with sqlconn.cursor() as cursor:
                        insertcmd = f"""INSERT INTO {tablename} (Uid, Date, CarNumber, Image)
                                        SELECT N'{uid}', '{date}', N'{carnumber}', (?)"""
                        #print(insertcmd)
                        cursor.execute(insertcmd, imagebytes)
                    print(f'Insert completed! Insert command: {insertcmd}')

#----------------------------------------------- Сохраняем файл с номерами для отладки
outfile_path = pathlib.Path(BASE_DIR, "out.txt")
file = open(outfile_path, 'w')
file.write(out_str)
file.close()

print("success")