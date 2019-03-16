import MessageQueue
import Persistence
import Processor
import sys
from time import sleep
from logzero import logger

try:
    Processor.Initialize(Persistence.Insert)
    MessageQueue.Initialize(Processor.ProcessMessage)
except:
    logger.Error(f'An unexpected error has ocurred. {sys.exc_info()[0]}')

logger.Info('Initialized instance.')

sleep(-1)