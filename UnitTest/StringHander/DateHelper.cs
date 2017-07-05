using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest.StringHander
{
    [TestClass]
    public class DateHelper
    {
        [TestMethod]
        public void GetDateString()
        {
            var dt = Convert.ToDateTime("2017-12-28");
            Assert.IsTrue(XCLNetTools.StringHander.DateHelper.GetDateString(dt) == dt.ToString());
            Assert.IsTrue(XCLNetTools.StringHander.DateHelper.GetDateString(dt, XCLNetTools.Enum.CommonEnum.DateFormat.dd_MMM_yyyy) == "28 Dec 2017");
        }
    }
}