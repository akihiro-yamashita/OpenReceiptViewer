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
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    public class Record : NotifyPropertyChanged
    {
        //public int Id
        //{
        //    get { return this._id; }
        //    set
        //    {
        //        this._id = value;
        //    }
        //}
        //private int _id;

        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public Record(string レコード識別情報)
        {
            this._レコード識別情報 = レコード識別情報;
        }

        /// <summary></summary>
        public string レコード識別情報
        {
            get { return this._レコード識別情報; }
        }
        private string _レコード識別情報;
    }

    /// <summary>医療機関情報、返戻医療機関共通レコード</summary>
    public abstract class IRHI : Record
    {
        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public IRHI(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary></summary>
        public 審査支払機関 審査支払機関
        {
            get { return this._審査支払機関; }
            set
            {
                this._審査支払機関 = value;
                OnPropertyChanged("審査支払機関");
            }
        }
        private 審査支払機関 _審査支払機関;

        /// <summary></summary>
        public int 都道府県
        {
            get { return this._都道府県; }
            set
            {
                this._都道府県 = value;
                OnPropertyChanged("都道府県");
            }
        }
        private int _都道府県;

        /// <summary></summary>
        public int 点数表
        {
            get { return this._点数表; }
            set
            {
                this._点数表 = value;
                OnPropertyChanged("点数表");
            }
        }
        private int _点数表;

        /// <summary></summary>
        public string 医療機関コード
        {
            get { return this._医療機関コード; }
            set
            {
                this._医療機関コード = value;
                OnPropertyChanged("医療機関コード");
            }
        }
        private string _医療機関コード;

        /// <summary></summary>
        public int? 予備
        {
            get { return this._予備; }
            set
            {
                this._予備 = value;
                OnPropertyChanged("予備");
            }
        }
        private int? _予備;

        /// <summary></summary>
        public string 医療機関名称
        {
            get { return this._医療機関名称; }
            set
            {
                this._医療機関名称 = value;
                OnPropertyChanged("医療機関名称");
            }
        }
        private string _医療機関名称;

        /// <summary></summary>
        public int 請求年月
        {
            get { return this._請求年月; }
            set
            {
                this._請求年月 = value;
                OnPropertyChanged("請求年月");
            }
        }
        private int _請求年月;

        /// <summary></summary>
        public int マルチボリューム識別子
        {
            get { return this._マルチボリューム識別子; }
            set
            {
                this._マルチボリューム識別子 = value;
                OnPropertyChanged("マルチボリューム識別子");
            }
        }
        private int _マルチボリューム識別子;

        /// <summary></summary>
        public string 電話番号
        {
            get { return this._電話番号; }
            set
            {
                this._電話番号 = value;
                OnPropertyChanged("電話番号");
            }
        }
        private string _電話番号;
    }

    /// <summary>医療機関情報レコード</summary>
    public class IR : IRHI
    {
        /// <summary></summary>
        public IR() : base(レコード識別情報定数.医療機関情報) { }
    }

    /// <summary>返戻医療機関レコード</summary>
    public class HI : IRHI
    {
        /// <summary></summary>
        public HI() : base(レコード識別情報定数.返戻医療機関) { }
    }

    /// <summary>レセプト共通レコード</summary>
    public class RE : Record
    {
        /// <summary></summary>
        public RE() : base(レコード識別情報定数.レセプト共通) { }

        /// <summary></summary>
        public int レセプト番号
        {
            get { return this._レセプト番号; }
            set
            {
                this._レセプト番号 = value;
                OnPropertyChanged("レセプト番号");
            }
        }
        private int _レセプト番号;

        /// <summary></summary>
        public int? 履歴管理番号
        {
            get { return this._履歴管理番号; }
            set
            {
                this._履歴管理番号 = value;
                OnPropertyChanged("履歴管理番号");
                OnPropertyChanged("レセプト番号");
            }
        }
        private int? _履歴管理番号;

        /// <summary></summary>
        public int レセプト種別
        {
            get { return this._レセプト種別; }
            set
            {
                this._レセプト種別 = value;
                OnPropertyChanged("レセプト種別");
            }
        }
        private int _レセプト種別;

        /// <summary></summary>
        public int 診療年月
        {
            get { return this._診療年月; }
            set
            {
                this._診療年月 = value;
                OnPropertyChanged("診療年月");
            }
        }
        private int _診療年月;

        /// <summary></summary>
        public string 氏名
        {
            get { return this._氏名; }
            set
            {
                this._氏名 = value;
                OnPropertyChanged("氏名");
            }
        }
        private string _氏名;

        /// <summary></summary>
        public 男女区分 男女区分
        {
            get { return this._男女区分; }
            set
            {
                this._男女区分 = value;
                OnPropertyChanged("男女区分");
            }
        }
        private 男女区分 _男女区分;

        /// <summary></summary>
        public int 生年月日
        {
            get { return this._生年月日; }
            set
            {
                this._生年月日 = value;
                OnPropertyChanged("生年月日");
            }
        }
        private int _生年月日;

        /// <summary></summary>
        public int 入院年月日
        {
            get { return this._入院年月日; }
            set
            {
                this._入院年月日 = value;
                OnPropertyChanged("入院年月日");
            }
        }
        private int _入院年月日;

        /// <summary></summary>
        public string カルテ番号
        {
            get { return this._カルテ番号; }
            set
            {
                this._カルテ番号 = value;
                OnPropertyChanged("カルテ番号");
            }
        }
        private string _カルテ番号;

        /// <summary></summary>
        public string 検索番号
        {
            get { return this._検索番号; }
            set
            {
                this._検索番号 = value;
                OnPropertyChanged("検索番号");
            }
        }
        private string _検索番号;

        /// <summary></summary>
        public string カタカナ
        {
            get { return this._カタカナ; }
            set
            {
                this._カタカナ = value;
                OnPropertyChanged("カタカナ");
            }
        }
        private string _カタカナ;

        /// <summary></summary>
        public string 患者の状態
        {
            get { return this._患者の状態; }
            set
            {
                this._患者の状態 = value;
                OnPropertyChanged("患者の状態");
            }
        }
        private string _患者の状態;
    }

    /// <summary>保険者レコード</summary>
    public class HO : Record
    {
        /// <summary></summary>
        public HO() : base(レコード識別情報定数.保険者) { }

        /// <summary>保険者番号</summary>
        public int 保険者番号
        {
            get { return this._保険者番号; }
            set
            {
                this._保険者番号 = value;
                OnPropertyChanged("保険者番号");
            }
        }
        private int _保険者番号;

        /// <summary>被保険者証記号</summary>
        public string 被保険者証記号
        {
            get { return this._被保険者証記号; }
            set
            {
                this._被保険者証記号 = value;
                OnPropertyChanged("被保険者証記号");
            }
        }
        private string _被保険者証記号;

        /// <summary>被保険者証番号</summary>
        public string 被保険者証番号
        {
            get { return this._被保険者証番号; }
            set
            {
                this._被保険者証番号 = value;
                OnPropertyChanged("被保険者証番号");
            }
        }
        private string _被保険者証番号;

        /// <summary>診療実日数</summary>
        public int 診療実日数
        {
            get { return this._診療実日数; }
            set
            {
                this._診療実日数 = value;
                OnPropertyChanged("診療実日数");
            }
        }
        private int _診療実日数;

        /// <summary>合計点数</summary>
        public int 合計点数
        {
            get { return this._合計点数; }
            set
            {
                this._合計点数 = value;
                OnPropertyChanged("合計点数");
            }
        }
        private int _合計点数;

        /// <summary>予備</summary>
        public int? 予備
        {
            get { return this._予備; }
            set
            {
                this._予備 = value;
                OnPropertyChanged("予備");
            }
        }
        private int? _予備;

        /// <summary>回数</summary>
        public int? 回数
        {
            get { return this._回数; }
            set
            {
                this._回数 = value;
                OnPropertyChanged("回数");
            }
        }
        private int? _回数;

        /// <summary>保険合計点数</summary>
        public int? 保険合計点数
        {
            get { return this._保険合計点数; }
            set
            {
                this._保険合計点数 = value;
                OnPropertyChanged("保険合計点数");
            }
        }
        private int? _保険合計点数;

        /// <summary>公費合計点数</summary>
        public int? 公費合計点数
        {
            get { return this._公費合計点数; }
            set
            {
                this._公費合計点数 = value;
                OnPropertyChanged("公費合計点数");
            }
        }
        private int? _公費合計点数;

        /// <summary>職務上の事由</summary>
        public int? 職務上の事由
        {
            get { return this._職務上の事由; }
            set
            {
                this._職務上の事由 = value;
                OnPropertyChanged("職務上の事由");
            }
        }
        private int? _職務上の事由;

        /// <summary>証明証番号</summary>
        public int? 証明証番号
        {
            get { return this._証明証番号; }
            set
            {
                this._証明証番号 = value;
                OnPropertyChanged("証明証番号");
            }
        }
        private int? _証明証番号;

        /// <summary>医療保険</summary>
        public int? 医療保険
        {
            get { return this._医療保険; }
            set
            {
                this._医療保険 = value;
                OnPropertyChanged("医療保険");
            }
        }
        private int? _医療保険;

        /// <summary>減免区分</summary>
        public int? 減免区分
        {
            get { return this._減免区分; }
            set
            {
                this._減免区分 = value;
                OnPropertyChanged("減免区分");
            }
        }
        private int? _減免区分;

        /// <summary>減額割合</summary>
        public int? 減額割合
        {
            get { return this._減額割合; }
            set
            {
                this._減額割合 = value;
                OnPropertyChanged("減額割合");
            }
        }
        private int? _減額割合;

        /// <summary>減額金額</summary>
        public int? 減額金額
        {
            get { return this._減額金額; }
            set
            {
                this._減額金額 = value;
                OnPropertyChanged("減額金額");
            }
        }
        private int? _減額金額;
    }

    /// <summary>公費レコード</summary>
    public class KO : Record
    {
        /// <summary></summary>
        public KO() : base(レコード識別情報定数.公費) { }

        /// <summary>負担者番号</summary>
        public string 負担者番号
        {
            get { return this._負担者番号; }
            set
            {
                this._負担者番号 = value;
                OnPropertyChanged("負担者番号");
            }
        }
        private string _負担者番号;

        /// <summary>受給者番号</summary>
        public string 受給者番号
        {
            get { return this._受給者番号; }
            set
            {
                this._受給者番号 = value;
                OnPropertyChanged("受給者番号");
            }
        }
        private string _受給者番号;

        /// <summary>任意給付区分</summary>
        public int? 任意給付区分
        {
            get { return this._任意給付区分; }
            set
            {
                this._任意給付区分 = value;
                OnPropertyChanged("任意給付区分");
            }
        }
        private int? _任意給付区分;

        /// <summary>診療実日数</summary>
        public int 診療実日数
        {
            get { return this._診療実日数; }
            set
            {
                this._診療実日数 = value;
                OnPropertyChanged("診療実日数");
            }
        }
        private int _診療実日数;

        /// <summary>合計点数</summary>
        public int 合計点数
        {
            get { return this._合計点数; }
            set
            {
                this._合計点数 = value;
                OnPropertyChanged("合計点数");
            }
        }
        private int _合計点数;

        /// <summary>公費</summary>
        public int? 公費
        {
            get { return this._公費; }
            set
            {
                this._公費 = value;
                OnPropertyChanged("公費");
            }
        }
        private int? _公費;

        /// <summary>外来一部負担金</summary>
        public int? 外来一部負担金
        {
            get { return this._外来一部負担金; }
            set
            {
                this._外来一部負担金 = value;
                OnPropertyChanged("外来一部負担金");
            }
        }
        private int? _外来一部負担金;

        /// <summary>入院一部負担金</summary>
        public int? 入院一部負担金
        {
            get { return this._入院一部負担金; }
            set
            {
                this._入院一部負担金 = value;
                OnPropertyChanged("入院一部負担金");
            }
        }
        private int? _入院一部負担金;

        /// <summary>予備</summary>
        public int? 予備
        {
            get { return this._予備; }
            set
            {
                this._予備 = value;
                OnPropertyChanged("予備");
            }
        }
        private int? _予備;

        /// <summary>回数</summary>
        public int? 回数
        {
            get { return this._回数; }
            set
            {
                this._回数 = value;
                OnPropertyChanged("回数");
            }
        }
        private int? _回数;

        /// <summary>合計金額</summary>
        public int? 合計金額
        {
            get { return this._合計金額; }
            set
            {
                this._合計金額 = value;
                OnPropertyChanged("合計金額");
            }
        }
        private int? _合計金額;
    }

    /// <summary>資格確認レコード</summary>
    public class SN : Record
    {
        /// <summary></summary>
        public SN() : base(レコード識別情報定数.資格確認) { }

        /// <summary>枝番</summary>
        public string 枝番
        {
            get { return this._枝番; }
            set
            {
                this._枝番 = value;
                OnPropertyChanged("枝番");
            }
        }
        private string _枝番;
    }

    /// <summary>受診日レコード</summary>
    public class JD : Record
    {
        /// <summary></summary>
        public JD() : base(レコード識別情報定数.受診日) { }
    }

    /// <summary>窓口負担額レコード</summary>
    public class MF : Record
    {
        /// <summary></summary>
        public MF() : base(レコード識別情報定数.窓口負担額) { }
    }

    /// <summary>包括評価対象外理由レコード</summary>
    public class GR : Record
    {
        /// <summary></summary>
        public GR() : base(レコード識別情報定数.包括評価対象外理由) { }
    }

    /// <summary></summary>
    public class Receipt
    {
        /// <summary>レセプト共通</summary>
        public RE RE { get; set; }

        /// <summary>保険者</summary>
        public HO HO { get; set; }

        /// <summary>資格確認</summary>
        public SN SN { get; set; }

        /// <summary>公費4つまで</summary>
        public List<KO> KOList { get; set; }

        /// <summary>傷病名</summary>
        public List<SY> SYList { get; set; }

        /// <summary>明細共通</summary>
        public List<SIIYTOCO> SIIYTOCOList { get; set; }

        /// <summary>履歴管理情報共通</summary>
        public List<HRJYONRC> HRJYONRCList { get; set; }
    }

    /// <summary>傷病名レコード</summary>
    public class SY : Record
    {
        /// <summary></summary>
        public SY() : base(レコード識別情報定数.傷病名) { }

        /// <summary></summary>
        public int 傷病名コード
        {
            get { return this._傷病名コード; }
            set
            {
                this._傷病名コード = value;
                OnPropertyChanged("傷病名コード");
            }
        }
        private int _傷病名コード;

        /// <summary></summary>
        public int 診療開始日
        {
            get { return this._診療開始日; }
            set
            {
                this._診療開始日 = value;
                OnPropertyChanged("診療開始日");
            }
        }
        private int _診療開始日;

        /// <summary></summary>
        public 転帰区分 転帰区分
        {
            get { return this._転帰区分; }
            set
            {
                this._転帰区分 = value;
                OnPropertyChanged("転帰区分");
            }
        }
        private 転帰区分 _転帰区分;

        /// <summary></summary>
        public string 修飾語コード
        {
            get { return this._修飾語コード; }
            set
            {
                this._修飾語コード = value;
                OnPropertyChanged("修飾語コード");
            }
        }
        private string _修飾語コード;

        /// <summary></summary>
        public string 傷病名称
        {
            get { return this._傷病名称; }
            set
            {
                this._傷病名称 = value;
                OnPropertyChanged("傷病名称");
            }
        }
        private string _傷病名称;

        /// <string></summary>
        public string 主傷病
        {
            get { return this._主傷病; }
            set
            {
                this._主傷病 = value;
                OnPropertyChanged("主傷病");
            }
        }
        private string _主傷病;

        /// <summary></summary>
        public string 補足コメント
        {
            get { return this._補足コメント; }
            set
            {
                this._補足コメント = value;
                OnPropertyChanged("補足コメント");
            }
        }
        private string _補足コメント;
    }

    /// <summary>明細共通レコード</summary>
    public abstract class SIIYTOCO : Record
    {
        public RE RE { get; set; }

        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public SIIYTOCO(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary>診療識別</summary>
        public int? 診療識別
        {
            get { return this._診療識別; }
            set
            {
                this._診療識別 = value;
                OnPropertyChanged("診療識別");
            }
        }
        private int? _診療識別;

        /// <summary>負担区分</summary>
        public string 負担区分
        {
            get { return this._負担区分; }
            set
            {
                this._負担区分 = value;
                OnPropertyChanged("負担区分");
            }
        }
        private string _負担区分;

        /// <summary>内容</summary>
        public abstract object 内容 { get; }

        /// <summary>数量</summary>
        public abstract float? 数量 { get; set; }

        /// <summary>点数</summary>
        public abstract int? 点数 { get; set; }

        /// <summary>回数</summary>
        public abstract int 回数 { get; set; }

        /// <summary>XX日の情報</summary>
        public abstract Dictionary<int, int> XX日の情報 { get; set; }
    }

    /// <summary>診療行為と医薬品と特定器材レコード</summary>
    public abstract class SIIYTO : SIIYTOCO
    {
        /// <summary>SIIYTO行内のコメント</summary>
        public class コメント : NotifyPropertyChanged
        {
            /// <summary></summary>
            public int コメントコード
            {
                get { return this._コメントコード; }
                set
                {
                    this._コメントコード = value;
                    OnPropertyChanged("コメントコード");
                }
            }
            private int _コメントコード;

            /// <summary></summary>
            public string 文字データ
            {
                get { return this._文字データ; }
                set
                {
                    this._文字データ = value;
                    OnPropertyChanged("文字データ");
                }
            }
            private string _文字データ;
        }

        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public SIIYTO(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary></summary>
        public int コード
        {
            get { return this._コード; }
            set
            {
                this._コード = value;
                OnPropertyChanged("コード");
                OnPropertyChanged("内容");
            }
        }
        private int _コード;

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return this.コード; }
        }

        /// <summary></summary>
        public override float? 数量
        {
            get { return this._数量; }
            set
            {
                this._数量 = value;
                OnPropertyChanged("数量");
            }
        }
        private float? _数量;

        /// <summary></summary>
        public override int? 点数
        {
            get { return this._点数; }
            set
            {
                this._点数 = value;
                OnPropertyChanged("点数");
            }
        }
        private int? _点数;

        /// <summary></summary>
        public override int 回数
        {
            get { return this._回数; }
            set
            {
                this._回数 = value;
                OnPropertyChanged("回数");
            }
        }
        private int _回数;

        /// <summary>コメント1～3</summary>
        public List<コメント> コメントList
        {
            get { return this._コメントList; }
            set
            {
                this._コメントList = value;
                OnPropertyChanged("コメントList");
            }
        }
        private List<コメント> _コメントList;

        /// <summary>XX日の情報</summary>
        public override Dictionary<int, int> XX日の情報
        {
            get { return this._XX日の情報; }
            set
            {
                this._XX日の情報 = value;
                OnPropertyChanged("XX日の情報");
            }
        }
        private Dictionary<int, int> _XX日の情報;
    }

    /// <summary>診療行為レコード</summary>
    public class SI : SIIYTO
    {
        /// <summary></summary>
        public SI() : base(レコード識別情報定数.診療行為) { }

        /// <summary></summary>
        public int 診療行為コード
        {
            get { return this.コード; }
            set { this.コード = value; }
        }
    }

    /// <summary>医薬品レコード</summary>
    public class IY : SIIYTO
    {
        /// <summary></summary>
        public IY() : base(レコード識別情報定数.医薬品) { }

        /// <summary></summary>
        public int 医薬品コード
        {
            get { return this.コード; }
            set { this.コード = value; }
        }
    }

    /// <summary>特定器材レコード</summary>
    public class TO : SIIYTO
    {
        /// <summary></summary>
        public TO() : base(レコード識別情報定数.特定器材) { }

        /// <summary></summary>
        public int 特定器材コード
        {
            get { return this.コード; }
            set { this.コード = value; }
        }

        /// <summary>単位コード</summary>
        public int? 単位コード
        {
            get { return this._単位コード; }
            set
            {
                this._単位コード = value;
                OnPropertyChanged("単位コード");
            }
        }
        private int? _単位コード;

        /// <summary>単価</summary>
        public float? 単価
        {
            get { return this._単価; }
            set
            {
                this._単価 = value;
                OnPropertyChanged("単価");
            }
        }
        private float? _単価;

        /// <summary>特定器材名称</summary>
        public string 特定器材名称
        {
            get { return this._特定器材名称; }
            set
            {
                this._特定器材名称 = value;
                OnPropertyChanged("特定器材名称");
            }
        }
        private string _特定器材名称;

        /// <summary>商品名及び規格</summary>
        public string 商品名及び規格
        {
            get { return this._商品名及び規格; }
            set
            {
                this._商品名及び規格 = value;
                OnPropertyChanged("商品名及び規格");
            }
        }
        private string _商品名及び規格;
    }

    /// <summary>コメントレコード</summary>
    public class CO : SIIYTOCO
    {
        /// <summary></summary>
        public CO() : base(レコード識別情報定数.コメント) { }

        /// <summary></summary>
        public int コメントコード
        {
            get { return this._コメントコード; }
            set
            {
                this._コメントコード = value;
                OnPropertyChanged("コメントコード");
                OnPropertyChanged("内容");
            }
        }
        private int _コメントコード;

        /// <summary></summary>
        public string 文字データ
        {
            get { return this._文字データ; }
            set
            {
                this._文字データ = value;
                OnPropertyChanged("文字データ");
                OnPropertyChanged("内容");
            }
        }
        private string _文字データ;

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return new Tuple<int, string>(this.コメントコード, this.文字データ); }
        }

        /// <summary></summary>
        public override float? 数量
        {
            get { return null; }
            set { }
        }

        /// <summary></summary>
        public override int? 点数
        {
            get { return null; }
            set { }
        }

        /// <summary></summary>
        public override int 回数
        {
            get { return 0; }
            set { }
        }

        /// <summary>XX日の情報</summary>
        public override Dictionary<int, int> XX日の情報
        {
            get { return null; }
            set { }
        }

        /// <summary>コメントレコードに存在しない項目だが、診療行為や医薬品と一律で扱う上でのバインドエラー対策</summary>
        public List<object> コメントList { get; set; }
    }

    public abstract class EmptySIIYTOCO : SIIYTOCO
    {
        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public EmptySIIYTOCO(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary>数量</summary>
        public override float? 数量
        {
            get { return null; }
            set { }
        }

        /// <summary>点数</summary>
        public override int? 点数
        {
            get { return null; }
            set { }
        }

        /// <summary>回数</summary>
        public override int 回数
        {
            get { return 0; }
            set { }
        }

        /// <summary>XX日の情報</summary>
        public override Dictionary<int, int> XX日の情報
        {
            get { return null; }
            set { }
        }

        /// <summary>コメントレコードに存在しない項目だが、診療行為や医薬品と一律で扱う上でのバインドエラー対策</summary>
        public List<object> コメントList { get; set; }
    }

    /// <summary>症状詳記レコード</summary>
    public class SJ : EmptySIIYTOCO
    {
        /// <summary></summary>
        public SJ() : base(レコード識別情報定数.症状詳記) { }

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return 症状詳記データ; }
        }

        /// <summary></summary>
        public int? 症状詳記区分
        {
            get { return this._症状詳記区分; }
            set
            {
                this._症状詳記区分 = value;
                OnPropertyChanged("症状詳記区分");
            }
        }
        private int? _症状詳記区分;

        /// <summary></summary>
        public string 症状詳記データ
        {
            get { return this._症状詳記データ; }
            set
            {
                this._症状詳記データ = value;
                OnPropertyChanged("症状詳記データ");
                OnPropertyChanged("内容");
            }
        }
        private string _症状詳記データ;
    }

    /// <summary>履歴管理情報関連レコード</summary>
    public abstract class HRJYONRC : Record
    {
        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public HRJYONRC(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary>内容</summary>
        public abstract object 内容 { get; }
    }

    /// <summary>返戻理由レコード</summary>
    public class HR : HRJYONRC
    {
        /// <summary></summary>
        public HR() : base(レコード識別情報定数.返戻理由) { }

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return 返戻理由; }
        }

        /// <summary></summary>
        public int 処理年月
        {
            get { return this._処理年月; }
            set
            {
                this._処理年月 = value;
                OnPropertyChanged("処理年月");
            }
        }
        private int _処理年月;

        /// <summary></summary>
        public string 返戻理由
        {
            get { return this._返戻理由; }
            set
            {
                this._返戻理由 = value;
                OnPropertyChanged("返戻理由");
                OnPropertyChanged("内容");
            }
        }
        private string _返戻理由;
    }

    /// <summary>事由レコード</summary>
    public class JY : HRJYONRC
    {
        /// <summary></summary>
        public JY() : base(レコード識別情報定数.事由) { }

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return "未実装"; }
        }
    }

    /// <summary>資格確認運用レコード</summary>
    public class ON : HRJYONRC
    {
        /// <summary></summary>
        public ON() : base(レコード識別情報定数.資格確認運用) { }

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return コード; }
        }

        /// <summary></summary>
        public string コード
        {
            get { return this._コード; }
            set
            {
                this._コード = value;
                OnPropertyChanged("コード");
                OnPropertyChanged("内容");
            }
        }
        private string _コード;
    }

    /// <summary>レコード管理レコード</summary>
    public class RC : HRJYONRC
    {
        /// <summary></summary>
        public RC() : base(レコード識別情報定数.レコード管理) { }

        /// <summary>内容</summary>
        public override object 内容
        {
            get { return 管理情報; }
        }

        /// <summary></summary>
        public string 管理情報
        {
            get { return this._管理情報; }
            set
            {
                this._管理情報 = value;
                OnPropertyChanged("管理情報");
                OnPropertyChanged("内容");
            }
        }
        private string _管理情報;
    }

    /// <summary>診療報酬請求書、返戻合計共通レコード</summary>    
    public class GOHG : Record
    {
        /// <summary></summary>
        /// <param name="レコード識別情報"></param>
        public GOHG(string レコード識別情報) : base(レコード識別情報) { }

        /// <summary></summary>
        public int 総件数
        {
            get { return this._総件数; }
            set
            {
                this._総件数 = value;
                OnPropertyChanged("総件数");
            }
        }
        private int _総件数;

        /// <summary></summary>
        public int 総合計点数
        {
            get { return this._総合計点数; }
            set
            {
                this._総合計点数 = value;
                OnPropertyChanged("総合計点数");
            }
        }
        private int _総合計点数;

        /// <summary></summary>
        public int マルチボリューム識別子
        {
            get { return this._マルチボリューム識別子; }
            set
            {
                this._マルチボリューム識別子 = value;
                OnPropertyChanged("マルチボリューム識別子");
            }
        }
        private int _マルチボリューム識別子;
    }

    /// <summary></summary>
    public class 名称単位マスター
    {
        /// <summary></summary>
        public int Id { get; set; }

        /// <summary></summary>
        public string 名称 { get; set; }

        /// <summary></summary>
        public string 単位 { get; set; }
    }

    /// <summary></summary>
    public class コメントマスター
    {
        /// <summary>8しか入らない。</summary>
        public int 区分 { get; set; }

        /// <summary></summary>
        public int パターン { get; set; }

        /// <summary></summary>
        public int 一連番号 { get; set; }

        /// <summary></summary>
        public string 漢字名称 { get; set; }

        /// <summary></summary>
        public List<Tuple<int, int>> カラム位置桁数 { get; set; }

        /// <summary></summary>
        public int コメントコード
        {
            get
            {
                return (区分 * 100000000) + (パターン * 1000000) + 一連番号;
            }
        }

        /// <summary></summary>
        public static コメントマスター Empty = new コメントマスター();
    }

}
