#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import urllib.request
import shutil
import zipfile
import time
import random

cur_dir = os.path.dirname(__file__)
master_dir = os.path.join(cur_dir)

user_agent = 'Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko'


def download(url):
    request = urllib.request.Request(url, None, {'User-Agent': user_agent})
    out_file_path = os.path.join(master_dir, 'tmp.zip')
    with urllib.request.urlopen(request) as response, open(out_file_path, 'wb') as out_file:
        shutil.copyfileobj(response, out_file)
    with zipfile.ZipFile(out_file_path) as z:
        for name in z.namelist():
            csv_path = os.path.join(master_dir, name)
            print(csv_path)
            with z.open(name) as f1, open(csv_path, 'wb') as f2:
                shutil.copyfileobj(f1, f2)
            if name.startswith('b_'):
                new_csv_path = os.path.join(master_dir, 'b.csv')
                if os.path.exists(new_csv_path):
                    os.remove(new_csv_path)
                os.rename(csv_path, new_csv_path)
            elif name.startswith('z_'):
                # new_name = 'z.csv'
                # if os.path.exists(new_name):
                #     os.remove(new_name)
                # os.rename(name, new_name)
                new_csv_path = os.path.join(master_dir, 'z.csv')
                if os.path.exists(new_csv_path):
                    os.remove(new_csv_path)
                os.rename(csv_path, new_csv_path)
    os.remove(out_file_path)
    time.sleep(random.random() * 2)


download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/sFile')  # 医科診療行為マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/yFile')  # 医薬品マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/tFile')  # 特定器材マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/bFile')  # 傷病名マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/zFile')  # 修飾語マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/cFile')  # コメントマスター
