import json
from pymongo import MongoClient
from logzero import logger
from Configurations import Configuration

def Insert(ch, method, _, body):

    client = MongoClient()
    database = Configuration.Persistence()['Database']
    tweet = json.loads(body) 

    tweets = client[database].tweets
    tweets.insert_one(tweet)

    ch.basic_ack(delivery_tag = method.delivery_tag)

    logger.info(f"PERSISTENCE - Tweet \"{tweet['Text']}\" from \"{tweet['IdCreator']}\"  successfully inserted.")