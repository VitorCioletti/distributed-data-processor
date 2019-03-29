import json
import os

def MessageQueue():
    return GetConfig('MessageQueue')

def Persistence():
    return GetConfig('Persistence')

def GetConfig(configName):
    configurationFile = os.getcwd() + '/Configurations/config.json'

    if not os.path.isfile(configurationFile):
        raise Exception(f'Configuration file {configurationFile} was not found.')
    
    with open(configurationFile) as config_file:
        return json.load(config_file)[configName]