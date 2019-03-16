from logzero import logger

Store = lambda m : None

def Initialize(StoreMessage):
    Store = StoreMessage
    
    logger.Info('Initialized processor.')

def ProcessMessage(message):
    if Store is None:
        raise Exception('You must initialize first.')

    Store(message)
    
    logger.Info(f"Processed message: '{message}'.")