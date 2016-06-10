using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenReceiptViewer
{
    public static class DateUtil
    {
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

    public static class CollectionUtil
    {
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
