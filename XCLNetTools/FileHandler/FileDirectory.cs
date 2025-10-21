/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.IO;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 文件目录操作类
    /// </summary>
    public static class FileDirectory
    {
        #region 目录操作

        /// <summary>
        /// 检测目录是否为空目录（既没有文件夹，也没有文件）
        /// </summary>
        public static bool IsEmpty(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }
            var files = Directory.GetFiles(path);
            var dirs = Directory.GetDirectories(path);
            return (null == files || files.Length == 0) && (null == dirs || dirs.Length == 0);
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 建立目录
        /// </summary>
        public static bool MakeDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(path);
            }
            catch
            {
                //
            }
            return Directory.Exists(path);
        }

        /// <summary>
        /// 给文件路径创建目录
        /// </summary>
        public static void MakeDirectoryForFile(string path)
        {
            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(XCLNetTools.FileHandler.ComFile.GetFileFolderPath(path));
        }

        /// <summary>
        /// 删除目录并删除其下的子目录及其文件
        /// </summary>
        public static bool DelTree(string path)
        {
            if (DirectoryExists(path))
            {
                Directory.Delete(path, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 清空指定目录
        /// </summary>
        public static bool ClearDirectory(string rootPath)
        {
            //删除子目录
            string[] subPaths = System.IO.Directory.GetDirectories(rootPath);
            foreach (string path in subPaths)
            {
                DelTree(path);
            }
            //删除文件
            string[] files = XCLNetTools.FileHandler.ComFile.GetFolderFiles(rootPath);
            if (null != files && files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    XCLNetTools.FileHandler.ComFile.DeleteFile(files[i]);
                }
            }
            return true;
        }

        /// <summary>
        /// 返回指定文件夹路径的父文件夹地址，如："C:\a\b\c\d\" ---> "C:\a\b\c"
        /// </summary>
        public static string GetDirParentPath(string dirPath)
        {
            if (string.IsNullOrWhiteSpace(dirPath))
            {
                return string.Empty;
            }
            return Path.GetDirectoryName(Path.GetDirectoryName(dirPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar));
        }

        #endregion 目录操作

        #region 文件操作

        /// <summary>
        /// 建立一个文件
        /// </summary>
        public static bool CreateTextFile(string path)
        {
            var info = new FileInfo(path);
            if (info.Exists)
            {
                return true;
            }
            try
            {
                MakeDirectoryForFile(path);
                //如果磁盘不存在，using 语句会异常
                using (var fs = info.Create())
                {
                    //
                }
            }
            catch
            {
                //
            }
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 在文件里追加内容
        /// </summary>
        /// <param name="filePathName">文件名</param>
        /// <param name="writeWord">追加的内容</param>
        /// <param name="encode">编码</param>
        public static void AppendText(string filePathName, string writeWord, System.Text.Encoding encode)
        {
            CreateTextFile(filePathName);
            System.IO.File.AppendAllText(filePathName, writeWord, encode);
        }

        /// <summary>
        /// 读取文本文件里的内容（自动识别编码，如果原编码是ascii，则默认以utf8读取）
        /// </summary>
        /// <param name="filePathName">路径</param>
        /// <returns>文件内容</returns>
        public static string ReadFileData(string filePathName)
        {
            if (!System.IO.File.Exists(filePathName))
            {
                return "";
            }
            var encode = XCLNetTools.FileHandler.ComFile.GetFileEncoding(filePathName);
            if (encode == System.Text.Encoding.ASCII)
            {
                encode = System.Text.Encoding.UTF8;
            }
            return System.IO.File.ReadAllText(filePathName, encode) ?? "";
        }

        /// <summary>
        /// 向路径中写入内容（覆盖路径中的所有内容，如果路径不存在，则自动创建该路径。如果编码是 ascii 且包含 Unicode 字符，则默认以 utf8 写入）
        /// </summary>
        public static void WriteFileData(string filePathName, string content, System.Text.Encoding encode)
        {
            if (encode == System.Text.Encoding.ASCII && XCLNetTools.Encode.Unicode.HasUnicode(content))
            {
                encode = System.Text.Encoding.UTF8;
            }
            System.IO.File.WriteAllText(filePathName, content, encode);
        }

        #endregion 文件操作

        #region 其它

        /// <summary>
        /// 获取当前操作系统桌面的物理路径，如：C:\Users\XCL\Desktop
        /// </summary>
        public static string GetDesktopPath()
        {
            return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        }

        #endregion 其它
    }
}