import json
import os

def MessageQueueConfig():
    return GetConfig('MessageQueue')

def PersistenceConfig():
    return GetConfig('Persistence')

def GetConfig(configName):
    configurationFile = 'configuration.json'

    if os.path.isfile(configurationFile):
        raise Exception(f'Configuration file {configurationFile} was not found.')
    
    with open(configurationFile) as config_file:
        return json.load(config_file)