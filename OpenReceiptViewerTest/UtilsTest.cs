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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenReceiptViewer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenReceiptViewerTest
{
    [TestClass]
    public class DateUtilTest
    {
        [TestMethod]
        public void ReceiptDateToShowDateTest()
        {
            string showDate;

            showDate = DateUtil.ReceiptDateToShowDate(0);
            Assert.AreEqual("", showDate);

            // 和暦
            showDate = DateUtil.ReceiptDateToShowDate(4301231);
            Assert.AreEqual("平成30.12.31", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(3010203);
            Assert.AreEqual("昭和01.02.03", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(43012, true);
            Assert.AreEqual("平成30.12", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(50105, true);
            Assert.AreEqual("令和01.05", showDate);

            // 西暦
            showDate = DateUtil.ReceiptDateToShowDate(20191231, false);
            Assert.AreEqual("2019.12.31", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(201901, true);
            Assert.AreEqual("2019.01", showDate);

            // 変な値
            showDate = DateUtil.ReceiptDateToShowDate(201912312, false);
            Assert.AreEqual("", showDate);
        }

        [TestMethod]
        public void ReceiptDateToDateTimeTest()
        {
            DateTime? dateTime;

            // 和暦
            dateTime = DateUtil.ReceiptDateToDateTime(4301231);
            Assert.AreEqual(new DateTime(2018, 12, 31), dateTime.Value);

            dateTime = DateUtil.ReceiptDateToDateTime(3010203);
            Assert.AreEqual(new DateTime(1926, 2, 3), dateTime.Value);

            // 西暦
            dateTime = DateUtil.ReceiptDateToDateTime(20191231);
            Assert.AreEqual(new DateTime(2019, 12, 31), dateTime.Value);

            dateTime = DateUtil.ReceiptDateToDateTime(20190102);
            Assert.AreEqual(new DateTime(2019, 1, 2), dateTime.Value);

            // 変な値
            dateTime = DateUtil.ReceiptDateToDateTime(430123);
            Assert.IsNull(dateTime);
        }
    }

    [TestClass]
    public class CollectionUtilTest
    {
        public class Person
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void FirstOrDefaultTest()
        {
            var list = new List<Person>();
            list.Add(new Person() { Name = "suzuki", });
            list.Add(new Person() { Name = "tanaka1", });
            list.Add(new Person() { Name = "ito", });
            list.Add(new Person() { Name = "tanaka2", });
            var collection = new Collection<Person>(list);

            var suzuki = list[0];
            var tanaka1 = list[1];
            var tanaka2 = list[3];
            var who = new Person() { Name = "who?", };

            Person result;

            result = collection.FirstOrDefault(x => x.Name.Contains("tanaka"), suzuki);
            Assert.AreEqual("tanaka1", result.Name);

            result = collection.FirstOrDefault(x => x.Name.Contains("tanaka"), tanaka1);
            Assert.AreEqual("tanaka2", result.Name);

            result = collection.FirstOrDefault(x => x.Name.Contains("tanaka"), who);
            Assert.AreEqual("tanaka1", result.Name);

            result = collection.FirstOrDefault(x => x.Name.Contains("tanaka"), tanaka2);
            Assert.AreEqual("tanaka1", result.Name);
        }
    }

    [TestClass]
    public class StringUtilTest
    {
        [TestMethod]
        public void HanToZenTest()
        {
            Assert.AreEqual("０１２３４５６７８９", StringUtil.HanToZen("0123456789"));
            Assert.AreEqual("ＥＦ－喉頭", StringUtil.HanToZen("EF-喉頭"));
            Assert.AreEqual("休日加算（初診）", StringUtil.HanToZen("休日加算(初診)"));
        }

        [TestMethod]
        public void ZenToHanTest()
        {
            Assert.AreEqual("0123456789", StringUtil.ZenToHan("０１２３４５６７８９"));
            Assert.AreEqual("EF-喉頭", StringUtil.ZenToHan("ＥＦ－喉頭"));
            Assert.AreEqual("休日加算(初診)", StringUtil.ZenToHan("休日加算（初診）"));
        }
    }

    [TestClass]
    public class EnumUtilTest
    {
        [TestMethod]
        public void MasterVersionOldToMasterVersionTest()
        {
            Assert.AreEqual(MasterVersion.Ver201910, EnumUtil.MasterVersionOldToMasterVersion(MasterVersionOld.Ver201910));
            Assert.AreEqual(MasterVersion.Ver202104, EnumUtil.MasterVersionOldToMasterVersion(MasterVersionOld.Ver202104));
        }

        [TestMethod]
        public void CalcMasterVersionTest()
        {
            Assert.AreEqual(MasterVersion.Ver201604, EnumUtil.CalcMasterVersion(201603));
            Assert.AreEqual(MasterVersion.Ver201604, EnumUtil.CalcMasterVersion(201604));
            Assert.AreEqual(MasterVersion.Ver201910, EnumUtil.CalcMasterVersion(202003));
            Assert.AreEqual(MasterVersion.Ver202004, EnumUtil.CalcMasterVersion(202004));
            Assert.AreEqual(MasterVersion.Ver202004, EnumUtil.CalcMasterVersion(202005));
            Assert.AreEqual(MasterVersion.Ver202004, EnumUtil.CalcMasterVersion(202103));
            Assert.AreEqual(MasterVersion.Ver202104, EnumUtil.CalcMasterVersion(202104));
            Assert.AreEqual(MasterVersion.Ver202104, EnumUtil.CalcMasterVersion(202105));
            Assert.AreEqual(MasterVersion.Ver202004, EnumUtil.CalcMasterVersion(50303));
            Assert.AreEqual(MasterVersion.Ver202104, EnumUtil.CalcMasterVersion(50304));
            Assert.AreEqual(MasterVersion.Ver202104, EnumUtil.CalcMasterVersion(50305));
            Assert.AreEqual(MasterVersion.Ver202201, EnumUtil.CalcMasterVersion(50401));
            Assert.AreEqual(MasterVersion.Ver202204, EnumUtil.CalcMasterVersion(50404));
            Assert.AreEqual(MasterVersion.Ver202207, EnumUtil.CalcMasterVersion(50407));
        }
    }
}
