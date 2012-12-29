#!/usr/bin/env python
# -*- coding: utf-8 -*-
import models

from django.contrib import admin
admin.site.register(models.Country)
admin.site.register(models.State)
admin.site.register(models.Customer)

