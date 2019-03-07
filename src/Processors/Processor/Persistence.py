import mysql.connector
from logzero import logger

def Insert(data):
    db = mysql.connector.connect(
        host = '',
        user = '',
        passwd = '',
        database = ''
    )

    cursor = db.cursor()

    sql = "INSERT INTO"
    val = ('', '')

    cursor.execute(sql, val)

    db.commit()

    logger.Info(f"Data '{data}' successfully inserted.")
    