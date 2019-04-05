import sys
from time import sleep
from logzero import logger
from Processor import Processor
from Integrations import MessageQueue
from Integrations import Persistence

try:
    messageQueue = MessageQueue.MessageQueue()
    persistence = Persistence.Persistence()

    processor = Processor.Processor(persistence.InsertTweet, persistence.InsertAnalysis)

    messageQueue.Initialize()
    messageQueue.StartConsuming(processor.ProcessMessage)
except Exception as e:
    logger.error(f'MAIN - An unexpected error has ocurred. {e}')
    messageQueue.Finalize()
    sys.exit()

logger.info('MAIN - Initialized instance.')

sleep(0)