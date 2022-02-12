using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.FileHandler
{
    [TestClass]
    public class ComFile
    {
        [TestMethod]
        public void GetPathFolderName()
        {
            //文件夹
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c", true));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c\", true));
            //文件
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c", false));
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c.pdf", false));
        }

        [TestMethod]
        public void ChangePathByFolderName()
        {
            //文件夹
            Assert.AreEqual(@"c:\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c", "x", true));
            Assert.AreEqual(@"c:\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c\", "x", true));
            //文件
            Assert.AreEqual(@"c:\a\x\c", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c", "x", false));
            Assert.AreEqual(@"c:\a\x\c.pdf", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c.pdf", "x", false));
        }

        [TestMethod]
        public void GetOptimizedFolderPath()
        {
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetOptimizedFolderPath(@"c:\a\b"));
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetOptimizedFolderPath(@"c:\a\b\"));
        }
    }
}