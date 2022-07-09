using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.FileHandler
{
    [TestClass]
    public class ComFile
    {
        [TestMethod]
        public void IsLocalDiskRootPath()
        {
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(""));
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath("/a/b/"));
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"c:\a\"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"c:"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"c:\"));
        }

        [TestMethod]
        public void GetLocalPathDiskName()
        {
            Assert.AreEqual("", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(""));
            Assert.AreEqual("", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName("/a/b/"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:\a\"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:\"));
        }

        [TestMethod]
        public void GetPathFolderName()
        {
            //文件夹
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\", true));
            Assert.AreEqual("d", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c\d", true));
            Assert.AreEqual("d", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c\d\", true));
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
        public void GetStandardPath()
        {
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b", true));
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b/", true));
            Assert.AreEqual(@"c:\a\b", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b/", false));
            Assert.AreEqual(@"c:\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b.docx/", false));
            Assert.AreEqual(@"c:\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b.docx", false));
        }
    }
}