import pika
from Configurations import Configuration
from logzero import logger

class MessageQueue:

    def __init__(self):
        self.configuration = Configuration.MessageQueue()        

    def Initialize(self):
        credentials = pika.PlainCredentials(self.configuration['User'], self.configuration['Password'])

        self.connection = pika.BlockingConnection(pika.ConnectionParameters(
            host = self.configuration['Host'],
            virtual_host = self.configuration['VirtualHost'],
            credentials = credentials
        ))

        self.channel = self.connection.channel()

        logger.info("MESSAGEQUEUE - Initialized.")

    def StartConsuming(self, StoreMessage):
        self.channel.basic_consume(
            self.configuration['Queue'],
            StoreMessage
        )

        self.channel.start_consuming()

        logger.info("MESSAGEQUEUE - Started consuming.")

    def Finalize(self):
        self.channel.stop_consuming()
        self.connection.close()

        logger.info("MESSAGEQUEUE - Finalized consuming.")