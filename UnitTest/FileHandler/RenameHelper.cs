using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.FileHandler
{
    [TestClass]
    public class RenameHelper
    {
        [TestMethod]
        public void ConvertPathToUniqueList()
        {
            List<string> lst = null;
            List<string> result = null;

            //文件
            lst = new List<string>() {
                @"d:\test\a.txt",
                @"d:\test\a.txt",
                @"d:\test\a.txt\",
                @"d:\test\b.txt\",
                @"d:\test\a.txt",
                @"d:\test\b.txt",
                @"d:\test\a(100).txt",
                @"d:\test\a.txt",
                @"d:\test\b.txt",
                @"d:\test\a.txt",
                @"d:\test\c",
                @"d:\test\a.txt\",
                @"d:\test\c",
                @"d:\test\b.txt\",
                @"d:\test\.txt",
                @"d:\test\.txt"
            };
            result = XCLNetTools.FileHandler.RenameHelper.ConvertPathToUniqueList(lst);
            Assert.AreEqual(lst.Count, result.Count);
            Assert.AreEqual(@"d:\test\a.txt", result[0]);
            Assert.AreEqual(@"d:\test\a(1).txt", result[1]);
            Assert.AreEqual(@"d:\test\a.txt(1)\", result[2]);
            Assert.AreEqual(@"d:\test\b.txt\", result[3]);
            Assert.AreEqual(@"d:\test\a(2).txt", result[4]);
            Assert.AreEqual(@"d:\test\b(1).txt", result[5]);
            Assert.AreEqual(@"d:\test\a(100).txt", result[6]);
            Assert.AreEqual(@"d:\test\a(3).txt", result[7]);
            Assert.AreEqual(@"d:\test\b(2).txt", result[8]);
            Assert.AreEqual(@"d:\test\a(4).txt", result[9]);
            Assert.AreEqual(@"d:\test\c", result[10]);
            Assert.AreEqual(@"d:\test\a.txt(2)\", result[11]);
            Assert.AreEqual(@"d:\test\c(1)", result[12]);
            Assert.AreEqual(@"d:\test\b.txt(1)\", result[13]);
            Assert.AreEqual(@"d:\test\.txt", result[14]);
            Assert.AreEqual(@"d:\test\(1).txt", result[15]);
        }
    }
}