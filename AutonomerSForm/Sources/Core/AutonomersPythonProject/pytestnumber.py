import numpy as np
import cv2
import pytesseract
import matplotlib.pyplot as plt


img = cv2.imread('/Users/nsamoilove/Desktop/f_6816294435700af1/AUTO_DIR/1186-cropped.jpg')
gray = cv2.cvtColor(img, cv2.COLOR_BGR2GRAY)
thresh = cv2.adaptiveThreshold(gray,255,cv2.ADAPTIVE_THRESH_GAUSSIAN_C, cv2.THRESH_BINARY,15,2)
contours,h = cv2.findContours(thresh,1,2)

largest_rectangle = [0,0]
for cnt in contours:
    approx = cv2.approxPolyDP(cnt,0.01*cv2.arcLength(cnt,True),True)
    if len(approx)==4:
        area = cv2.contourArea(cnt)
        if area > largest_rectangle[0]:
            largest_rectangle = [cv2.contourArea(cnt), cnt, approx]



x,y,w,h = cv2.boundingRect(largest_rectangle[1])
roi=img[y:y+h,x:x+w]
plt.imshow(roi, cmap = 'gray')
plt.show()


text = pytesseract.image_to_string(roi)
print(text)