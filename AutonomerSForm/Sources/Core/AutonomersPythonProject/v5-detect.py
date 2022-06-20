# https://github.com/qfgaohao/pytorch-ssd

import torch
import torch.nn as nn
import torchvision
import torchvision.transforms as transforms
from torch.utils.data import Dataset, DataLoader, random_split

import cv2

from torchvision import transforms
from utils import *
from PIL import Image, ImageDraw, ImageFont

import torch
from vision.ssd.vgg_ssd import create_vgg_ssd, create_vgg_ssd_predictor
from vision.ssd.mobilenetv1_ssd import create_mobilenetv1_ssd, create_mobilenetv1_ssd_predictor
from vision.ssd.mobilenetv1_ssd_lite import create_mobilenetv1_ssd_lite, create_mobilenetv1_ssd_lite_predictor
from vision.ssd.squeezenet_ssd_lite import create_squeezenet_ssd_lite, create_squeezenet_ssd_lite_predictor
from vision.datasets.voc_dataset import VOCDataset
from vision.datasets.open_images import OpenImagesDataset
from vision.utils import box_utils, measurements
from vision.utils.misc import str2bool, Timer
import argparse
import pathlib
import numpy as np
import logging
import sys
from vision.ssd.mobilenet_v2_ssd_lite import create_mobilenetv2_ssd_lite, create_mobilenetv2_ssd_lite_predictor
from vision.ssd.mobilenetv3_ssd_lite import create_mobilenetv3_large_ssd_lite, create_mobilenetv3_small_ssd_lite

import pathlib
import sys
# директория файла
BASE_DIR = os.path.abspath(os.path.dirname(__file__))
sys.path.append(BASE_DIR)

DEVICE = torch.device("cuda:0" if torch.cuda.is_available() else "cpu")


# ---------------------------------- VARIABLES

MODEL_DIR = pathlib.Path("D:\\", "fl", "2022", "830-autonomer")
MODEL_FILENAME = "mobilenet-v1-ssd-mp-0_675.pth"
DATA_DIR = pathlib.Path("D:\\", "fl", "2022", "830-autonomer", "data")
IMAGE_FILENAME = "f_263628be7a661045.jpg"
TRESHOLD = 0.5 # точность, чем меньше, тем менее уверенно опознанные обьекты детектируются

# ---------------------------------- / VARIABLES


class_names = [name.strip() for name in open(os.path.join(BASE_DIR, "voc-model-labels.txt")).readlines()]
net = create_mobilenetv1_ssd(len(class_names), is_test=True)
net.load(os.path.join(MODEL_DIR, MODEL_FILENAME))
net = net.to(DEVICE)
predictor = create_mobilenetv1_ssd_predictor(net, nms_method="hard", device=DEVICE)

# из vision/datasets/voc_dataset.py/def _read_image(self, image_id):
image_file = os.path.join(DATA_DIR, IMAGE_FILENAME)
image = cv2.imread(str(image_file))
image = cv2.cvtColor(image, cv2.COLOR_BGR2RGB)
boxes, labels, probs = predictor.predict(image)

print(probs)
print(1)
for i in range(boxes.size(0)):
    if (float(probs[i]) > TRESHOLD):
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



path = os.path.join(DATA_DIR, "run_ssd_example_output.jpg") 
cv2.imwrite(path, image)


print("success")
