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
        ['A',1,0],
        ['B',4,0],
        ['C',7,0],
        ['D',30,0],
        ['E',0,0],
    ]
    State.objects.all().delete()
    for state in states:
        obj = State()
        obj.name = state[0]
        obj.period = state[1]
        obj.total = state[2]
        obj.save()

def import_country():
    # 英文国家/地区名    ,中文国家/地区名,电话区号,时差,工作时间,常用语言,国缩写,地区,google
    path = os.path.join(HERE, "c.csv")
    fp = open(path)
    reader = csv.reader(fp, delimiter=',', quotechar='"')
    Country.objects.all().delete()
    for row in reader:
        row = [cell.strip() for cell in row]
        c = Country()
        c.abbr = row[6]
        if not c.abbr:
            print "empty abbr for %s, skip" % row[0]
            continue
        if Country.objects.filter(abbr=c.abbr):
            print "%s exists, skip" % c.abbr
            continue
        print row

        c.english = row[0]
        c.chinese = row[1]
        c.code = row[2]
        str_hourdiff = row[3]
        if ':30' in str_hourdiff:
            str_hourdiff = str_hourdiff.replace(':30', '.5',1)
            print 'new hourdiff:',str_hourdiff
        print 'str_hourdiff:',str_hourdiff
        if str_hourdiff:
            c.hourdiff = float(str_hourdiff)
        c.language = row[5]
        
        c.area = row[7]
        c.google = row[8]
        c.save()

def parsetime(str):
    dt = datetime.now()
    if not str or not str.strip():
        return dt
    try:
        dt = datetime.strptime(str, '%Y-%m-%d')
    except:
        dt = datetime.strptime(str, '%Y.%m.%d')
    return dt

def import_customer():        
    path = os.path.join(HERE, "a.csv")
    fp = open(path)
    reader = csv.reader(fp, delimiter=',', quotechar='"')
    x = 0
    y = 0
    for row in reader:
        x+=1
        print x
        r = [cell.strip() for cell in row]
        c = Customer()
        i = 0
        #公司,国家,网站,地址,姓名,Skype,邮箱,手机,电话,
        c.company = r[i]
        i+=1
        c.country = r[i]
        i+=1
        c.site = r[i]
        i+=1
        c.addr = r[i]
        i+=1
        c.name = r[i]
        i+=1
        c.skype = r[i]
        i+=1
        c.email = r[i]
        if not c.email:
            print 'no email, skip'
            continue
        if Customer.objects.filter(email=c.email):
            print '%s exists, skip' % c.email
            y+=1
            continue
        i+=1
        c.mobile = r[i]
        i+=1
        c.phone = r[i]
        i+=1
        #浏览,询盘,首次,最近,下次,状态,剩余,备注,,,,,,
        c.browse = int(r[i].strip() or 0)
        i+=1
        c.inquiry = int(r[i].strip() or 0)
        i+=1
        c.create_on = parsetime(r[i])
        i+=1
        c.update_on = parsetime(r[i])
        i+=1
        next = r[i] # **
        i+=1
        state = r[i]
        c.state = State.objects.get(pk=5)
        i+=1
        c.count = 0
        i+=1
        c.note = r[i]
        i+=1
        c.save()
        # print c.name + ' saved'
    print y
        
        
if __name__ == '__main__':
    # init_states()
    # import_country()
    import_customer()
    c = Customer.objects.all().count()
    print c

    