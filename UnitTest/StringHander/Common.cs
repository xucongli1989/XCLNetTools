using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using XCLNetTools.Entity;
using XCLNetTools.StringHander;

namespace UnitTest.StringHander
{
    [TestClass]
    public class Common
    {
        [TestMethod]
        public void IsHttp()
        {
            Assert.IsTrue(XCLNetTools.StringHander.Common.IsHttp("http://www.a.com"));
            Assert.IsTrue(XCLNetTools.StringHander.Common.IsHttp("htTp://www.a.com"));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttp(""));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttp("http:www.a.com"));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttp("https://www.a.com"));
        }

        [TestMethod]
        public void IsHttps()
        {
            Assert.IsTrue(XCLNetTools.StringHander.Common.IsHttps("https://www.a.com"));
            Assert.IsTrue(XCLNetTools.StringHander.Common.IsHttps("htTps://www.a.com"));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttps(""));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttps("https:www.a.com"));
            Assert.IsFalse(XCLNetTools.StringHander.Common.IsHttps("http://www.a.com"));
        }

        [TestMethod]
        public void GetRangeValueEntity()
        {
            var model = XCLNetTools.StringHander.Common.GetRangeValueEntity(0, 0, 0, 0);
            Assert.IsTrue(model.StartValue == 0 && model.EndValue == 0 && model.Count == 1);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(3, 1, 1, 10);
            Assert.IsTrue(null == model);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(1, 2, 3, 1);
            Assert.IsTrue(null == model);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(1, 4, 5, 10);
            Assert.IsTrue(null == model);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(11, 15, 5, 10);
            Assert.IsTrue(null == model);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(4, 6, 5, 10);
            Assert.IsTrue(model.StartValue == 5 && model.EndValue == 6 && model.Count == 2);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(4, 5, 5, 10);
            Assert.IsTrue(model.StartValue == 5 && model.EndValue == 5 && model.Count == 1);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(10, 16, 10, 15);
            Assert.IsTrue(model.StartValue == 10 && model.EndValue == 15 && model.Count == 6);

            model = XCLNetTools.StringHander.Common.GetRangeValueEntity(15, 16, 10, 15);
            Assert.IsTrue(model.StartValue == 15 && model.EndValue == 15 && model.Count == 1);
        }

        [TestMethod]
        public void GetRangeValueEntityListFromText()
        {
            var lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("", 1, 100);
            Assert.IsTrue(lst.Count == 0);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("1", 1, 100);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 1 && lst[0].EndValue == 1);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("-1", 1, 100);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 100 && lst[0].EndValue == 100);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("-2", 1, 100);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 99 && lst[0].EndValue == 99);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("100", 1, 100);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 100 && lst[0].EndValue == 100);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("10", 1, 9);
            Assert.IsTrue(lst.Count == 0);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("3,10", 1, 9);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 3 && lst[0].EndValue == 3);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("1,2", 1, 100);
            Assert.IsTrue(lst.Count == 2);
            Assert.IsTrue(lst[0].StartValue == 1 && lst[0].EndValue == 1);
            Assert.IsTrue(lst[1].StartValue == 2 && lst[1].EndValue == 2);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("3:5", 1, 100);
            Assert.IsTrue(lst.Count == 1 && lst[0].StartValue == 3 && lst[0].EndValue == 5);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("1,3:5,8:9,-10:-1", -100, 100);
            Assert.IsTrue(lst.Count == 4);
            Assert.IsTrue(lst[0].StartValue == 1 && lst[0].EndValue == 1);
            Assert.IsTrue(lst[1].StartValue == 3 && lst[1].EndValue == 5);
            Assert.IsTrue(lst[2].StartValue == 8 && lst[2].EndValue == 9);
            Assert.IsTrue(lst[3].StartValue == 91 && lst[3].EndValue == 100);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("  1  ,  3 :  5 , 8 : 9,  -10   :-1,,,,,", -100, 100);
            Assert.IsTrue(lst.Count == 4);
            Assert.IsTrue(lst[0].StartValue == 1 && lst[0].EndValue == 1);
            Assert.IsTrue(lst[1].StartValue == 3 && lst[1].EndValue == 5);
            Assert.IsTrue(lst[2].StartValue == 8 && lst[2].EndValue == 9);
            Assert.IsTrue(lst[3].StartValue == 91 && lst[3].EndValue == 100);

            lst = XCLNetTools.StringHander.Common.GetRangeValueEntityListFromText("1:4,3:5,8:9,10,11,12,13,30,31,33,-10:-1", -100, 100, true);
            Assert.IsTrue(lst.Count == 5);
            Assert.IsTrue(lst[0].StartValue == 1 && lst[0].EndValue == 5);
            Assert.IsTrue(lst[1].StartValue == 8 && lst[1].EndValue == 13);
            Assert.IsTrue(lst[2].StartValue == 30 && lst[2].EndValue == 31);
            Assert.IsTrue(lst[3].StartValue == 33 && lst[3].EndValue == 33);
            Assert.IsTrue(lst[4].StartValue == 91 && lst[4].EndValue == 100);
        }

        [TestMethod]
        public void GetPositionRangeValueEntity()
        {
            var model = XCLNetTools.StringHander.Common.GetPositionRangeValueEntity(0, 0, 0);
            Assert.IsTrue(model.StartValue == 0 && model.EndValue == 0);

            model = XCLNetTools.StringHander.Common.GetPositionRangeValueEntity(0, 1, 2);
            Assert.IsTrue(null == model);

            model = XCLNetTools.StringHander.Common.GetPositionRangeValueEntity(2, 1, 2);
            Assert.IsTrue(model.StartValue == 2);

            model = XCLNetTools.StringHander.Common.GetPositionRangeValueEntity(-2, 1, 2);
            Assert.IsTrue(model.StartValue == 1);
        }

        [TestMethod]
        public void GetStartEndNumList()
        {
            var lst = XCLNetTools.StringHander.Common.GetStartEndNumList(4, 7);
            Assert.IsTrue(lst.Count == 4);
            Assert.IsTrue(lst[0] == 4);
            Assert.IsTrue(lst[1] == 5);
            Assert.IsTrue(lst[2] == 6);
            Assert.IsTrue(lst[3] == 7);
        }

        [TestMethod]
        public void RemoveStart()
        {
            Assert.AreEqual("", XCLNetTools.StringHander.Common.RemoveStart("", ""));
            Assert.AreEqual("  ", XCLNetTools.StringHander.Common.RemoveStart("  ", ""));
            Assert.AreEqual(" ", XCLNetTools.StringHander.Common.RemoveStart("  ", " "));
            Assert.AreEqual("abCXYzMn", XCLNetTools.StringHander.Common.RemoveStart("abCXYzMn", "abc"));
            Assert.AreEqual("XYzMn", XCLNetTools.StringHander.Common.RemoveStart("abCXYzMn", "abC"));
            Assert.AreEqual("ABCXYzMn", XCLNetTools.StringHander.Common.RemoveStart("abCABCXYzMn", "abC"));
            Assert.AreEqual("abcABCXYzMn", XCLNetTools.StringHander.Common.RemoveStart("abCabcABCXYzMn", "ABC", true));
        }

        [TestMethod]
        public void RemoveEnd()
        {
            Assert.AreEqual("", XCLNetTools.StringHander.Common.RemoveEnd("", ""));
            Assert.AreEqual("  ", XCLNetTools.StringHander.Common.RemoveEnd("  ", ""));
            Assert.AreEqual(" ", XCLNetTools.StringHander.Common.RemoveEnd("  ", " "));
            Assert.AreEqual("xYzabC", XCLNetTools.StringHander.Common.RemoveEnd("xYzabC", "abc"));
            Assert.AreEqual("xYz", XCLNetTools.StringHander.Common.RemoveEnd("xYzabC", "abC"));
            Assert.AreEqual("abCABCXYzMn", XCLNetTools.StringHander.Common.RemoveEnd("abCABCXYzMnabC", "abC"));
            Assert.AreEqual("abCabcABCXYabC", XCLNetTools.StringHander.Common.RemoveEnd("abCabcABCXYabCAbC", "ABC", true));
        }

        [TestMethod]
        public void TrimEnd()
        {
            var lst = new List<string>();
            var result = XCLNetTools.StringHander.Common.TrimEnd(lst);
            Assert.AreSame(lst, result);

            lst = new List<string>() { null, "   ", "1", "", null, "", null };
            result = XCLNetTools.StringHander.Common.TrimEnd(lst);
            Assert.IsTrue(result.Count == 3);
            Assert.AreSame(lst[0], result[0]);
            Assert.AreSame(lst[1], result[1]);
            Assert.AreSame(lst[2], result[2]);

            var lst2 = new List<object>() { null, "   ", "1", "", null, "", null };
            var result2 = XCLNetTools.StringHander.Common.TrimEnd(lst2);
            Assert.IsTrue(result2.Count == 3);
            Assert.AreSame(lst2[0], result2[0]);
            Assert.AreSame(lst2[1], result2[1]);
            Assert.AreSame(lst2[2], result2[2]);

            var lst3 = new List<object>() { null, "   ", "", "", null, "", null };
            var result3 = XCLNetTools.StringHander.Common.TrimEnd(lst3);
            Assert.IsTrue(result3.Count == 0);

            var lst4 = new List<int>() { 1, 2, 3, 0, 0, 0 };
            var result4 = XCLNetTools.StringHander.Common.TrimEnd(lst4);
            Assert.AreSame(lst4, result4);
        }

        [TestMethod]
        public void GetLines()
        {
            List<LineItem<string>> lst = XCLNetTools.StringHander.Common.GetLines("", "");
            Assert.IsTrue(lst.Count == 1 && lst[0].Item1 == "" && lst[0].Item2 == "");

            lst = XCLNetTools.StringHander.Common.GetLines("123", "");
            Assert.IsTrue(lst.Count == 1 && lst[0].Item1 == "123" && lst[0].Item2 == "");

            lst = XCLNetTools.StringHander.Common.GetLines("1\n2\r\n3", "6");
            Assert.IsTrue(lst.Count == 3);
            Assert.IsTrue(lst[0].Item1 == "1" && lst[0].Item2 == "6");
            Assert.IsTrue(lst[1].Item1 == "2" && lst[1].Item2 == null);
            Assert.IsTrue(lst[2].Item1 == "3" && lst[2].Item2 == null);

            lst = XCLNetTools.StringHander.Common.GetLines("1\n2\r\n3", "6\n7\n8\r\n9\n10");
            Assert.IsTrue(lst.Count == 5);
            Assert.IsTrue(lst[0].Item1 == "1" && lst[0].Item2 == "6");
            Assert.IsTrue(lst[1].Item1 == "2" && lst[1].Item2 == "7");
            Assert.IsTrue(lst[2].Item1 == "3" && lst[2].Item2 == "8");
            Assert.IsTrue(lst[3].Item1 == null && lst[3].Item2 == "9");
            Assert.IsTrue(lst[4].Item1 == null && lst[4].Item2 == "10");
        }

        [TestMethod]
        public void RemoveWithSafe()
        {
            var str = "";
            Assert.IsTrue(str.RemoveWithSafe(0) == string.Empty);
            str = null;
            Assert.IsTrue(str.RemoveWithSafe(0) == string.Empty);
            str = "123456";
            Assert.AreEqual(str, str.RemoveWithSafe(-1));
            Assert.AreEqual(str, str.RemoveWithSafe(6));
            Assert.AreEqual("", str.RemoveWithSafe(0));
            Assert.AreEqual("1", str.RemoveWithSafe(1));
            Assert.AreEqual("12", str.RemoveWithSafe(2));
            Assert.AreEqual("123", str.RemoveWithSafe(3));
            Assert.AreEqual("1234", str.RemoveWithSafe(4));
            Assert.AreEqual("12345", str.RemoveWithSafe(5));
            Assert.AreEqual("123456", str.RemoveWithSafe(6));
            Assert.AreEqual("123456", str.RemoveWithSafe(7));
            Assert.AreEqual("123456", str.RemoveWithSafe(8));
            Assert.AreEqual("123456", str.RemoveWithSafe(1, 0));
            Assert.AreEqual("13456", str.RemoveWithSafe(1, 1));
            Assert.AreEqual("1456", str.RemoveWithSafe(1, 2));
            Assert.AreEqual("156", str.RemoveWithSafe(1, 3));
            Assert.AreEqual("16", str.RemoveWithSafe(1, 4));
            Assert.AreEqual("1", str.RemoveWithSafe(1, 5));
            Assert.AreEqual("1", str.RemoveWithSafe(1, 6));
            Assert.AreEqual("1", str.RemoveWithSafe(1, 7));
            Assert.AreEqual("23456", str.RemoveWithSafe(0, 1));
            Assert.AreEqual("3456", str.RemoveWithSafe(0, 2));
            Assert.AreEqual("456", str.RemoveWithSafe(0, 3));
            Assert.AreEqual("56", str.RemoveWithSafe(0, 4));
            Assert.AreEqual("6", str.RemoveWithSafe(0, 5));
            Assert.AreEqual("", str.RemoveWithSafe(0, 6));
            Assert.AreEqual("", str.RemoveWithSafe(0, 7));
            Assert.AreEqual("", str.RemoveWithSafe(0, 8));
        }
    }
}