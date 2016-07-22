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
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
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
            var receiptDateStr = receiptDate.ToString();
            if ((!withoutDay && receiptDateStr.Length == 7)
                || (withoutDay && receiptDateStr.Length == 5))
            {
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

            return "?";
        }

        /// <summary></summary>
        /// <param name="receiptDate"></param>
        /// <returns></returns>
        public static DateTime? ReceiptDateToDateTime(int receiptDate)
        {
            var showDate = ReceiptDateToShowDate(receiptDate);

            if (showDate.StartsWith("?"))
            {
                return null;
            }

            try
            {
                return DateTime.ParseExact(showDate, "ggyy.MM.dd", Culture);
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
}
