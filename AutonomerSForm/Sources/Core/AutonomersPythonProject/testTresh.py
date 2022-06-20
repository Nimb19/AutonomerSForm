import cv2
import pytesseract

roi = cv2.imread('/Users/nsamoilove/Desktop/f_6816294435700af1/IMAGES_DIR/2635.jpg')

width, height = 1222, 1080
x, y = 320, 0

# Crop image to specified area using slicing
crop_img = roi[y:y+height, x:x+width]
# Show image
cv2.imshow("cropped", crop_img)
cv2.waitKey(0)

text = pytesseract.image_to_string(roi,config="-c tessedit_char_blacklist=.,|><:;~`/][}{-=)($%^-!")
print(text.replace("'",""))