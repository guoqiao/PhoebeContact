# -*- coding: UTF-8 -*-

import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'settings'

from datetime import datetime
import traceback
import csv

from django.conf import settings
from fenix.models import *

HERE = os.path.abspath(os.path.dirname(__file__))

# 浦东,世博园
X="31.179459826048465" 
Y="121.47756278514862"

AREA_ID = 2

EMPTY_DATETIME= datetime(2000,1,1,0,0,0)

def init_area():
    """
    初始化区域, 仅2个, 一个一级区域上海, 一个二级区域浦东
    """
    objs = Area.objects.all().delete()
    a1 = Area(name="上海",parent=0)
    a1.save()
    print 'a1.id=%d' % a1.id
    a2 = Area(name="浦东",parent=a1.id)
    a2.save()
    AREA_ID = a2.id
    print 'a2.id=%d' % a2.id

def import_sn_num():
    filename = 'sn_num.csv'
    filepath = os.path.join(HERE, filename)
    print filepath
    fp = open(filepath, 'rb')
    reader = csv.reader(fp, delimiter=',', quotechar='|')
    print 'clear all hydrants...'
    Hydrant.objects.all().delete()
    for row in reader:
        device_sn = row[0].strip()
        number = row[1].strip()
        print device_sn, number
        obj = Hydrant(
            device=device_sn,
            number=number,
            area=AREA_ID,
            x=X,
            y=Y,
            install=EMPTY_DATETIME,
            last=EMPTY_DATETIME,
            )
        obj.save()      
        
def get_hydrant_info():
    filename = 'hydrants.txt'
    filepath = os.path.join(HERE, filename)
    print filepath
    fp = open(filepath, 'rb')
    reader = csv.reader(fp, delimiter='\t', quotechar='|')
    
    filenumberchange = os.path.join(HERE,'numberchange.txt')
    fncp = open(filenumberchange,'w')
    
    # 消防栓编号	区名	路名	经度	纬度	模块加入日期	模块号码	
    # 通信模块状态	消防栓状态	消防栓状态更新时间
    for row in reader:
        device_sn = row[0].strip()
        print device_sn
        obj = Hydrant.objects.get(device=device_sn)
        
        road = row[2].strip()
        if road:
            if not road.isdigit():
                print road
                obj.road = road
        
        # 老系统里,貌似是先纬度,后经度
        y = row[3].strip()
        x = row[4].strip()
        if x and y:
            if '.' in x and '.' in y:
                print x,y
                obj.x = x
                obj.y = y
        
        install = row[5].strip()
        if install:
            print install
            install = datetime.strptime(install,'%Y/%m/%d %H:%M:%S')
            obj.install = install
        
        number = row[6].strip()
        if number:
            # check
            if number != obj.number:
                line = 'device %s number %s change to %s' % (obj.device, obj.number, number)
                print line
                fncp.write(line+'\n')
                obj.number = number
        
        last = row[-1].strip()
        if last:
            print last
            last = datetime.strptime(last,'%Y/%m/%d %H:%M:%S')
            obj.last = last
            
        obj.save()

def strip_device_sn():
    """
    部分设备编号后面有空格,用此函数去掉
    """
    objs = Hydrant.objects.all()
    for obj in objs:
        print obj.device
        obj.device = obj.device.strip()
        obj.save()
        
def rm_digit_road():
    """
    部分设备的road信息是编号,修改掉
    """
    objs = Hydrant.objects.all()
    for obj in objs:
        if obj.road:
            if obj.road.isdigit():
                print 'rm road:',obj.device
                print obj.road
                obj.road = ""
                obj.save()              

def pick_messages():
    
    filemessages = os.path.join(HERE, 'message.txt')
    fpw = open(filemessages,'w')
    
    dirpath = os.path.join(HERE,'log')
    for filename in os.listdir(dirpath):
        if 'report' in filename:
            continue
        print filename
        filepath = os.path.join(dirpath, filename)
        fp = open(filepath)
        flag = 0
        for line in fp:
            if flag:
                print line
                fpw.write(line)
                flag -= 1
                if flag == 0:
                    fpw.write('\n')
                continue
            if not line:
                continue
            # line = line.strip()
            # if not line:
                # continue
            if line.startswith("+CMGL") and 'REC UNREAD' in line and '10086' not in line:
                print line
                fpw.write(line)
                flag = 1
            else:
                flag = 0
    
def backup_device_circle_positon():
    filename = 'device_status.csv'
    filepath = os.path.join(HERE,filename)
    fp = open(filepath,'w')
    objs = Hydrant.objects.all()
    for obj in objs:
        line = '%d,%d,%d' % (obj.id, obj.circle, obj.position)
        print line
        fp.write(line+'\n')
    fp.close()
    
def clear_dev_status():
    objs = Hydrant.objects.all()
    for obj in objs:
        print 'clear', obj.id
        obj.circle = 0
        obj.position = -1
        obj.save()

def show_dev_status():
    objs = Hydrant.objects.all()
    for obj in objs:
        print obj.id,obj.circle,obj.position
        
def restore_dev_status():
    filename = 'device_status.csv'
    filepath = os.path.join(HERE,filename)
    fp = open(filepath,'r')
    reader = csv.reader(fp, delimiter=',', quotechar='|')
    for row in reader:
        print row
        obj = Hydrant.objects.get(id=int(row[0]))
        obj.circle = int(row[1])
        obj.position = int(row[2])
        obj.save()
  
def find_repeated_messages():
    objs = Message.objects.all()
    fp = open(os.path.join(HERE,'repeated2.csv'),'w')
    for obj in objs:
        repeated = Message.objects.filter(number=obj.number, receive=obj.receive, id__gt=obj.id)
        if repeated:
            print 'find repteated:', obj.id, obj.number, obj.receive, obj.content
        for item in repeated:
            line = "%d,%s,%s,%s,[%d]" % (item.id, item.number, str(item.receive), item.content, obj.id)
            print '>>>>>',line 
            fp.write(line+'\n')
            
def rm_repeated_messages():            
    filename = 'repeated.csv'
    filepath = os.path.join(HERE,filename)
    fp = open(filepath,'r')
    reader = csv.reader(fp, delimiter=',', quotechar='|')
    for row in reader:
        print row
        obj = Message.objects.get(id=int(row[0]))
        obj.delete()
        
def find_repeated_usages():
    objs = Usage.objects.all()
    fp = open(os.path.join(HERE,'repeated_usages.csv'),'w')
    for obj in objs:
        repeated = Usage.objects.filter(hydrant=obj.hydrant, start=obj.start, id__gt=obj.id)
        if repeated:
            print 'find repteated:', obj.id, obj.hydrant, obj.start
        for item in repeated:
            line = "%d,%s,%s,[%d]" % (item.id, item.hydrant, str(item.start), obj.id)
            print '>>>>>',line 
            fp.write(line+'\n')
            
def rm_repeated_usages():            
    filename = 'repeated_usages.csv'
    filepath = os.path.join(HERE,filename)
    fp = open(filepath,'r')
    reader = csv.reader(fp, delimiter=',', quotechar='|')
    for row in reader:
        print row
        obj = Usage.objects.get(id=int(row[0]))
        obj.delete()        
        
if __name__ == '__main__':
    # init_area()
    # import_sn_num()
    # get_hydrant_info()
    # strip_device_sn()
    # rm_digit_road()
    # pick_messages()
    # backup_device_circle_positon()
    # clear_dev_status()
    # show_dev_status()
    # find_repeated_messages()
    # rm_repeated_messages()
    # find_repeated_usages()
    rm_repeated_usages()

    