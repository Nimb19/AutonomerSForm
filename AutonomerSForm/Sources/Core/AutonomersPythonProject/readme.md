v0-baseline.py   
при таком наивном подходе - выбор максимального четырехугольника, детектируется далеко не всегда то что надо  

v1-baseline.py  
отсеиваем по минимальному и максимальному порогу периметра, а не берем самые большие - остается слишком много, для применения медленного распознавания  

v2-haar-cascades.py  
https://pyimagesearch.com/2021/04/12/opencv-haar-cascades/  
https://stackabuse.com/object-detection-with-opencv-python-using-a-haar-cascade-classifier/  
- вообще не детектирует  

v3-openalpr.py  
https://github.com/openalpr/openalpr  
https://pypi.org/project/openalpr/  
pip install openalpr  
платное  

