import json
from logzero import logger
from textblob import TextBlob

class Processor:
    def __init__(self, StoreTweet, StoreAnalysis):
        self.StoreTweet = StoreTweet
        self.StoreAnalysis = StoreAnalysis

    def ProcessMessage(self, ch, method, _, body):

        tweet = json.loads(body)

        analysis = TextBlob(tweet["Text"])

        self.StoreTweet(tweet)
        self.StoreAnalysis(tweet, analysis)

        ch.basic_ack(delivery_tag = method.delivery_tag)

        logger.info(f"PROCESSOR - Processed message: '{body}'.")