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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenReceiptViewer;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace OpenReceiptViewerTest
{
    [TestClass]
    public class ConvertersTest
    {
        [TestMethod]
        public void コメントConverterTest()
        {
            コメントConverter.Instance.コメントDict = new Dictionary<int, コメントマスター>();
            コメントConverter.Instance.修飾語Dict = new Dictionary<int, string>();
            コメントConverter.Instance.修飾語Dict.Add(2056, "右");
            コメントConverter.Instance.修飾語Dict.Add(1066, "足");
            var text = string.Empty;
            var id = 0;

            // 10: 症状の説明等、任意の文字列情報を記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 10, 漢字名称 = "定型コメント10", });
            text = コメントConverter.Instance.Convert(id, "あああ", null);
            Assert.AreEqual("※あああ", text);

            // 20: 定型のコメント文を設定する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 20, 漢字名称 = "定型コメント20", });
            text = コメントConverter.Instance.Convert(id, "あああ", null);
            Assert.AreEqual("定型コメント20", text);

            // 30: 定型のコメント文に、一部の文字列情報を記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 30, 漢字名称 = "定型コメント30；", });
            text = コメントConverter.Instance.Convert(id, "あああ", null);
            Assert.AreEqual("定型コメント30；※あああ", text);

            DictConverter.診療行為Instance(99999).Dict = new Dictionary<int, string>();
            DictConverter.診療行為Instance(99999).Dict.Add(12345, "しんりょうこうい");

            // 31: 定型のコメント文に、診療行為を記載する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 31, 漢字名称 = "定型コメント31", });
            text = コメントConverter.Instance.Convert(id, "12345", null);
            Assert.AreEqual("定型コメント31；しんりょうこうい", text);
            text = コメントConverter.Instance.Convert(id, "１２３４５", null);
            Assert.AreEqual("定型コメント31；しんりょうこうい", text);
            text = コメントConverter.Instance.Convert(id, "123456", null);
            Assert.AreEqual("定型コメント31；※123456", text);

            // 40: 定型のコメント文に、一部の数字情報を記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター
                {
                    パターン = 40,
                    漢字名称 = "傷病手当金意見書交付　平成　　年　　月　　日",  // 実際にあるやつ
                    カラム位置桁数 = new List<Tuple<int, int>>
                    {
                        new Tuple<int, int>(14, 2),
                        new Tuple<int, int>(17, 2),
                        new Tuple<int, int>(20, 2),
                    },
                });
            text = コメントConverter.Instance.Convert(id, "301231", null);
            Assert.AreEqual("傷病手当金意見書交付　平成30年12月31日", text);
            text = コメントConverter.Instance.Convert(id, "３０１２３１", null);
            Assert.AreEqual("傷病手当金意見書交付　平成３０年１２月３１日", text);
            text = コメントConverter.Instance.Convert(id, "０１０２０３", null);
            Assert.AreEqual("傷病手当金意見書交付　平成０１年０２月０３日", text);
            text = コメントConverter.Instance.Convert(id, "　１　２　３", null);
            Assert.AreEqual("傷病手当金意見書交付　平成　１年　２月　３日", text);
            text = コメントConverter.Instance.Convert(id, "０１０２３", null);
            Assert.AreEqual("傷病手当金意見書交付　平成０１年０２月　３日", text);

            // 42: 定型のコメント文に、実施回数や検査値など数値を記載する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 42, 漢字名称 = "定型コメント42", });
            text = コメントConverter.Instance.Convert(id, "あああ", null);
            Assert.AreEqual("定型コメント42；※あああ", text);

            // 50: コメント内容に年月日を記載する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 50, 漢字名称 = "定型コメント50", });
            text = コメントConverter.Instance.Convert(id, "20191231", null);
            Assert.AreEqual("定型コメント50；2019.12.31", text);
            text = コメントConverter.Instance.Convert(id, "２０１９１２３１", null);
            Assert.AreEqual("定型コメント50；2019.12.31", text);
            text = コメントConverter.Instance.Convert(id, "2019 12 31", null);
            Assert.AreEqual("定型コメント50；2019.12.31", text);
            text = コメントConverter.Instance.Convert(id, "5011231", null);
            Assert.AreEqual("定型コメント50；令和01.12.31", text);
            text = コメントConverter.Instance.Convert(id, "501 12 31", null);
            Assert.AreEqual("定型コメント50；令和01.12.31", text);
            text = コメントConverter.Instance.Convert(id, "501831", null);
            Assert.AreEqual("定型コメント50；※501831", text);

            // 51: 定型のコメント文に、一部の時刻情報（時間及び分を4桁）で記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 51, 漢字名称 = "定型コメント51", });
            text = コメントConverter.Instance.Convert(id, "2359", null);
            Assert.AreEqual("定型コメント51；23時59分", text);
            text = コメントConverter.Instance.Convert(id, "２３５９", null);
            Assert.AreEqual("定型コメント51；23時59分", text);
            text = コメントConverter.Instance.Convert(id, "23 59", null);
            Assert.AreEqual("定型コメント51；23時59分", text);
            text = コメントConverter.Instance.Convert(id, "２３　５９", null);
            Assert.AreEqual("定型コメント51；23時59分", text);
            text = コメントConverter.Instance.Convert(id, "０３５９", null);
            Assert.AreEqual("定型コメント51；3時59分", text);
            text = コメントConverter.Instance.Convert(id, "３５９", null);
            Assert.AreEqual("定型コメント51；※３５９", text);

            // 52: コメント内容に時間（分単位）を記載する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 52, 漢字名称 = "定型コメント52", });
            text = コメントConverter.Instance.Convert(id, "123", null);
            Assert.AreEqual("定型コメント52；123分", text);
            text = コメントConverter.Instance.Convert(id, "１２３", null);
            Assert.AreEqual("定型コメント52；123分", text);
            text = コメントConverter.Instance.Convert(id, "００１２３", null);  // 本来全角数値5桁ゼロ埋めが正しいっぽい。
            Assert.AreEqual("定型コメント52；123分", text);
            text = コメントConverter.Instance.Convert(id, "あああ", null);
            Assert.AreEqual("定型コメント52；※あああ", text);

            // 53: 定型のコメント文に、一部の日時情報（日、時間及び分を6桁）で記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 53, 漢字名称 = "定型コメント53", });
            text = コメントConverter.Instance.Convert(id, "031259", null);
            Assert.AreEqual("定型コメント53；3日12時59分", text);
            text = コメントConverter.Instance.Convert(id, "23 12 59", null);
            Assert.AreEqual("定型コメント53；23日12時59分", text);
            text = コメントConverter.Instance.Convert(id, "31259", null);
            Assert.AreEqual("定型コメント53；※31259", text);

            // 80: 定型のコメント文に、一部の年月日情報（和暦年月日）及び一部の数字情報（数値として扱うもの（先頭を0埋めした8桁）に限る。）を記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 80, 漢字名称 = "定型コメント80", });
            text = コメントConverter.Instance.Convert(id, "501123198.75", null);
            Assert.AreEqual("定型コメント80；令和01.12.31　検査値：98.75", text);
            text = コメントConverter.Instance.Convert(id, "5011231", null);
            Assert.AreEqual("定型コメント80；令和01.12.31　検査値：", text);
            text = コメントConverter.Instance.Convert(id, "2018123198.75", null);
            // もし西暦が来た場合は2から始まるので大正で表示されるし内容もおかしい。和暦のみという仕様がおかしい。
            Assert.AreEqual("定型コメント80；大正01.81.23　検査値：198.75", text);

            // 90: 処置、手術及び画像診断等を行った部位を、修飾語（部位）コードを使用して記録する。
            id++;
            コメントConverter.Instance.コメントDict.Add(id
                , new コメントマスター { パターン = 90, 漢字名称 = "定型コメント90", });
            text = コメントConverter.Instance.Convert(id, "２０５６１０６６", null);
            Assert.AreEqual("右足", text);
            text = コメントConverter.Instance.Convert(id, "20561066", null);
            Assert.AreEqual("右足", text);
            text = コメントConverter.Instance.Convert(id, "205610661", null);
            Assert.AreEqual("※205610661", text);
            text = コメントConverter.Instance.Convert(id, "1066", null);
            Assert.AreEqual("足", text);
        }
    }
}
