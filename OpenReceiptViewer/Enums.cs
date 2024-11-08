﻿/*
Copyright Since 2016 Akihiro Yamashita

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    #region 各レコードのインデックス

    /// <summary>医療機関情報レコード</summary>
    public enum IR_IDX
    {
        審査支払機関 = 1,
        都道府県 = 2,
        点数表 = 3,
        医療機関コード = 4,
        予備 = 5,
        医療機関名称 = 6,
        請求年月 = 7,
        マルチボリューム識別子 = 8,
        電話番号 = 9,
    }

    /// <summary>レセプト共通レコード</summary>
    public enum RE_IDX
    {
        レセプト番号 = 1,
        レセプト種別 = 2,
        診療年月 = 3,
        氏名 = 4,
        男女区分 = 5,
        生年月日 = 6,
        給付割合 = 7,
        入院年月日 = 8,
        病棟区分 = 9,
        一部負担金等区分 = 10,
        レセプト特記事項 = 11,
        病床数 = 12,
        カルテ番号等 = 13,
        割引点数単価 = 14,
        予備1 = 15,
        予備2 = 16,
        予備3 = 17,
        検索番号 = 18,
        記録条件仕様年月情報 = 19,  // TODO: 項目名なんか違う。
        請求情報 = 20,
        診療科1_診療科名 = 21,
        診療科1_人体の部位等 = 22,
        診療科1_性別等 = 23,
        診療科1_医学的処置 = 24,
        診療科1_特定疾病 = 25,
        診療科2_診療科名 = 26,
        診療科2_人体の部位等 = 27,
        診療科2_性別等 = 28,
        診療科2_医学的処置 = 29,
        診療科2_特定疾病 = 30,
        診療科3_診療科名 = 31,
        診療科3_人体の部位等 = 32,
        診療科3_性別等 = 33,
        診療科3_医学的処置 = 34,
        診療科3_特定疾病 = 35,
        カタカナ = 36,  // H30年4月以降
        患者の状態 = 37,  // H30年4月以降
    }

    /// <summary>保険者レコード</summary>
    public enum HO_IDX
    {
        保険者番号 = 1,
        被保険者証記号 = 2,
        被保険者証番号 = 3,
        診療実日数 = 4,
        合計点数 = 5,
        予備 = 6,
        回数 = 7,
        合計金額 = 8,
        職務上の事由 = 9,
        証明証番号 = 10,
        医療保険 = 11,
        減免区分 = 12,
        減額割合 = 13,
        減額金額 = 14,
    }

    /// <summary>公費レコード</summary>
    public enum KO_IDX
    {
        負担者番号 = 1,
        受給者番号 = 2,
        任意給付区分 = 3,
        診療実日数 = 4,
        合計点数 = 5,
        公費 = 6,
        外来一部負担金 = 7,
        入院一部負担金 = 8,
        予備 = 9,
        回数 = 10,
        合計金額 = 11,
    }

    /// <summary>資格確認レコード</summary>
    public enum SN_IDX
    {
        負担者種別 = 1,
        確認区分 = 2,
        保険者番号 = 3,
        被保険者証記号 = 4,
        被保険者証番号 = 5,
        枝番 = 6,
        受給者番号 = 7,
        予備 = 8,
    }

    /// <summary>受診日レコード</summary>
    public enum JD_IDX
    {
        負担者種別 = 1,
        X01日の情報 = 2,
        X31日の情報 = 32,
    }

    /// <summary>窓口負担額レコード</summary>
    public enum MF_IDX
    {
        窓口負担額区分 = 1,
        予備01 = 2,
        予備31 = 32,
    }

    /// <summary>包括評価対象外理由レコード</summary>
    public enum GR_IDX
    {
        医科点数表算定理由 = 1,
        DPCコード = 2,
    }

    /// <summary>傷病名レコード</summary>
    public enum SY_IDX
    {
        傷病名コード = 1,
        診療開始日 = 2,
        転帰区分 = 3,
        修飾語コード = 4,
        傷病名称 = 5,
        主傷病 = 6,
        補足コメント = 7,
    }

    /// <summary>診療行為と医薬品レコード</summary>
    public enum SI_IY_IDX
    {
        診療識別 = 1,
        負担区分 = 2,
        診療行為または医薬品コード = 3,
        数量 = 4,
        点数 = 5,
        回数 = 6,
        コメント1_コメントコード = 7,
        コメント1_文字データ = 8,
        コメント2_コメントコード = 9,
        コメント2_文字データ = 10,
        コメント3_コメントコード = 11,
        コメント3_文字データ = 12,
        X01日の情報 = 13,
        X31日の情報 = 43,
    }

    /// <summary>特定器材レコード</summary>
    public enum TO_IDX
    {
        診療識別 = 1,
        負担区分 = 2,
        特定器材コード = 3,
        使用量 = 4,
        点数 = 5,
        回数 = 6,
        単位コード = 7,
        単価 = 8,
        特定器材名称 = 9,
        商品名及び規格 = 10,
        コメント1_コメントコード = 11,
        コメント1_文字データ = 12,
        コメント2_コメントコード = 13,
        コメント2_文字データ = 14,
        コメント3_コメントコード = 15,
        コメント3_文字データ = 16,
        X01日の情報 = 17,
        X31日の情報 = 47,
    }

    /// <summary>コメントレコード</summary>
    public enum CO_IDX
    {
        診療識別 = 1,
        負担区分 = 2,
        コメントコード = 3,
        文字データ = 4,
    }

    /// <summary>症状詳記レコード</summary>
    public enum SJ_IDX
    {
        症状詳記区分 = 1,
        症状詳記データ = 2,
    }

    /// <summary>返戻医療機関レコード</summary>
    /// <remarks>IRと微妙に違う。仕様を考えた人センス無い。</remarks>
    public enum HI_IDX
    {
        審査支払機関 = 1,
        請求年月 = 2,
        都道府県 = 3,
        点数表 = 4,
        医療機関コード = 5,
        予備 = 6,
        マルチボリューム識別子 = 7,
    }

    /// <summary>返戻理由レコード</summary>
    public enum HR_IDX
    {
        処理年月 = 1,
        不明2 = 2,
        不明3 = 3,
        不明4 = 4,
        返戻理由 = 5,
        不明6 = 6,
        不明7 = 7,
        不明8 = 8,
        不明9 = 9,
        不明10 = 10,
        不明11 = 11,
        不明12 = 12,
        不明13 = 13,
    }

    /// <summary>事由レコード</summary>
    /// <remarks>仕様不明</remarks>
    public enum JY_IDX
    {
    }

    /// <summary>資格確認運用レコード</summary>
    /// <remarks>仕様不明</remarks>
    public enum ON_IDX
    {
        不明1 = 1,
        不明2 = 2,
        不明3 = 3,
        不明4 = 4,
        不明5 = 5,
        不明6 = 6,
        不明7 = 7,
        不明8 = 8,
        不明9 = 9,
        不明10 = 10,
        不明11 = 11,
        不明12 = 12,
        不明13 = 13,
        コード = 14,
    }

    /// <summary>レコード管理レコード</summary>
    public enum RC_IDX
    {
        管理情報 = 1,
    }

    /// <summary>診療報酬請求書、返戻合計共通レコード</summary>
    public enum GOHG_IDX
    {
        総件数 = 1,
        総合計点数 = 2,
        マルチボリューム識別子 = 3,
    }

    /// <summary>傷病名マスターレコード</summary>
    /// <remarks>Masterフォルダb.csv用</remarks>
    public enum MASTER_B_IDX
    {
        傷病名コード = 2,
        傷病名基本名称 = 5,
    }

    /// <summary>修飾語マスターレコード</summary>
    /// <remarks>Masterフォルダz.csv用</remarks>
    public enum MASTER_Z_IDX
    {
        修飾語コード = 2,
        修飾語名称 = 6,
    }

    /// <summary>診療行為・医薬品・特定器材マスターレコード</summary>
    /// <remarks>Masterフォルダs.csv,y.csv,t.csv用</remarks>
    public enum MASTER_S_Y_T_IDX
    {
        コード = 2,
        名称 = 4,
        単位 = 9,
    }

    /// <summary>コメントマスターレコード</summary>
    /// <remarks>Masterフォルダc.csv用</remarks>
    public enum MASTER_C_IDX
    {
        区分 = 2,
        パターン = 3,
        一連番号 = 4,
        漢字名称 = 6,
        カラム1位置 = 9,
        カラム1桁数 = 10,
        カラム2位置 = 11,
        カラム2桁数 = 12,
        カラム3位置 = 13,
        カラム3桁数 = 14,
        カラム4位置 = 15,
        カラム4桁数 = 16,
        //予備1 = 17,
        //予備2 = 18,
        //選択式コメント識別 = 19,
        //変更年月日 = 20,
        //廃止年月日 = 21,
        コメントコード = 22,
        //公表順序番号 = 23,
    }

    #endregion

    /// <summary>審査支払機関</summary>
    /// <remarks>別表1</remarks>
    public enum 審査支払機関
    {
        社保 = 1,
        国保 = 2,
    }

    // 使っていない。レセプト種別Converter参照。
    ///// <summary>本人区分</summary>
    ///// <remarks>別表5の3桁目</remarks>
    //public enum 本人区分
    //{
    //    本人,
    //    未就学者,
    //    家族,
    //    高齢受給者一般低所得者,
    //    高齢受給者７割,
    //    後期高齢一般低所得者,
    //    後期高齢７割,
    //}
    ///// <summary>入院入院外区分</summary>
    ///// <remarks>別表5の4桁目</remarks>
    //public enum 入院入院外区分
    //{
    //    入院,
    //    入院外,
    //}

    /// <summary>年号区分</summary>
    /// <remarks>別表4、削除済み</remarks>
    public enum 年号区分
    {
        明治 = 1,
        大正 = 2,
        昭和 = 3,
        平成 = 4,
        令和 = 5,
    }

    /// <summary>男女区分</summary>
    /// <remarks>別表6</remarks>
    public enum 男女区分
    {
        男 = 1,
        女 = 2,
    }

    /// <summary>転帰区分</summary>
    /// <remarks>別表18</remarks>
    public enum 転帰区分
    {
        継続 = 1,
        治癒 = 2,
        死亡 = 3,
        中止 = 4,
    }

    /// <summary>診療識別</summary>
    /// <remarks>別表20</remarks>
    public enum 診療識別
    {
        全体に係る識別コード01 = 1,
        初診 = 11,
        再診 = 12,
        医学管理 = 13,
        在宅 = 14,
        内服 = 21,
        屯服 = 22,
        外用 = 23,
        調剤 = 24,
        処方 = 25,
        麻毒 = 26,
        調基 = 27,
        その他投薬 = 28,
        皮下筋肉内注射 = 31,
        静脈内注射 = 32,
        その他注射 = 33,
        薬剤料減点 = 39,
        処置 = 40,
        手術 = 50,
        麻酔 = 54,
        検査_病理 = 60,
        画像診断 = 70,
        その他 = 80,
        入院基本料 = 90,
        特定入院料 = 92,
        食事療法_生活療法_標準負担額 = 97,
        全体に係る識別コード99 = 99,
    }

    /// <summary>診療報酬マスターバージョン</summary>
    public enum MasterVersionOld
    {
        // 4: 平成、5: 令和
        Ver201604 = 42804,
        Ver201804 = 43004,
        Ver201910 = 50110,
        Ver202004 = 50204,
        Ver202104 = 50304,
        Ver202201 = 50401,
        Ver202204 = 50404,
        Ver202207 = 50407,
        Ver202210 = 50410,
        Ver202304 = 50504,
        Ver202406 = 50606,
    }

    /// <summary>診療報酬マスターバージョン</summary>
    public enum MasterVersion
    {
        Ver201604 = 201604,
        Ver201804 = 201804,
        Ver201910 = 201910,
        Ver202004 = 202004,
        Ver202104 = 202104,

        // 実際は2021/12/31にマスター更新があったので、31日の診療行為の表示がバグる。
        // 根本的には全ての診療行為の日付ごとにマスターを切り替えないといけない。
        Ver202201 = 202201,
        Ver202204 = 202204,
        Ver202207 = 202207,
        Ver202210 = 202210,
        Ver202304 = 202304,
        Ver202406 = 202406,
    }

    /// <summary></summary>
    public enum 病棟種別
    {
        高度急性期 = 19061,
        急性期 = 19062,
        回復期 = 19063,
        慢性期 = 19064,
    }

    /// <summary>症状詳記区分コード</summary>
    /// <remarks>別表23</remarks>
    public enum 症状詳記区分コード
    {
        診断根拠となる臨床症状 = 1,
        診断根拠となる臨床症状の所見 = 2,
        治療行為の必要性 = 3,
        治療行為の経過 = 4,
        高額薬剤に係る症状 = 5,
        高額処置に係る症状 = 6,
        その他 = 7,
        治験概要 = 50,
        リハビリに係る治療継続の理由 = 51,
        廃用症候群をもたらすに至った要因 = 52,
        規定に基づく診療報酬明細書以外の症状詳記 = 90,
    }
}
