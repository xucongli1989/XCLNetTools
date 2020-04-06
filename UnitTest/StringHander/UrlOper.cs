using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;

namespace UnitTest.StringHander
{
    [TestClass]
    public class UrlOper
    {
        [TestMethod]
        public void AddParam()
        {
            string url = "http://www.test.com";
            Assert.AreEqual("http://www.test.com", XCLNetTools.StringHander.UrlOper.AddParam(url, null, null));
            Assert.AreEqual("http://www.test.com", XCLNetTools.StringHander.UrlOper.AddParam(url, "", ""));
            Assert.AreEqual("http://www.test.com", XCLNetTools.StringHander.UrlOper.AddParam(url, "  ", ""));
            Assert.AreEqual("http://www.test.com?name=", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", null));
            Assert.AreEqual("http://www.test.com?name=", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", ""));
            Assert.AreEqual("http://www.test.com?name=", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "  "));
            Assert.AreEqual("http://www.test.com?name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com?";
            Assert.AreEqual("http://www.test.com?name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com?     ";
            Assert.AreEqual("http://www.test.com?name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com&";
            Assert.AreEqual("http://www.test.com?name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com?a=b";
            Assert.AreEqual("http://www.test.com?a=b&name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com?a=b&c=d";
            Assert.AreEqual("http://www.test.com?a=b&c=d&name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, "name", "hi"));

            url = "http://www.test.com";
            var nv = new NameValueCollection() { };
            Assert.AreEqual("http://www.test.com", XCLNetTools.StringHander.UrlOper.AddParam(url, nv));

            url = "http://www.test.com";
            nv = new NameValueCollection() {
                { "name","hi"},{ "age","10"}
            };
            Assert.AreEqual("http://www.test.com?name=hi&age=10", XCLNetTools.StringHander.UrlOper.AddParam(url, nv));

            url = "http://www.test.com";
            nv = new NameValueCollection() {
                { "","hi"}
            };
            Assert.AreEqual("http://www.test.com", XCLNetTools.StringHander.UrlOper.AddParam(url, nv));

            url = "http://www.test.com";
            nv = new NameValueCollection() {
                { "name","  hi   "}
            };
            Assert.AreEqual("http://www.test.com?name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, nv));

            url = "http://www.test.com?a=b";
            nv = new NameValueCollection() {
                { "name","  hi   "}
            };
            Assert.AreEqual("http://www.test.com?a=b&name=hi", XCLNetTools.StringHander.UrlOper.AddParam(url, nv));
        }
    }
}