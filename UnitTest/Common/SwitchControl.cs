using Microsoft.VisualStudio.TestTools.UnitTesting;
using XCLNetTools.Entity;

namespace UnitTest.Common
{
    [TestClass]
    public class SwitchControl
    {
        [TestMethod]
        public void TFToBool()
        {
            Assert.IsFalse(XCLNetTools.Common.SwitchControl.TFToBool(""));
            Assert.IsFalse(XCLNetTools.Common.SwitchControl.TFToBool(null));
            Assert.IsFalse(XCLNetTools.Common.SwitchControl.TFToBool("F"));
            Assert.IsFalse(XCLNetTools.Common.SwitchControl.TFToBool("f"));
            Assert.IsTrue(XCLNetTools.Common.SwitchControl.TFToBool("T"));
            Assert.IsTrue(XCLNetTools.Common.SwitchControl.TFToBool("t"));
        }

        [TestMethod]
        public void IsOpen()
        {
            MethodResult<bool> result = null;
            result = XCLNetTools.Common.SwitchControl.IsOpen("", "");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("T");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("f");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("F");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("a");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc", "");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc", "abc");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=t", "abc");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=t", "bb");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=f", "bb");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=-1", "abc");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=101", "abc");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&v=100", "aa");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&f=bb&v=t", "bb");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen("t=abc&f=&v=t", "bb");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen(@"t=abc&rt=^\d+&v=0", "1abc");
            Assert.IsTrue(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen(@"t=abc&rf=^\d+&v=0", "1abc");
            Assert.IsFalse(result.Result);

            result = XCLNetTools.Common.SwitchControl.IsOpen(@"t=abc&rt=^\d+&v=100", "aa");
            Assert.IsTrue(result.Result);
        }
    }
}