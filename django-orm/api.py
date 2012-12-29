# -*- coding: UTF-8 -*-

import os
import sys

os.environ['DJANGO_SETTINGS_MODULE'] = 'settings'
from django.core import mail
from django.conf import settings

HERE = os.path.abspath(os.path.dirname(__file__))

def send_mail(receiver,subject,body):
    mail.send_mail(subject,body,settings.EMAIL_HOST_USER,[receiver,])

if __name__ == '__main__':
    a,b,c = sys.argv[1:]
    send_mail(a,b,c)


