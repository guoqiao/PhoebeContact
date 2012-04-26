#!/usr/bin/env python
# -*- coding: utf-8 -*-

from django.db import models

class State(models.Model):
    name = models.CharField(default="",max_length=63)
    period = models.IntegerField(default=7)
    total = models.IntegerField(default=1)
    count = models.IntegerField(default=0)
    tmplate = models.CharField(default="",max_length=63)
    
    class Meta:
        db_table = 'State'

class Customer(models.Model):
    name = models.CharField(default="",max_length=63)
    site = models.URLField(default="",max_length=63)
    addr = models.CharField(default="",max_length=63)
    country = models.CharField(default="",max_length=63)
    phone = models.CharField(default="",max_length=63)
    
    contact = models.CharField(default="",max_length=63)
    mobile = models.CharField(default="",max_length=63)
    email = models.EmailField(default="",max_length=63)
    
    create_on = models.DateField()
    update_on = models.DateField()
    
    note = models.TextField(default="",max_length=2047)
    
    state = models.ForeignKey(State)
    
    class Meta:
        db_table = 'Customer'