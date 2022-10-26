import io
import os
import json
from typing import Callable;


config = json.load(open("./config.json"))

def readConfig():
    readProp(config, 'sources',  readSources)

def readProp(props : dict, key : str, callback : Callable[[dict | list], None]):
    if key in props:
        callback(props[key])

def readSources(sources: dict | list ) -> None:
    for source in sources:
        files = os.listdir(source)
        print(files)

readConfig()