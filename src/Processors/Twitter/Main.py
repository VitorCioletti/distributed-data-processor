import sys
from time import sleep
from logzero import logger
from Processor import Processor
from Integrations import MessageQueue
from Integrations import Persistence

try:
    Processor.Initialize()
    
    messageQueue = MessageQueue.MessageQueue()

    messageQueue.Initialize()
    messageQueue.StartConsuming(Persistence.Insert)
except Exception as e:
    logger.error(f'An unexpected error has ocurred. {e}')
    sys.exit()

logger.info('Initialized instance.')

sleep(0)