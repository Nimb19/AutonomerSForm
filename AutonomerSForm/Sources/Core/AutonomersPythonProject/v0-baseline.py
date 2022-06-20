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


img_filename = "f_263628be7a661045.jpg"


img_path = os.path.join(basedir, img_filename)
img = plt.imread(img_path)
img = img[..., ::-1]  # RGB --> BGR
print(img.shape)

#в оттенки серого
img_gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
# размытие (blur)
img_blur = cv2.bilateralFilter(img_gray, 11, 15, 15)
# детекция границ
img_edges = np.uint8(img_blur)
img_edges = cv2.Canny(img_edges, 30, 200)

# контуры
contours = cv2.findContours(img_edges.copy(), cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)
contours  = imutils.grab_contours(contours)
contours = sorted(contours, key=cv2.contourArea, reverse=True)[:8]

# отсеиваем не прямоугольники
pos = None
for c in contours:
  # периметр контура
  peri = cv2.arcLength(c, True)
  approx = cv2.approxPolyDP(c, 0.018 * peri, True)
  if len(approx) == 4:
    pos = approx
    break



contour_color = (255, 0, 0)
gray_BGR = cv2.cvtColor(img_gray, cv2.COLOR_GRAY2BGR)
show_img = cv2.drawContours(gray_BGR, [pos], 0, contour_color, -1)
plt.imshow(cv2.cvtColor(show_img, cv2.COLOR_BGR2RGB))
plt.show()
exit()

mask = np.zeros(img_gray.shape, np.uint8)
new_img = cv2.drawContours(mask, [pos], 0, 255, -1)
bitwise_img = cv2.bitwise_and(img, img, mask=mask)

(x, y) = np.where(mask==255)
(x1, y1) = (np.min(x), np.min(y))
(x2, y2) = (np.max(x), np.max(y))
crop = gray[x1:x2, y1:y2]
text = easyocr.Reader(['ru'])
text = text.readtext(crop)

res = text[0][-2]
final_image = cv2.putText(img, res, (x1, y2 + 60), cv2.FONT_HERSHEY_PLAIN, 1, (0,0,255), 1)
final_image = cv2.rectangle(img, (x1, y1), (x2, y2), (0,255,0), 1)
print(pos)

pl.imshow(cv2.cvtColor(crop, cv2.COLOR_BGR2RGB))
pl.show()
