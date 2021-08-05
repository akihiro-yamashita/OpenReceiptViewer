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

using CsvHelper;
using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace OpenReceiptViewer
{
    /// <summary></summary>
    public static class DateUtil
    {
        /// <summary></summary>
        /// <param name="receiptDate"></param>
        /// <param name="withoutDay"></param>
        /// <returns></returns>
        public static string ReceiptDateToShowDate(int receiptDate, bool withoutDay = false)
        {
            if (receiptDate == 0)
            {
                return string.Empty;  // 外来レセプトの入院年月日等
            }

            var receiptDateStr = receiptDate.ToString();

            if ((!withoutDay && receiptDateStr.Length == 7)
                || (withoutDay && receiptDateStr.Length == 5))
            {
                // 和暦

                var showDate = "??";
                var nengo = Int32.Parse(receiptDateStr.Substring(0, 1));
                foreach (var x in Enum.GetValues(typeof(年号区分)))
                {
                    if (nengo == (int)x)
                    {
                        showDate = x.ToString();
                        break;
                    }
                }

                showDate += receiptDateStr.Substring(1, 2);
                showDate += ".";
                showDate += receiptDateStr.Substring(3, 2);
                if (!withoutDay)
                {
                    showDate += ".";
                    showDate += receiptDateStr.Substring(5, 2);
                }

                return showDate;
            }
            else if ((!withoutDay && receiptDateStr.Length == 8)
                || (withoutDay && receiptDateStr.Length == 6))
            {
                // 西暦

                var showDate = string.Empty;
                showDate += receiptDateStr.Substring(0, 4);
                showDate += ".";
                showDate += receiptDateStr.Substring(4, 2);
                if (!withoutDay)
                {
                    showDate += ".";
                    showDate += receiptDateStr.Substring(6, 2);
                }

                return showDate;
            }

            return string.Empty;
        }

        /// <summary></summary>
        /// <param name="receiptDate"></param>
        /// <returns></returns>
        public static DateTime? ReceiptDateToDateTime(int receiptDate)
        {
            var showDate = ReceiptDateToShowDate(receiptDate);

            if (string.IsNullOrEmpty(showDate))
            {
                return null;
            }
            if (showDate.StartsWith("?"))
            {
                return null;
            }

            var is西暦 = char.IsDigit(showDate[0]);

            try
            {
                if (is西暦)
                {
                    return DateTime.ParseExact(showDate, "yyyy.MM.dd", null);
                }
                else
                {
                    return DateTime.ParseExact(showDate, "ggyy.MM.dd", Culture);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary></summary>
        public static CultureInfo Culture
        {
            get
            {
                if (_culture == null)
                {
                    _culture = new CultureInfo("ja-JP", true);
                    _culture.DateTimeFormat.Calendar = new JapaneseCalendar();
                }
                return _culture;
            }
        }
        private static CultureInfo _culture;
    }

    /// <summary></summary>
    public static class CollectionUtil
    {
        /// <summary></summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="searchAfter"></param>
        /// <returns></returns>
        public static TSource FirstOrDefault<TSource>(this Collection<TSource> source, Func<TSource, bool> predicate, TSource searchAfter)
        {
            var startFromIdx = source.Count;
            for (int i = 0; i < source.Count; i++)
            {
                var x = source[i];
                if (x.Equals(searchAfter))
                {
                    // 見つけた要素searchAfterの次からpredicateに合うものを探す。
                    startFromIdx = i + 1;
                    break;
                }
            }

            for (int i = startFromIdx; i < source.Count; i++)
            {
                var x = source[i];
                if (predicate(x))
                {
                    return x;
                }
            }

            // 見つからないので普通に探す。
            for (int i = 0; i < startFromIdx; i++)
            {
                var x = source[i];
                if (predicate(x))
                {
                    return x;
                }
            }

            return default(TSource);
        }
    }

    /// <summary></summary>
    public static class CSVUtil
    {
        public static void Read(string filePath, Action<CsvReader> readAction, bool hasHeaderRecord = false)
        {
            using (var stream = new System.IO.StreamReader(filePath, Encoding.GetEncoding("Shift_JIS")))
            {
                var config = new CsvHelper.Configuration.CsvConfiguration()
                {
                    HasHeaderRecord = hasHeaderRecord,
                };

                using (var csv = new CsvReader(stream, config))
                {
                    readAction(csv);
                }
            }
        }
    }

    public static class StringUtil
    {
        public static string HanToZen(string s)
        {
            if (s == null) { return null; }
            return string.Join(string.Empty, s.Select(c => HanToZen(c)));
        }

        public static char HanToZen(char c)
        {
            if ('0' <= c && c <= '9')
            {
                return (char)('０' + (c - '0'));
            }
            else if ('a' <= c && c <= 'z')
            {
                return (char)('ａ' + (c - 'a'));
            }
            else if ('A' <= c && c <= 'Z')
            {
                return (char)('Ａ' + (c - 'A'));
            }
            else if (c == '/')
            {
                return '／';
            }
            else if (c == '-')
            {
                return '－';
            }
            else if (c == '(')
            {
                return '（';
            }
            else if (c == ')')
            {
                return '）';
            }
            else
            {
                return c;
            }
        }

        public static string ZenToHan(string s)
        {
            if (s == null) { return null; }
            return string.Join(string.Empty, s.Select(c => ZenToHan(c)));
        }

        public static char ZenToHan(char c)
        {
            if ('０' <= c && c <= '９')
            {
                return (char)('0' + (c - '０'));
            }
            else if ('ａ' <= c && c <= 'ｚ')
            {
                return (char)('a' + (c - 'ａ'));
            }
            else if ('Ａ' <= c && c <= 'Ｚ')
            {
                return (char)('A' + (c - 'Ａ'));
            }
            else if (c == '／')
            {
                return '/';
            }
            else if (c == '－')
            {
                return '-';
            }
            else if (c == '（')
            {
                return '(';
            }
            else if (c == '）')
            {
                return ')';
            }
            else
            {
                return c;
            }
        }
    }

    /// <summary></summary>
    public static class EnumUtil
    {
        public static MasterVersion MasterVersionOldToMasterVersion(this MasterVersionOld old)
        {
            foreach (MasterVersion e in Enum.GetValues(typeof(MasterVersion)))
            {
                if (old.ToString() == e.ToString())
                {
                    return e;
                }
            }

            // とりあえず一番古いマスター
            return MasterVersion.Ver201604;
        }

        /// <summary>診療年月からMasterVersionを計算</summary>
        /// <param name="診療年月"></param>
        /// <returns></returns>
        public static MasterVersion CalcMasterVersion(int 診療年月)
        {
            var 診療年月Str = 診療年月.ToString();
            if (診療年月Str.Length == 5)
            {
                var masterVersion = (MasterVersionOld?)null;
                foreach (MasterVersionOld e in Enum.GetValues(typeof(MasterVersionOld)))
                {
                    if ((int)e <= 診療年月)
                    {
                        masterVersion = e;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (masterVersion.HasValue)
                {
                    return masterVersion.Value.MasterVersionOldToMasterVersion();
                }
            }
            else if (診療年月Str.Length == 6)
            {
                var masterVersion = (MasterVersion?)null;
                foreach (MasterVersion e in Enum.GetValues(typeof(MasterVersion)))
                {
                    if ((int)e <= 診療年月)
                    {
                        masterVersion = e;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (masterVersion.HasValue)
                {
                    return masterVersion.Value;
                }
            }

            return MasterVersion.Ver201604;
        }

        /// <summary>TODO: 逆にフォルダ名にVerつけられないか</summary>
        /// <param name="masterVersion"></param>
        /// <returns></returns>
        public static string GetMasterSubDiretoryName(MasterVersion masterVersion)
        {
            return masterVersion.ToString().Replace("Ver", "");
        }
    }
}
