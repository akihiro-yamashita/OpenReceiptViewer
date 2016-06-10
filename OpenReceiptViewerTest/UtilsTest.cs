using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReceiptViewer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenReceiptViewerTest
{
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
}
