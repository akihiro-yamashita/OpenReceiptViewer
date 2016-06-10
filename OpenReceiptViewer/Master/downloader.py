__author__ = 'yamacl'

import os
import urllib.request
import shutil
import zipfile


user_agent = 'Mozilla/5.0 (Windows NT 6.1; Trident/7.0; rv:11.0) like Gecko'


def download(url):
    request = urllib.request.Request(url, None, {'User-Agent': user_agent})
    out_file_path = 'tmp.zip'
    with urllib.request.urlopen(request) as response, open(out_file_path, 'wb') as out_file:
        shutil.copyfileobj(response, out_file)
    with zipfile.ZipFile(out_file_path) as z:
        for name in z.namelist():
            print(name)
            with z.open(name) as f1, open(name, 'wb') as f2:
                shutil.copyfileobj(f1, f2)
    os.remove(out_file_path)


download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/sFile')  # 医科診療行為マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/yFile')  # 医薬品マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/tFile')  # 特定器材マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/bFile')  # 傷病名マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/zFile')  # 修飾語マスター
download('http://www.iryohoken.go.jp/shinryohoshu/downloadMenu/cFile')  # コメントマスター
