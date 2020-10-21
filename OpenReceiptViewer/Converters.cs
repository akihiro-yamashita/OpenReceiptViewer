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
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
#if DEBUG
    public class DebugConverter : TypeSafeConverter<string, object>
    {
        public override string Convert(object value, object parameter)
        {
            if (value == null)
            {
                return "null";
            }
            else
            {
                return string.Format("type: {0}, value: {1}", value.GetType().ToString(), value.ToString());
            }
        }

        public static DebugConverter Instance
        {
            get { return _instance = _instance ?? new DebugConverter(); }
        }
        private static DebugConverter _instance;
    }
#endif

    public class 日付表示Converter : TypeSafeConverter<string, int>
    {
        /// <summary></summary>
        public bool WithoutDay { get; private set; }

        public override string Convert(int value, object parameter)
        {
            return DateUtil.ReceiptDateToShowDate(value, this.WithoutDay);
        }

        public static 日付表示Converter 年月日表示Instance
        {
            get { return _年月日表示instance = _年月日表示instance ?? new 日付表示Converter() { WithoutDay = false }; }
        }
        private static 日付表示Converter _年月日表示instance;

        public static 日付表示Converter 年月表示Instance
        {
            get { return _年月表示instance = _年月表示instance ?? new 日付表示Converter() { WithoutDay = true }; }
        }
        private static 日付表示Converter _年月表示instance;
    }

    public class 年齢Converter : TypeSafeConverter<object, int>
    {
        public override object Convert(int value, object parameter)
        {
            var birthDay = DateUtil.ReceiptDateToDateTime(value);
            if (birthDay == null)
            {
                return "?";
            }

            var age = DateTime.Today.Year - birthDay.Value.Year;
            if (DateTime.Today < birthDay.Value.AddYears(age))
            {
                // 今年の誕生日がまだ来ていない。
                age -= 1;
            }
            return age.ToString();
        }

        public static 年齢Converter Instance
        {
            get { return _instance = _instance ?? new 年齢Converter(); }
        }
        private static 年齢Converter _instance;
    }

    public class 主傷病Converter : TypeSafeConverter<string, string>
    {
        public override string Convert(string value, object parameter)
        {
            if (value == "01")
            {
                return "○";
            }
            else if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            else
            {
                return "？";
            }
        }

        public static 主傷病Converter Instance
        {
            get { return _instance = _instance ?? new 主傷病Converter(); }
        }
        private static 主傷病Converter _instance;
    }

    public class DictConverter : TypeSafeConverter<string, int>
    {
        /// <summary></summary>
        public Dictionary<int, string> Dict { get; set; }

        /// <summary></summary>
        public bool ReturnsKeyWhenNotFount { get; set; }

        public override string Convert(int key, object parameter)
        {
            if (this.Dict != null)
            {
                if (this.Dict.ContainsKey(key))
                {
                    return this.Dict[key];
                }
            }

            if (ReturnsKeyWhenNotFount)
            {
                return key.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static DictConverter 診療行為Instance(int 診療年月)
        {
            var masterVersion = EnumUtil.CalcMasterVersion(診療年月);

            if (_診療行為Dict.ContainsKey(masterVersion) == false)
            {
                _診療行為Dict.Add(masterVersion, new DictConverter());
            }

            return _診療行為Dict[masterVersion];
        }
        private static Dictionary<MasterVersion, DictConverter> _診療行為Dict = new Dictionary<MasterVersion, DictConverter>();

        public static DictConverter 診療行為単位Instance
        {
            get { return _診療行為単位Instance = _診療行為単位Instance ?? new DictConverter(); }
        }
        private static DictConverter _診療行為単位Instance;

        public static DictConverter 医薬品Instance(int 診療年月)
        {
            var masterVersion = EnumUtil.CalcMasterVersion(診療年月);

            if (_医薬品Dict.ContainsKey(masterVersion) == false)
            {
                _医薬品Dict.Add(masterVersion, new DictConverter());
            }

            return _医薬品Dict[masterVersion];
        }
        private static Dictionary<MasterVersion, DictConverter> _医薬品Dict = new Dictionary<MasterVersion, DictConverter>();

        public static DictConverter 医薬品単位Instance
        {
            get { return _医薬品単位Instance = _医薬品単位Instance ?? new DictConverter() { ReturnsKeyWhenNotFount = false, }; }
        }
        private static DictConverter _医薬品単位Instance;

        public static DictConverter 特定器材Instance(int 診療年月)
        {
            var masterVersion = EnumUtil.CalcMasterVersion(診療年月);

            if (_特定器材Dict.ContainsKey(masterVersion) == false)
            {
                _特定器材Dict.Add(masterVersion, new DictConverter());
            }

            return _特定器材Dict[masterVersion];
        }
        private static Dictionary<MasterVersion, DictConverter> _特定器材Dict = new Dictionary<MasterVersion, DictConverter>();

        public static DictConverter 特定器材単位Instance
        {
            get { return _特定器材単位Instance = _特定器材単位Instance ?? new DictConverter(); }
        }
        private static DictConverter _特定器材単位Instance;
    }

    public class DictConverter2 : TypeSafeConverter<string, int?>
    {
        /// <summary></summary>
        private DictConverter _converter = new DictConverter();

        /// <summary></summary>
        public Dictionary<int, string> Dict
        {
            get { return this._converter.Dict; }
            set { this._converter.Dict = value; }
        }

        public override string Convert(int? value, object parameter)
        {
            if (value.HasValue)
            {
                return _converter.Convert(value.Value, parameter);
            }
            else
            {
                return null;
            }
        }
    }

    public class 内容Converter : TypeSafeMultiConverter<string, int, string, object>
    {
        public override string Convert(int 診療年月, string レコード識別情報, object 内容, object parameter)
        {
            if (レコード識別情報 == レコード識別情報定数.診療行為)
            {
                return DictConverter.診療行為Instance(診療年月).Convert((int)内容, parameter);
            }
            else if (レコード識別情報 == レコード識別情報定数.医薬品)
            {
                return DictConverter.医薬品Instance(診療年月).Convert((int)内容, parameter);
            }
            else if (レコード識別情報 == レコード識別情報定数.特定器材)
            {
                return DictConverter.特定器材Instance(診療年月).Convert((int)内容, parameter);
            }
            else if (レコード識別情報 == レコード識別情報定数.コメント)
            {
                var tuple = (Tuple<int, string>)内容;
                return コメントConverter.Instance.Convert(tuple.Item1, tuple.Item2, parameter);
            }
            else
            {
                return string.Empty;
            }
        }

        public static 内容Converter Instance
        {
            get { return _instance = _instance ?? new 内容Converter(); }
        }
        private static 内容Converter _instance;
    }

    public class 数量Converter : TypeSafeMultiConverter<string, string, float?, object>
    {
        public override string Convert(string レコード識別情報, float? 数量, object 内容, object parameter)
        {
            if (数量 == null)
            {
                return string.Empty;
            }

            if (レコード識別情報 == レコード識別情報定数.診療行為)
            {
                var id = (int)内容;
                return 数量.ToString() + DictConverter.診療行為単位Instance.Convert(id, parameter);
            }
            else if (レコード識別情報 == レコード識別情報定数.医薬品)
            {
                var id = (int)内容;
                return 数量.ToString() + DictConverter.医薬品単位Instance.Convert(id, parameter);
            }
            else if (レコード識別情報 == レコード識別情報定数.特定器材)
            {
                var id = (int)内容;
                return 数量.ToString() + DictConverter.特定器材単位Instance.Convert(id, parameter);
            }
            else
            {
                return string.Empty;
            }
        }

        public static 数量Converter Instance
        {
            get { return _instance = _instance ?? new 数量Converter(); }
        }
        private static 数量Converter _instance;
    }

    public class EnumNullableIntStringConverter : DictConverter2
    {
        public EnumNullableIntStringConverter(Type enumType)
        {
            this.Dict = new Dictionary<int, string>();
            foreach (var e in Enum.GetValues(enumType))
            {
                this.Dict.Add((int)e, e.ToString());
            }
        }

        public static EnumNullableIntStringConverter 診療識別Instance
        {
            get { return _診療識別Instance = _診療識別Instance ?? new EnumNullableIntStringConverter(typeof(診療識別)); }
        }
        private static EnumNullableIntStringConverter _診療識別Instance;
    }

    public class 傷病名Converter : TypeSafeMultiConverter<string, int, string>
    {
        /// <summary></summary>
        private DictConverter _dictConverter = new DictConverter();

        /// <summary></summary>
        public Dictionary<int, string> 傷病名Dict
        {
            get { return _dictConverter.Dict; }
            set { _dictConverter.Dict = value; }
        }

        /// <summary></summary>
        public Dictionary<int, string> 修飾語Dict { get; set; }

        public override string Convert(int value1, string value2, object parameter)
        {
            if (value1 == Define.未コード化傷病コード)
            {
                return "（未コード化傷病名）";
            }

            var tmp = _dictConverter.Convert(value1, null);

            if (this.修飾語Dict != null && !string.IsNullOrEmpty(value2) && value2.Length % 4 == 0)
            {
                for (int i = 0; i < 20;/*20?*/ i++)
                {
                    if (((i + 1) * 4) <= value2.Length)
                    {
                        int id;
                        if (int.TryParse(value2.Substring((i * 4), 4), out id))
                        {
                            if (this.修飾語Dict.ContainsKey(id))
                            {
                                if (id < 8000)  // 1～7999まで接頭語
                                {
                                    tmp = this.修飾語Dict[id] + tmp;
                                }
                                else if (id < 9000)  // 8000～8999まで接尾語
                                {
                                    tmp = tmp + this.修飾語Dict[id];
                                }
                                else
                                {
                                    // 歯科部位コード
                                }
                            }
                        }
                    }
                }
            }
            return tmp;
        }

        public static 傷病名Converter Instance
        {
            get { return _instance = _instance ?? new 傷病名Converter(); }
        }
        private static 傷病名Converter _instance;
    }

    public class コメントConverter : TypeSafeMultiConverter<string, int, string>
    {
        /// <summary></summary>
        public Dictionary<int, コメントマスター> コメントDict { get; set; }

        private コメントマスター Find(int コメントコード)
        {
            if (this.コメントDict != null)
            {
                if (this.コメントDict.ContainsKey(コメントコード))
                {
                    return this.コメントDict[コメントコード];
                }
            }
            return コメントマスター.Empty;
        }

        public override string Convert(int コメントコード, string 文字データ, object parameter)
        {
            var x = this.Find(コメントコード);

            // TODO: コメントパターンについては10/20/30/40/90以外の資料が見当たらない。
            // 他のコメントパターンはとりあえず↓のorcaの資料を参考にして作っている。
            // https://ftp.orca.med.or.jp/pub/data/receipt/outline/update/improvement/pdf/2020comment-2020-06-30.pdf

            // 自由入力の文字データを定型コメント文と区別するために表示を変える。
            Func<string, string> 文字データの表示 = s => string.Format("※{0}", s);

            if (x.パターン == 10)
            {
                // 10: 症状の説明等、任意の文字列情報を記録する。
                return 文字データの表示(文字データ);
            }
            else if (x.パターン == 20)
            {
                // 20: 定型のコメント文を設定する。
                return x.漢字名称;
            }
            else if (x.パターン == 30)
            {
                // 30: 定型のコメント文に、一部の文字列情報を記録する。
                // 
                return x.漢字名称 + 文字データの表示(文字データ);
            }
            else if (x.パターン == 31)
            {
                // 31: 定型のコメント文に、診療行為を記載する。
                var 診療行為名 = string.Empty;
                if (int.TryParse(StringUtil.ZenToHan(文字データ), out int 診療行為コード))
                {
                    // TODO: マスターバージョン指定が必要。面倒なので最新版。
                    診療行為名 = DictConverter.診療行為Instance(99999).Convert(診療行為コード, null);
                }

                if (string.IsNullOrEmpty(診療行為名))
                {
                    return x.漢字名称 + "；" + 文字データの表示(文字データ);  // 妥協して元の文字列を返す。
                }
                else
                {
                    return x.漢字名称 + "；" + 診療行為名;
                }
            }
            else if (x.パターン == 40)
            {
                // 40: 定型のコメント文に、一部の数字情報を記録する。
                var result = x.漢字名称;
                var tmp文字データ = 文字データ;

                foreach (var カラム位置桁数 in x.カラム位置桁数)
                {
                    var rep = tmp文字データ.Substring(0, カラム位置桁数.Item2);

                    var idxカラム位置 = カラム位置桁数.Item1 - 1;
                    result = result.Substring(0, idxカラム位置) + rep + result.Substring(idxカラム位置 + カラム位置桁数.Item2);

                    // 置き換えが済んだ文字データは削除しておく。
                    tmp文字データ = tmp文字データ.Substring(カラム位置桁数.Item2);
                }

                return result;
            }
            else if (x.パターン == 42)
            {
                // 42: 定型のコメント文に、実施回数や検査値など数値を記載する。
                // 30との違いは、30は定型文に「；」が既についている模様。
                return x.漢字名称 + "；" + 文字データの表示(文字データ);
            }
            else if (x.パターン == 50)
            {
                // 50: コメント内容に年月日を記載する。
                var split = StringUtil.ZenToHan(文字データ).Split(new char[] { ' ', '　', }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 1)
                {
                    if (int.TryParse(split[0], out int ymd))
                    {
                        var showDate = DateUtil.ReceiptDateToShowDate(ymd);
                        if (!string.IsNullOrEmpty(showDate))
                        {
                            return x.漢字名称 + "；" + showDate;
                        }
                    }
                }
                else if (split.Length == 3)
                {
                    if (int.TryParse(split[0], out int y)
                        && int.TryParse(split[1], out int m)
                        && int.TryParse(split[2], out int d))
                    {
                        var ymd = (y * 10000) + (m * 100) + d;
                        var showDate = DateUtil.ReceiptDateToShowDate(ymd);
                        if (!string.IsNullOrEmpty(showDate))
                        {
                            return x.漢字名称 + "；" + showDate;
                        }
                    }
                }
                return x.漢字名称 + "；" + 文字データの表示(文字データ);
            }
            else if (x.パターン == 51)
            {
                // 51: コメント内容に時刻を記載する。
                var split = StringUtil.ZenToHan(文字データ).Split(new char[] { ' ', '　', }, StringSplitOptions.RemoveEmptyEntries);
                if (split.Length == 2)
                {
                    if (int.TryParse(split[0], out int h)
                        && int.TryParse(split[1], out int m))
                    {
                        var time = string.Format("{0}時{1}分", h, m);
                        return x.漢字名称 + "；" + time;
                    }
                }
                return x.漢字名称 + "；" + 文字データの表示(文字データ);
            }
            else if (x.パターン == 52)
            {
                // 52: コメント内容に時間（分単位）を記載する。
                if (int.TryParse(StringUtil.ZenToHan(文字データ), out int m))
                {
                    return x.漢字名称 + "；" + m + "分";
                }
                else
                {
                    return x.漢字名称 + "；" + 文字データの表示(文字データ);
                }
            }
            else if (x.パターン == 90)
            {
                // 90: 処置、手術及び画像診断等を行った部位を、修飾語（部位）コードを使用して記録する。
                return "（未実装）" + 文字データの表示(文字データ);
            }
            else
            {
                return コメントコード.ToString() + "；" + 文字データの表示(文字データ);
            }
        }

        public static コメントConverter Instance
        {
            get { return _instance = _instance ?? new コメントConverter(); }
        }
        private static コメントConverter _instance;
    }

    public class 算定日Converter : TypeSafeConverter<string, Dictionary<int, int>>
    {
        public override string Convert(Dictionary<int, int> value, object parameter)
        {
            if (value != null)
            {
                var list = new List<string>();
                foreach (var kv in value.OrderBy(x => x.Key))
                {
                    var dateIdx = kv.Key;
                    list.Add((dateIdx + 1).ToString());
                }
                return string.Join(", ", list);
            }

            return null;
        }

        public static 算定日Converter Instance
        {
            get { return _instance = _instance ?? new 算定日Converter(); }
        }
        private static 算定日Converter _instance;
    }

    //public class TitleConverter : TypeSafeMultiConverter<string, 審査支払機関, int>
    //{
    //    public override string Convert(審査支払機関 審査支払機関, int 請求年月, object parameter)
    //    {
    //        if (0 < 請求年月)
    //        {
    //            return 審査支払機関.ToString() + "  請求年月" + DateUtil.ReceiptDateToShowDate(請求年月, true);
    //        }

    //        return "OpenReceiptViewer";
    //    }

    //    public static TitleConverter Instance
    //    {
    //        get { return _instance = _instance ?? new TitleConverter(); }
    //    }
    //    private static TitleConverter _instance;
    //}

    public class レセプト種別Converter : TypeSafeConverter<string, int>
    {
        public override string Convert(int value, object parameter)
        {
            var tmp = "";

            var keta123 = (int)(value / 10);
            var keta12 = (int)(value / 100);
            var keta4 = value - (keta123 * 10);
            switch (keta123)
            {
                case 111:
                    tmp += "医科・医保単独";
                    break;
                case 112:
                    //tmp += "医科・医保と１種の公費併用";
                    tmp += "医科・医保と公費１";
                    break;
                case 113:
                    tmp += "医科・医保と公費２";
                    break;
                case 114:
                    tmp += "医科・医保と公費３";
                    break;
                case 115:
                    tmp += "医科・医保と公費４";
                    break;
                case 121:
                    tmp += "医科・公費単独";
                    break;
                case 122:
                    //tmp += "医科・２種の公費併用";
                    tmp += "医科・公費２";
                    break;
                case 123:
                    tmp += "医科・公費３";
                    break;
                case 124:
                    tmp += "医科・公費４";
                    break;
                case 131:
                    //tmp += "医科・後期高齢者単独";
                    tmp += "医科・後期高齢";
                    break;
                case 132:
                    //tmp += "医科・後期高齢者単独と１種の公費併用";
                    tmp += "医科・後期高齢と公費１";
                    break;
                case 133:
                    tmp += "医科・後期高齢と公費２";
                    break;
                case 134:
                    tmp += "医科・後期高齢と公費３";
                    break;
                case 135:
                    tmp += "医科・後期高齢と公費４";
                    break;
                case 141:
                    tmp += "医科・退職者単独";
                    break;
                case 142:
                    tmp += "医科・退職者と公費１";
                    break;
                case 143:
                    tmp += "医科・退職者と公費２";
                    break;
                case 144:
                    tmp += "医科・退職者と公費３";
                    break;
                case 145:
                    tmp += "医科・退職者と公費４";
                    break;
                default:
                    break;
            }
            tmp += " ";
            if (keta12 != 12)
            {
                switch (keta4)
                {
                    case 1:
                    case 2:
                        tmp += "本人";
                        break;
                    case 3:
                    case 4:
                        tmp += "未就学者";
                        break;
                    case 5:
                    case 6:
                        tmp += "家族";
                        break;
                    case 7:
                    case 8:
                        if (keta12 != 13)
                        {
                            //tmp += "高齢受給者一般・低所得者";
                            tmp += "高齢一般・低所得者";
                        }
                        else
                        {
                            tmp += "一般・低所得者";
                        }
                        break;
                    case 9:
                    case 0:
                        if (keta12 != 13)
                        {
                            //tmp += "高齢受給者７割";
                            tmp += "高齢７割";
                        }
                        else
                        {
                            tmp += "７割";
                        }
                        break;
                    default:
                        break;
                }
                tmp += " ";
            }

            if (keta4 % 2 == 0)
            {
                tmp += "入院外";
            }
            else
            {
                tmp += "入院";
            }

            return tmp;
        }

        public static レセプト種別Converter Instance
        {
            get { return _instance = _instance ?? new レセプト種別Converter(); }
        }
        private static レセプト種別Converter _instance;
    }

    public class ZeroHideConverter : TypeSafeConverter<string, int>
    {
        public override string Convert(int value, object parameter)
        {
            if (value == 0)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }

        public static ZeroHideConverter Instance
        {
            get { return _instance = _instance ?? new ZeroHideConverter(); }
        }
        private static ZeroHideConverter _instance;
    }

    public class 被保険者Converter : TypeSafeMultiConverter<object, string, string>
    {
        public override object Convert(string 被保険者証記号, string 被保険者証番号, object parameter)
        {
            return 被保険者証記号 + " - " + 被保険者証番号;
        }

        public static 被保険者Converter Instance
        {
            get { return _instance = _instance ?? new 被保険者Converter(); }
        }
        private static 被保険者Converter _instance;
    }

    public class 氏名カタカナConverter : TypeSafeMultiConverter<object, string, string>
    {
        public override object Convert(string 氏名, string カタカナ, object parameter)
        {
            if (string.IsNullOrEmpty(カタカナ))
            {
                return 氏名;
            }
            else
            {
                return string.Format("{0} （ {1} ）", 氏名, カタカナ);
            }
        }

        public static 氏名カタカナConverter Instance
        {
            get { return _instance = _instance ?? new 氏名カタカナConverter(); }
        }
        private static 氏名カタカナConverter _instance;
    }
}
