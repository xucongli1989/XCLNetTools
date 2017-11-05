using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.FileHandler
{
    [TestClass]
    public class ComFile
    {
        [TestMethod]
        public void GetFileExtType()
        {
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType("txt") == XCLNetTools.Enum.CommonEnum.FileExtInfoEnum.Txt);
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType("pdf") == XCLNetTools.Enum.CommonEnum.FileExtInfoEnum.Pdf);
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType("xls") == XCLNetTools.Enum.CommonEnum.FileExtInfoEnum.Office);
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType("mp4") == XCLNetTools.Enum.CommonEnum.FileExtInfoEnum.Video);
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType(" MP3 ") == XCLNetTools.Enum.CommonEnum.FileExtInfoEnum.Music);
            Assert.IsTrue(XCLNetTools.FileHandler.ComFile.GetFileExtType("aaaa") == (XCLNetTools.Enum.CommonEnum.FileExtInfoEnum)(-1));
        }
    }
}