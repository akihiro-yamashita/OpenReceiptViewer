#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import csv
import glob


cur_dir = os.path.dirname(__file__)
master_dir = os.path.join(cur_dir)


MASTER_S_Y_T_IDX_コード = 2
MASTER_S_Y_T_IDX_名称 = 4
MASTER_S_Y_T_IDX_単位コード = 7
MASTER_S_Y_T_IDX_単位 = 9
MASTER_Y_IDX_YJコード = 31
MASTER_B_IDX_コード = 2
MASTER_B_IDX_名称 = 5
MASTER_Z_IDX_コード = 2
MASTER_Z_IDX_名称 = 6
MASTER_C_IDX_区分 = 2
MASTER_C_IDX_パターン = 3
MASTER_C_IDX_一連番号 = 4
MASTER_C_IDX_漢字名称 = 6
MASTER_C_IDX_位置1 = 9
MASTER_C_IDX_桁数1 = 10
MASTER_C_IDX_位置2 = 11
MASTER_C_IDX_桁数2 = 12
MASTER_C_IDX_位置3 = 13
MASTER_C_IDX_桁数3 = 14
MASTER_C_IDX_位置4 = 15
MASTER_C_IDX_桁数4 = 16
MASTER_C_IDX_コード = 22


def minify(sub_dir_name: str):
    sub_dir_path = os.path.join(master_dir, sub_dir_name)
    for file_name in glob.glob('*.*', root_dir=sub_dir_path):
        if 0 <= file_name.find('min'):
            continue
        csv_path = os.path.join(sub_dir_path, file_name)
        if file_name.startswith('s_') or file_name.startswith('y_') or file_name.startswith('t_'):
            def rewrite_row(row: list[str]):
                code = row[MASTER_S_Y_T_IDX_コード]
                name = row[MASTER_S_Y_T_IDX_名称]
                unit_code = row[MASTER_S_Y_T_IDX_単位コード]
                unit = row[MASTER_S_Y_T_IDX_単位]
                #       0     1     2     3     4     5     6     7     8     9
                return (None, None, code, None, name, None, None, None, None, unit, )
        elif file_name.startswith('b_'):
            def rewrite_row(row: list[str]):
                code = row[MASTER_B_IDX_コード]
                name = row[MASTER_B_IDX_名称]
                #       0     1     2     3     4     5
                return (None, None, code, None, None, name, )
        elif file_name.startswith('z_'):
            def rewrite_row(row: list[str]):
                code = row[MASTER_Z_IDX_コード]
                name = row[MASTER_Z_IDX_名称]
                #       0     1     2     3     4     5     6
                return (None, None, code, None, None, None, name, )
        elif file_name.startswith('c_'):
            # 必要な列が多いため、最小化してもあまり容量削減にならない。
            def rewrite_row(row: list[str]):
                return (
                    None,                           # 0
                    None,                           # 1
                    row[MASTER_C_IDX_区分],         # 2
                    row[MASTER_C_IDX_パターン],     # 3
                    row[MASTER_C_IDX_一連番号],     # 4
                    None,                           # 5
                    row[MASTER_C_IDX_漢字名称],     # 6
                    None,                           # 7
                    None,                           # 8
                    row[MASTER_C_IDX_位置1],        # 9
                    row[MASTER_C_IDX_桁数1],
                    row[MASTER_C_IDX_位置2],
                    row[MASTER_C_IDX_桁数2],
                    row[MASTER_C_IDX_位置3],
                    row[MASTER_C_IDX_桁数3],
                    row[MASTER_C_IDX_位置4],
                    row[MASTER_C_IDX_桁数4],
                    None,                           # 17
                    None,                           # 18
                    None,                           # 19
                    None,                           # 20
                    None,                           # 21
                    row[MASTER_C_IDX_コード],       # 22
                )
        else:
            continue

        csv_min_path = csv_path[0:-13] + '.min.csv' # len('_YYYYMMDD.csv') == 13
        print(csv_min_path)
        reader = csv.reader(open(csv_path, encoding='shift_jis'))
        writer = csv.writer(open(csv_min_path, 'w', encoding='shift_jis', newline=''))
        for row in reader:
            new_row = rewrite_row(row)
            writer.writerow(new_row)


if __name__ == '__main__':
    #minify('202404')
    minify('202406')
