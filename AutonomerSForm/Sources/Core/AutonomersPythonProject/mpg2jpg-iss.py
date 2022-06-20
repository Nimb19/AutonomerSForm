# -*- coding: utf-8 -*-
import os,json, tqdm
os.system('cls' if os.name == 'nt' else 'clear')

import pathlib
data_path = pathlib.Path("D:\\", "_hack", "2021-22", "isshack", "_iss_1")
mpg_file = str(pathlib.Path(data_path, "fight_train.mp4"))
mpg_file_800 = str(pathlib.Path(data_path, "fight_train_800.mp4"))

jpg_path = pathlib.Path(data_path, "img_ffmpeg")



import cv2
import numpy as np


import ffmpeg
for i in range(0, 4645):
    out_filename = "%d.jpg" % i
    out_file = str(pathlib.Path(jpg_path, out_filename))
    stream = ffmpeg.input(mpg_file, ss=i)
    stream = ffmpeg.filter(stream, 'scale', 800, -1)
    stream = ffmpeg.output(stream, out_file, vframes=1)
    ffmpeg.run(stream)



"""
#https://stackoverflow.com/questions/33311153/python-extracting-and-saving-video-frames
vidcap = cv2.VideoCapture(mpg_file)
success, image = vidcap.read()
count = 0
while success:
    jpg_file_name = "img%d.jpg" % count
    jpg_file = os.path.join(data_path, jpg_file_name)

    #is_cv2_save = cv2.imwrite(str(jpg_file), image)     # save frame as JPEG file      
    success, image = vidcap.read()
    count += 1
    print(str(count))

print("count: ", str(count))
"""



print()
print("success")



