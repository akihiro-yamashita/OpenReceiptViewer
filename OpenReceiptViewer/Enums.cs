/*
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
        記録条件仕様年月情報 = 19,
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

    /// <summary>診療報酬請求書レコード</summary>
    public enum GO_IDX
    {
        総件数 = 1,
        総合計点数 = 2,
        マルチボリューム識別子 = 3,
    }

    /// <summary></summary>
    public enum 審査支払機関
    {
        社保 = 1,
        国保 = 2,
    }

    /// <summary>年号区分</summary>
    /// <remarks>別表２</remarks>
    public enum 年号区分
    {
        明治 = 1,
        大正 = 2,
        昭和 = 3,
        平成 = 4,
        令和 = 5,
    }

    /// <summary>本人区分</summary>
    /// <remarks>別表３</remarks>
    public enum 本人区分
    {
        本人,
        未就学者,
        家族,
        高齢受給者一般低所得者,
        高齢受給者７割,
        後期高齢一般低所得者,
        後期高齢７割,
    }

    /// <summary>入院入院外区分</summary>
    /// <remarks>別表３</remarks>
    public enum 入院入院外区分
    {
        入院,
        入院外,
    }

    /// <summary>男女区分</summary>
    /// <remarks>別表４</remarks>
    public enum 男女区分
    {
        男 = 1,
        女 = 2,
    }

    /// <summary>転帰区分</summary>
    /// <remarks>別表１５</remarks>
    public enum 転帰区分
    {
        継続 = 1,
        治癒 = 2,
        死亡 = 3,
        中止 = 4,
    }

    /// <summary>診療識別</summary>
    /// <remarks>別表１６</remarks>
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
    public enum MasterVersion
    {
        Ver201604 = 42804,
        Ver201804 = 43004,
        Ver201910 = 50110,
        Ver202004 = 50204,
    }

    ///// <summary>診療報酬マスターバージョン</summary>
    //public enum MasterVersion
    //{
    //    Ver201604 = 201604,
    //    Ver201804 = 201804,
    //    Ver201910 = 201910,
    //    Ver202004 = 202004,
    //}
}
