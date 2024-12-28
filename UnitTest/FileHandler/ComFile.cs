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
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"\\pc\a"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"\\pc"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsLocalDiskRootPath(@"\\pc\"));
        }

        [TestMethod]
        public void GetLocalPathDiskName()
        {
            Assert.AreEqual("", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(""));
            Assert.AreEqual("", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName("/a/b/"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:\a\"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:"));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"c:\"));
            Assert.AreEqual("pc", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"\\pc\a.docx"));
            Assert.AreEqual("pc", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"\\pc\a\b\c\"));
            Assert.AreEqual("pc", XCLNetTools.FileHandler.ComFile.GetLocalPathDiskName(@"\\pc"));
        }

        [TestMethod]
        public void IsFileOrFolderInRootPath()
        {
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(""));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"c:"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"c:\a\"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"c:\a.txt"));
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"c:\a\b"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"\\pc"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"\\pc\a"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"\\pc\a.txt"));
            Assert.AreEqual(true, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"\\pc\a\"));
            Assert.AreEqual(false, XCLNetTools.FileHandler.ComFile.IsFileOrFolderInRootPath(@"\\pc\a\b"));
        }

        [TestMethod]
        public void GetFileFolderPath()
        {
            Assert.AreEqual(@"c:\", XCLNetTools.FileHandler.ComFile.GetFileFolderPath(@"c:\a.txt"));
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetFileFolderPath(@"c:\a\b\c"));
            Assert.AreEqual(@"\\pc\", XCLNetTools.FileHandler.ComFile.GetFileFolderPath(@"\\pc\a.txt"));
            Assert.AreEqual(@"\\pc\a\b\", XCLNetTools.FileHandler.ComFile.GetFileFolderPath(@"\\pc\a\b\c"));
        }

        [TestMethod]
        public void ChangePathByFileName()
        {
            Assert.AreEqual(@"c:\b", XCLNetTools.FileHandler.ComFile.ChangePathByFileName(@"c:\a.txt", "b"));
            Assert.AreEqual(@"c:\a\b\b", XCLNetTools.FileHandler.ComFile.ChangePathByFileName(@"c:\a\b\c", "b"));
            Assert.AreEqual(@"\\pc\b", XCLNetTools.FileHandler.ComFile.ChangePathByFileName(@"\\pc\a.txt", "b"));
            Assert.AreEqual(@"\\pc\a\b\b", XCLNetTools.FileHandler.ComFile.ChangePathByFileName(@"\\pc\a\b\c", "b"));
        }

        [TestMethod]
        public void GetPathFolderName()
        {
            //文件夹
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\", true));
            Assert.AreEqual("d", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c\d", true));
            Assert.AreEqual("d", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c\d\", true));
            Assert.AreEqual("pc", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc", true));
            Assert.AreEqual("a", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc\a", true));
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc\a\b\c\", true));

            //文件
            Assert.AreEqual("c", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a", false));
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c", false));
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"c:\a\b\c.pdf", false));
            Assert.AreEqual("pc", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc\a", false));
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc\a\b\c", false));
            Assert.AreEqual("b", XCLNetTools.FileHandler.ComFile.GetPathFolderName(@"\\pc\a\b\c.pdf", false));
        }

        [TestMethod]
        public void ChangePathByFolderName()
        {
            //文件夹
            Assert.AreEqual(@"c:\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a", "x", true));
            Assert.AreEqual(@"c:\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c", "x", true));
            Assert.AreEqual(@"c:\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c\", "x", true));
            Assert.AreEqual(@"\\pc\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"\\pc\a", "x", true));
            Assert.AreEqual(@"\\pc\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"\\pc\a\b\c", "x", true));
            Assert.AreEqual(@"\\pc\a\b\x\", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"\\pc\a\b\c\", "x", true));
            //文件
            Assert.AreEqual(@"c:\a\x\c", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c", "x", false));
            Assert.AreEqual(@"c:\a\x\c", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"c:\a\b\c\", "x", false));
            Assert.AreEqual(@"\\pc\a\x\c", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"\\pc\a\b\c", "x", false));
            Assert.AreEqual(@"\\pc\a\x\c", XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(@"\\pc\a\b\c\", "x", false));
        }

        [TestMethod]
        public void GetStandardPath()
        {
            Assert.AreEqual(@"c:\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:", true));
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b", true));
            Assert.AreEqual(@"c:\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b/", true));
            Assert.AreEqual(@"c:\a\b", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b/", false));
            Assert.AreEqual(@"c:\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b.docx/", false));
            Assert.AreEqual(@"c:\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"c:/a/b.docx", false));

            Assert.AreEqual(@"\\pc\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc", true));
            Assert.AreEqual(@"\\pc\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc/a/b", true));
            Assert.AreEqual(@"\\pc\a\b\", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc/a/b/", true));
            Assert.AreEqual(@"\\pc\a\b", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc/a/b/", false));
            Assert.AreEqual(@"\\pc\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc/a/b.docx/", false));
            Assert.AreEqual(@"\\pc\a\b.docx", XCLNetTools.FileHandler.ComFile.GetStandardPath(@"\\pc/a/b.docx", false));
        }

        [TestMethod]
        public void AppendPathAsSubPath()
        {
            Assert.AreEqual(@"", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("", "", true));
            Assert.AreEqual(@"c:\a\b\c\x\y\", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("c:/a/b", "c:/x/y/", true));
            Assert.AreEqual(@"c:\a\b\x\y\", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("c:/a/b", "/x/y/", true));
            Assert.AreEqual(@"c:\a\b\test-pc\y\a.docx\", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("c:/a/b", "//test-pc\\y/a.docx", true));
            Assert.AreEqual(@"c:\a\b\test-pc\y\a.docx", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("c:/a/b", "//test-pc\\y/a.docx", false));
            Assert.AreEqual(@"c:\a\b\x\y\z\", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("c:/a/b", "x/y/z", true));
            Assert.AreEqual(@"\\test-pc\a\b\c\x\y\", XCLNetTools.FileHandler.ComFile.AppendPathAsSubPath("//test-pc/a/b", "c:/x/y/", true));
        }
    }
}