using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Serialize
{
    [TestClass]
    public class JSON
    {
        [TestMethod]
        public void IsJSON()
        {
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON(null));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON(""));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON("1"));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON("a"));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON("[}"));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON("{]"));
            Assert.IsTrue(XCLNetTools.Serialize.JSON.IsJSON("{}"));
            Assert.IsTrue(XCLNetTools.Serialize.JSON.IsJSON("[]"));
            Assert.IsTrue(XCLNetTools.Serialize.JSON.IsJSON(@"{""id"":123}"));
            Assert.IsTrue(XCLNetTools.Serialize.JSON.IsJSON(@"[{""id"":123}]"));
            Assert.IsFalse(XCLNetTools.Serialize.JSON.IsJSON("[{i d:123}]"));
        }
    }
}