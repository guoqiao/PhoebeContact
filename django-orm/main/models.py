#!/usr/bin/env python
# -*- coding: utf-8 -*-

from django.db import models

class Country(models.Model):
    abbr = models.CharField(default="",max_length=63)
    chinese = models.CharField(default="",max_length=63)
    english = models.CharField(default="",max_length=63)
    
    area = models.CharField(default="",max_length=63)
    code = models.CharField(default="",max_length=63)
    language = models.CharField(default="",max_length=63)
    google = models.URLField(default="",max_length=63)
    
    hourdiff = models.IntegerField(default=-12) 
    
    class Meta:
        db_table = 'Country'

class State(models.Model):
    name = models.CharField(default="",max_length=63)
    period = models.IntegerField(default=7)
    total = models.IntegerField(default=1)  
    
    class Meta:
        db_table = 'State'

class Customer(models.Model):
    company = models.CharField(default="",max_length=63)
    country = models.CharField(default="",max_length=63)
    site = models.URLField(default="",max_length=63)
    addr = models.CharField(default="",max_length=63)
    
    name = models.CharField(default="",max_length=63)
    skype = models.CharField(default="",max_length=63)
    email = models.EmailField(default="",max_length=63)
    mobile = models.CharField(default="",max_length=63)
    phone = models.CharField(default="",max_length=63)
    browse = models.IntegerField(default=0)
    inquiry = models.IntegerField(default=0)
    
    create_on = models.DateField()
    update_on = models.DateField()
    
    state = models.ForeignKey(State)
    count = models.IntegerField(default=4)
    note = models.TextField(default="",max_length=2047)
    
    
    class Meta:
        db_table = 'Customer'