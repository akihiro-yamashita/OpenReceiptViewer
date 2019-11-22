/*
Copyright 2016 Akihiro Yamashita

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

    /// <summary>医療機関情報レコード</summary>
    public class IR : Record
    {
        /// <summary></summary>
        public IR() : base(レコード識別情報定数.医療機関情報) { }

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
        public int 医療機関コード
        {
            get { return this._医療機関コード; }
            set
            {
                this._医療機関コード = value;
                OnPropertyChanged("医療機関コード");
            }
        }
        private int _医療機関コード;

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
        public int 患者番号
        {
            get { return this._患者番号; }
            set
            {
                this._患者番号 = value;
                OnPropertyChanged("患者番号");
            }
        }
        private int _患者番号;
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
        public int? 受給者番号
        {
            get { return this._受給者番号; }
            set
            {
                this._受給者番号 = value;
                OnPropertyChanged("受給者番号");
            }
        }
        private int? _受給者番号;

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

    /// <summary></summary>
    public class Receipt
    {
        public RE RE { get; set; }

        /// <summary></summary>
        public HO HO { get; set; }

        /// <summary></summary>
        public KO KO { get; set; }

        /// <summary></summary>
        public List<SY> SYList { get; set; }

        /// <summary></summary>
        public List<SIIYTOCO> SIIYTOCOList { get; set; }
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

        /// <summary></summary>
        public int コメント1_コメントコード
        {
            get { return this._コメント1_コメントコード; }
            set
            {
                this._コメント1_コメントコード = value;
                OnPropertyChanged("コメント1_コメントコード");
            }
        }
        private int _コメント1_コメントコード;

        /// <summary></summary>
        public string コメント1_文字データ
        {
            get { return this._コメント1_文字データ; }
            set
            {
                this._コメント1_文字データ = value;
                OnPropertyChanged("コメント1_文字データ");
            }
        }
        private string _コメント1_文字データ;

        /// <summary></summary>
        public int コメント2_コメントコード
        {
            get { return this._コメント2_コメントコード; }
            set
            {
                this._コメント2_コメントコード = value;
                OnPropertyChanged("コメント2_コメントコード");
            }
        }
        private int _コメント2_コメントコード;

        /// <summary></summary>
        public string コメント2_文字データ
        {
            get { return this._コメント2_文字データ; }
            set
            {
                this._コメント2_文字データ = value;
                OnPropertyChanged("コメント2_文字データ");
            }
        }
        private string _コメント2_文字データ;

        /// <summary></summary>
        public int コメント3_コメントコード
        {
            get { return this._コメント3_コメントコード; }
            set
            {
                this._コメント3_コメントコード = value;
                OnPropertyChanged("コメント3_コメントコード");
            }
        }
        private int _コメント3_コメントコード;

        /// <summary></summary>
        public string コメント3_文字データ
        {
            get { return this._コメント3_文字データ; }
            set
            {
                this._コメント3_文字データ = value;
                OnPropertyChanged("コメント3_文字データ");
            }
        }
        private string _コメント3_文字データ;

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
    }

    /// <summary>診療報酬請求書レコード</summary>    
    public class GO : Record
    {
        /// <summary></summary>
        public GO() : base(レコード識別情報定数.診療報酬請求書) { }

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
