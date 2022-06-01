import datetime
import uuid
import pyodbc # pip install pyodbc

# Класс record, каждый такой экземпляр лежит в таблице Records в БД в одной строке
class Record:
    def __init__(self, uid, cameraname, date, carnumber, image):
        self.uid = uid
        self.cameraname = cameraname
        self.date = date
        self.carnumber = carnumber
        self.image = image

    # Инфа на экран о экземпляре записи
    def tostring(self):
        print(f'Uid={self.uid};Camera name={self.cameraname};Date={self.date};Car number={self.carnumber}"') #;Image="{self.image}


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


# Пример INSERT
uid = str(uuid.uuid4())
cameraname = 'Camera №1'
date = datetime.datetime.now().strftime('%d-%m-%Y %H:%M:%S')
carnumber = 'A228AA128'

imagefile = open('dali.jpg', 'rb')
imagebytes = str(bytearray(imagefile.read())).replace("'", "''")
# imagebytes = str(imagefile.read()).replace("'", "''")
imagefile.close()

with sqlconn.cursor() as cursor:
    insertcmd = f"""INSERT INTO {tablename} (Uid, CameraName, Date, CarNumber, Image)
                    SELECT N'{uid}', N'{cameraname}', '{date}', N'{carnumber}', CAST('{imagebytes}' as VARBINARY(MAX))"""
    #print(insertcmd)
    cursor.execute(insertcmd)
print('Insert completed!')


# Пример SELECT
records = []
with sqlconn.cursor() as cursor:
    cursor.execute(f'SELECT * FROM {tablename}')
    # 'WHERE Uid = такой то' или любая другая выборка. Это как общий метод чтения всех записей, которые получили
    for row in cursor.fetchall():
        record = Record(row.Uid, row.CameraName, row.Date, row.CarNumber, row.Image)
        records.append(record)

for record in records:
    record.tostring()
print('Select completed!')


# # Пример REMOVE
# with sqlconn.cursor() as cursor:
#     deletecmd = f"DELETE FROM {tablename} WHERE Uid = '{uid}'"
#     #print(deletecmd)
#     cursor.execute(deletecmd)
# print('Delte completed!')

