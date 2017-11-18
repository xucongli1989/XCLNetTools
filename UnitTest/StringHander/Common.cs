using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}