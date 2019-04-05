import json
from datetime import datetime
from pymongo import MongoClient
from logzero import logger
from Integrations import Analysis
from Configurations import Configuration

class Persistence:
    def InsertTweet(self, tweet):
        self.client = MongoClient()
        self.database = Configuration.Persistence()['Database']

        tweetsCollection = self.client[self.database].tweets
        tweetsCollection.insert_one(tweet)

        logger.info(f"PERSISTENCE - Tweet \"{tweet['Text']}\" from \"{tweet['IdCreator']}\"  successfully inserted.")

    def InsertAnalysis(self, tweet, newAnalysis):
        date = format(datetime.strptime(tweet['PostedOn'], '%Y-%m-%dT%H:%M:%S+00:00').date())
        subject = tweet['Subject']

        todayAndSubject = {
            'date': date,
            'subject': subject
        }
        
        analysisCollection = self.client[self.database].analysis

        currentAnalysis = analysisCollection.find_one(todayAndSubject)

        updatedAnalysis = Analysis.Analysis(None, None, None, None, None)

        newPolarity = newAnalysis.sentiment.polarity
        newSubjectivity = newAnalysis.sentiment.subjectivity

        if currentAnalysis:
            messagesCount = currentAnalysis['messages'] + 1
            currentPolarity = currentAnalysis['polarity']
            currentSubjectivity = currentAnalysis['subjectivity']

            updatedAnalysis.polarity = (currentPolarity + newPolarity) / messagesCount
            updatedAnalysis.subjectivity = (currentSubjectivity + newSubjectivity) / messagesCount
            updatedAnalysis.subject = subject
            updatedAnalysis.messages = messagesCount
            updatedAnalysis.date = date 

        else:
            updatedAnalysis.polarity = newPolarity
            updatedAnalysis.subjectivity = newSubjectivity
            updatedAnalysis.subject = subject
            updatedAnalysis.messages = 1
            updatedAnalysis.date = date

        analysisCollection.replace_one(todayAndSubject, updatedAnalysis.toDict(), upsert = True)

        logger.info(f"PERSISTENCE -  Analysis from \"{subject}\" and \"{date}\" inserted. Polarity: \"{updatedAnalysis.polarity}\", Subjectivity: \"{updatedAnalysis.subjectivity}\".")