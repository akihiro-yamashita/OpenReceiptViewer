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

            showDate = DateUtil.ReceiptDateToShowDate(4301231);
            Assert.AreEqual("平成30.12.31", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(3010203);
            Assert.AreEqual("昭和01.02.03", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(43012, true);
            Assert.AreEqual("平成30.12", showDate);

            showDate = DateUtil.ReceiptDateToShowDate(430123, true);
            Assert.AreEqual("?", showDate);
        }

        [TestMethod]
        public void ReceiptDateToDateTimeTest()
        {
            DateTime? dateTime;

            dateTime = DateUtil.ReceiptDateToDateTime(4301231);
            Assert.AreEqual(new DateTime(2018, 12, 31), dateTime.Value);

            dateTime = DateUtil.ReceiptDateToDateTime(3010203);
            Assert.AreEqual(new DateTime(1926, 2, 3), dateTime.Value);

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
            Assert.AreEqual("ＥＦ－喉頭", StringUtil.HanToZen("EF-喉頭"));
            Assert.AreEqual("休日加算（初診）", StringUtil.HanToZen("休日加算(初診)"));
        }
    }
}
