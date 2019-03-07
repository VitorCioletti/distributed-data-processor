import pika
import Configuration
from logzero import logger

connection = pika.BlockingConnection(pika.ConnectionParameters(''))

channel = connection.channel()

def Initialize(ProcessMessage):
    channel.basic_consume(
        consumer_callback = ProcessMessage,
        queue = '',
        no_ack = False,
    )

    channel.start_consuming()

    logger.Info("Initialized message queue consuming.")

def Finalize():
    channel.stop_consuming()
    connection.close()

    logger.Info("Finalized message queue consuming.")