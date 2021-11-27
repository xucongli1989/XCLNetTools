using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using XCLNetTools.Entity;

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
        public void GetStartEndNumList()
        {
            var lst = XCLNetTools.StringHander.Common.GetStartEndNumList(4, 7);
            Assert.IsTrue(lst.Count == 4);
            Assert.IsTrue(lst[0] == 4);
            Assert.IsTrue(lst[1] == 5);
            Assert.IsTrue(lst[2] == 6);
            Assert.IsTrue(lst[3] == 7);
        }
    }
}