# -*- coding: UTF-8 -*-

import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'settings'

from datetime import datetime
import csv

from django.conf import settings
from main.models import *

HERE = os.path.abspath(os.path.dirname(__file__))

def init_states():
    states = [
        ['A',1,1,0],
        ['B',4,1,0],
        ['C',7,4,0],
        ['D',30,12,0],
        ['E',0,0,0],
    ]
    State.objects.all().delete()
    for state in states:
        obj = State()
        obj.name = state[0]
        obj.period = state[1]
        obj.total = state[2]
        obj.count = state[3]
        obj.save()

if __name__ == '__main__':
    init_states()

    