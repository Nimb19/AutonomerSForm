from ntpath import join
import cv2
import numpy as np
import imutils
import easyocr
from matplotlib import pyplot as plt


import os, sys
# директория файла
basedir = os.path.abspath(os.path.dirname(__file__))
sys.path.append(basedir)


img_filename = "f_727628be7ee0d19f.jpg"


img_path = os.path.join(basedir, img_filename)
img = plt.imread(img_path)
img = img[..., ::-1]  # RGB --> BGR
print("img.shape", img.shape)

#в оттенки серого
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
gray_BGR = cv2.cvtColor(img_gray, cv2.COLOR_GRAY2BGR)


# загружаем каскад
detector = cv2.CascadeClassifier(cv2.data.haarcascades + "haarcascade_licence_plate_rus_16stages.xml")
# ищем
detected_objects = detector.detectMultiScale(img_gray, minSize=(30, 30))

if len(detected_objects) != 0:
    for (x, y, width, height) in detected_objects:
        cv2.rectangle(gray_BGR, (x, y),
                      (x + height, y + width),
                      (0, 255, 0), 2)

print("len(detected_objects):", len(detected_objects))

plt.imshow(cv2.cvtColor(gray_BGR, cv2.COLOR_BGR2RGB))
plt.show()

print("complete")
