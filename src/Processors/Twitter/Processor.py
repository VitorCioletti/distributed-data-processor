from logzero import logger

def Initialize():
    logger.info('Initialized processor.')

def ProcessMessage(ch, method, properties, body):
    
    ch.basic_ack(delivery_tag = method.delivery_tag)
    
    logger.info(f"Processed message: '{body}'.")