import datetime
import uuid
import pyodbc # pip install pyodbc
import pathlib
import os
import pathlib
import sys

sys.stdout.reconfigure(encoding='utf-8')
sys.stderr.reconfigure(encoding='utf-8')

# Класс record, каждый такой экземпляр лежит в таблице Records в БД в одной строке
class Record:
    def __init__(self, uid, date, carnumber, image):
        self.uid = uid
        self.date = date
        self.carnumber = carnumber
        self.image = image

    # Инфа на экран о экземпляре записи
    def tostring(self):
        print(f'Uid={self.uid};Date={self.date};Car number={self.carnumber}"') #;Image="{self.image}


# Начало подключения, формируем строку для подключения и пытаемся подключиться
# Много примеров подключения: https://stackoverflow.com/questions/34249304/pyodbc-cant-connect-to-database
appname = 'Python sql connector'
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

maindir = os.path.dirname(os.path.abspath(__file__))
folderwithimages = 'detection-examples'
folderwithimagespath = pathlib.Path(maindir,folderwithimages)

def insertrecord(seconds, carnumber, imagefilename):
    date = datetime.datetime.fromtimestamp(seconds).strftime('%H:%M:%S') # %d-%m-%Y 
    uid = str(uuid.uuid4())
    imagefilepath = pathlib.Path(folderwithimagespath, imagefilename)
    imagefile = open(imagefilepath, 'rb')
    imagebytes = pyodbc.Binary(imagefile.read())
    imagefile.close()
    
    insertcmd = ''
    with sqlconn.cursor() as cursor:
        insertcmd = f"""INSERT INTO {tablename} (Uid, Date, CarNumber, Image) SELECT N'{uid}', '{date}', N'{carnumber}', (?)"""
    print(f'Insert command: {insertcmd}')
    cursor.execute(insertcmd, imagebytes)

insertrecord(31860, 'В277СК26', '1.png')
insertrecord(33720, 'Е381ХМ26', '2.png')
insertrecord(35220, 'М025ВХ26', '3.png')
insertrecord(36060, 'В416СУ26', '4.png')
insertrecord(37210, 'В280СО26', '5.png')
insertrecord(38440, 'А713СС126', '6.png')

print('Видео успешно проанализировано!')