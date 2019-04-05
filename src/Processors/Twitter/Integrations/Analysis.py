class Analysis():
    def __init__(self, polarity, subjectivity, subject, messages, date):
        self.polarity = polarity
        self.subjectivity = subjectivity
        self.subject = subject
        self.messages = messages
        self.date = date

    def toDict(self):
        return {
            'polarity': self.polarity,
            'subjectivity': self.subjectivity,
            'subject': self.subject,
            'messages': self.messages,
            'date': self.date
        }