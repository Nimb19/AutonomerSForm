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


import json
from openalpr import Alpr


alpr = Alpr("ru", "/path/to/openalpr.conf", "/path/to/runtime_data")

plt.imshow(cv2.cvtColor(gray_BGR, cv2.COLOR_BGR2RGB))
plt.show()

print("complete")
