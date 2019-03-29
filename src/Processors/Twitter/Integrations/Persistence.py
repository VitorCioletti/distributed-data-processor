import psycopg2
import json
from logzero import logger
from Configurations import Configuration

def Insert(ch, method, properties, body):

    config =  Configuration.Persistence()
    message = json.loads(body)

    connection = psycopg2.connect(
        user = config["User"],
        password = config["Password"],
        host = config["Host"],
        port = config["Port"],
        database = config["Database"]
    )

    sql = "INSERT INTO Message VALUES (idcreator, subject, text, postedon)"
    val = (message["IdCreator"], message["Subject"], message["Text"], message["PostedOn"])

    connection.cursor().execute(sql, val)
    connection.commit()
    connection.close()
        
    ch.basic_ack(delivery_tag = method.delivery_tag)

    logger.info(f"Data '{body}' successfully inserted.")
    