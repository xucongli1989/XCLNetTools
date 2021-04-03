/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    ///文件操作公共类
    /// </summary>
    public static class ComFile
    {
        #region 删除文件

        /// <summary>
        /// 删除文件
        /// </summary>
        public static bool DeleteFile(string filePath)
        {
            bool isSuccess = false;
            filePath = ComFile.MapPath(filePath);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                    isSuccess = !System.IO.File.Exists(filePath);
                }
                catch
                {
                    //
                }
            }
            else
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        #endregion 删除文件

        #region 复制文件

        /// <summary>
        /// 复制文件(若已存在目标文件则覆盖)，若目标目录不存在，则自动创建
        /// </summary>
        public static bool CopyFile(string srcPath, string dstPath)
        {
            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(GetFileFolderPath(dstPath));
            return CopyFile(srcPath, dstPath, true);
        }

        /// <summary>
        /// 复制文件，若目标目录不存在，则自动创建
        /// </summary>
        public static bool CopyFile(string srcPath, string dstPath, bool overwrite)
        {
            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(GetFileFolderPath(dstPath));
            File.Copy(ComFile.MapPath(srcPath), ComFile.MapPath(dstPath), overwrite);
            return File.Exists(ComFile.MapPath(dstPath));
        }

        /// <summary>
        /// 复制整个文件夹
        /// </summary>
        /// <param name="sourceDirName">原路径</param>
        /// <param name="destDirName">目标路径</param>
        /// <param name="copySubDirs">是否复制子目录</param>
        public static void CopyDir(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("目录不存在：" + sourceDirName);
            }
            DirectoryInfo[] dirs = dir.GetDirectories();
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDir(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

        #endregion 复制文件

        #region 取得文件夹中的文件列表

        /// <summary>
        /// 取得文件夹中的文件列表
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>字符串数组(存储了一个或多个文件名)</returns>
        public static string[] GetFolderFiles(string path)
        {
            try
            {
                return System.IO.Directory.GetFiles(ComFile.MapPath(path));
            }
            catch
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// 递归获取指定文件夹下的所有文件路径
        /// </summary>
        /// <param name="rootPath">起始文件夹路径</param>
        /// <returns>文件路径数组</returns>
        public static string[] GetFolderFilesByRecursion(string rootPath)
        {
            List<string> lst = new List<string>();
            GetFolderFilesByRecursion(rootPath, lst);
            return lst.ToArray();
        }

        /// <summary>
        /// 递归获取指定文件夹下的所有文件路径
        /// </summary>
        /// <param name="rootPath">起始文件夹路径</param>
        /// <param name="lst">文件路径存放的list</param>
        private static void GetFolderFilesByRecursion(string rootPath, List<string> lst)
        {
            string[] subPaths = GetFolders(rootPath);
            foreach (string path in subPaths)
            {
                GetFolderFilesByRecursion(path, lst);
            }
            string[] files = GetFolderFiles(rootPath);
            if (null != files && files.Length > 0)
            {
                lst.AddRange(files);
            }
        }

        #endregion 取得文件夹中的文件列表

        #region 取得文件夹中的文件夹列表

        /// <summary>
        /// 取得文件夹中的子文件夹列表
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <returns>字符串数组(存储了一个或多个文件夹名)</returns>
        public static string[] GetFolders(string path)
        {
            try
            {
                return System.IO.Directory.GetDirectories(ComFile.MapPath(path));
            }
            catch
            {
                return new string[] { };
            }
        }

        /// <summary>
        /// 递归获取指定文件夹下的所有子文件夹的路径
        /// </summary>
        /// <param name="rootPath">起始文件夹路径</param>
        /// <returns>所有子文件夹路径数组</returns>
        public static string[] GetFoldersByRecursion(string rootPath)
        {
            List<string> lst = new List<string>();
            GetFoldersByRecursion(rootPath, lst);
            return lst.ToArray();
        }

        /// <summary>
        /// 递归获取指定文件夹下的所有子文件夹路径
        /// </summary>
        /// <param name="rootPath">起始文件夹路径</param>
        /// <param name="lst">文件夹路径存放的list</param>
        private static void GetFoldersByRecursion(string rootPath, List<string> lst)
        {
            string[] subPaths = GetFolders(rootPath);
            foreach (string path in subPaths)
            {
                GetFoldersByRecursion(path, lst);
            }
            string[] folders = GetFolders(rootPath);
            if (null != folders && folders.Length > 0)
            {
                lst.AddRange(folders);
            }
        }

        #endregion 取得文件夹中的文件夹列表

        #region 文件下载

        /// <summary>
        /// 文件下载
        /// </summary>
        /// <param name="path">文件链接（物理路径）</param>
        /// <param name="realName">要显示下载时的文件名</param>
        public static void DownLoadFile(string path, string realName)
        {
            path = ComFile.MapPath(path);
            realName = string.IsNullOrEmpty(realName) ? "文件下载" : realName;
            FileInfo info = new FileInfo(path);
            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.ClearHeaders();
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            string exp = HttpContext.Current.Request.UserAgent.ToLower();
            if (exp.IndexOf("msie") > -1)
            {
                //当客户端使用IE时，对其进行编码；We should encode the filename when our visitors use IE
                //使用 ToHexString 代替传统的 UrlEncode()；We use "ToHexString" replaced "context.Server.UrlEncode(fileName)"
                realName = XCLNetTools.Encode.Hex.ToHexString(realName);
            }
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + realName.Replace("+", "_").Replace(" ", ""));
            Response.AppendHeader("Content-Length", info.Length.ToString());
            Response.WriteFile(path);
            Response.Flush();
            Response.End();
        }

        #endregion 文件下载

        #region 目录相关

        /// <summary>
        /// 返回目录路径，若该目录不存在，则创建该目录
        /// </summary>
        public static string GetSaveDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        /// <summary>
        /// 获取文件所在的文件夹路径【不带'\'】，如："C:\a\b\c\file.txt" --> "C:\a\b\c"
        /// </summary>
        public static string GetFileFolderPath(string filePath)
        {
            return System.IO.Path.GetDirectoryName(filePath);
        }

        /// <summary>
        /// 使用新的文件名，更新指定路径path中的文件名。如：（"C:\demo\a.txt","abcd.doc"）=>C:\demo\abcd.doc
        /// </summary>
        /// <param name="path">文件物理路径</param>
        /// <param name="name">新的文件名（如：a.txt）</param>
        /// <returns>新的文件物理路径</returns>
        public static string ChangePathByFileName(string path, string name)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return System.IO.Path.GetDirectoryName(path) + '\\' + name;
        }

        #endregion 目录相关

        #region 文件属性相关

        /// <summary>
        /// 返回文件大小(字节)
        /// </summary>
        /// <returns>文件大小 byte</returns>
        public static long GetFileSize(string filePath)
        {
            long s = 0;
            if (System.IO.File.Exists(ComFile.MapPath(filePath)))
            {
                FileInfo fi = new FileInfo(ComFile.MapPath(filePath));
                s = fi.Length;
            }
            return s;
        }

        #endregion 文件属性相关

        #region 文件类型判断

        /// <summary>
        /// 判断文件是否是二进制文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回True为二进制文件，否则是文本文件</returns>
        public static bool IsBinaryFile(string filePath)
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                int len = (int)fs.Length;
                int count = 0;
                byte[] content = new byte[len];
                int size = fs.Read(content, 0, len);

                for (int i = 0; i < size; i++)
                {
                    if (content[i] == 0)
                    {
                        count++;
                        if (count == 4)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        count = 0;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 判断文件是否是文本文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>返回True为文本文件，否则是二进制文件</returns>
        public static bool IsTextFile(string filePath)
        {
            using (FileStream file = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] byteData = new byte[1];
                while (file.Read(byteData, 0, byteData.Length) > 0)
                {
                    if (byteData[0] == 0)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 判断指定的文件扩展名属于哪种文件类型（若返回-1，则表示在本库预设的类型中没有找到）
        /// </summary>
        /// <param name="ext">文件扩展名</param>
        /// <returns>文件类型枚举</returns>
        public static XCLNetTools.Enum.CommonEnum.FileExtInfoEnum GetFileExtType(string ext)
        {
            var result = (XCLNetTools.Enum.CommonEnum.FileExtInfoEnum)(-1);
            if (string.IsNullOrWhiteSpace(ext))
            {
                return result;
            }
            ext = ext.Trim().Trim('.').ToLower();
            var model = XCLNetTools.Enum.CommonEnum.EnumDic["FileExtInfoEnum"]?.FirstOrDefault(k => (k.Description ?? "").Split(',').ToList().Contains(ext));
            if (null == model)
            {
                return result;
            }
            XCLNetTools.Enum.CommonEnum.FileExtInfoEnum temp;
            if (System.Enum.TryParse(model.Text, out temp))
            {
                result = temp;
            }
            return result;
        }

        #endregion 文件类型判断

        #region 其它

        /// <summary>
        /// 取得文件物理路径
        /// </summary>
        /// <param name="path">文件路径(如果为绝对路径，则直接返回，否则，转为绝对路径)</param>
        /// <returns>文件物理路径</returns>
        public static string MapPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            if (Path.IsPathRooted(path))
            {
                return path;
            }
            var current = HttpContext.Current;
            if (null != current)
            {
                return current.Server.MapPath(path);
            }
            return Path.Combine(System.Environment.CurrentDirectory, path.TrimStart(@"~/\".ToCharArray()).Replace('/', '\\'));
        }

        /// <summary>
        /// 根据指定文件的物理路径Path，将它转换为相对于RootPath的Url相对路径
        /// 例如：
        /// GetUrlRelativePath("C:\Program Files\Information\","C:\Program Files\Information\A\B\C.txt")=>"A/B/C.txt"
        /// GetUrlRelativePath("C:\Program Files\Information\","C:\A\B\C.txt")=>"../../A/B/C.txt"
        /// </summary>
        /// <param name="rootPath">根物理路径</param>
        /// <param name="path">指定要转换的物理路径</param>
        /// <returns>path相对于rootPath的url路径</returns>
        public static string GetUrlRelativePath(string rootPath, string path)
        {
            if (string.IsNullOrEmpty(rootPath) || string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            Uri root = new Uri(ComFile.MapPath(rootPath));
            Uri p = new Uri(ComFile.MapPath(path));
            return root.MakeRelativeUri(p).ToString();
        }

        /// <summary>
        /// 获取文件名，如："C:\a\b\c\file.txt" --> "file.txt"
        /// </summary>
        /// <param name="path">路径（相对或绝对均可）</param>
        /// <param name="isWithExt">是否包含扩展名</param>
        /// <returns>文件名</returns>
        public static string GetFileName(string path, bool isWithExt = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return isWithExt ? System.IO.Path.GetFileName(path) : System.IO.Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        ///返回随机的文件命名，如：（"C:\demo\a.exe"）=>"20170408090024LGP9JWOAO2.exe"
        /// </summary>
        /// <param name="fileName">文件名或路径</param>
        /// <returns>新的文件名（不含路径）</returns>
        public static string GetRandomFileName(string fileName)
        {
            string num = string.Empty;
            Random r = new Random();
            for (var i = 0; i <= 9; i++)
            {
                num = num + XCLNetTools.Common.Consts.NumberEngLetterChar[r.Next(0, XCLNetTools.Common.Consts.NumberEngLetterChar.Length)].ToString();
            }
            return DateTime.Now.ToString("yyyyMMddHHmmss") + num + GetExtName(fileName, true);
        }

        /// <summary>
        /// 取得文件扩展名（默认不包含小圆点）【小写】
        /// </summary>
        /// <param name="fileName">路径或文件名</param>
        /// <param name="withDot">是否包含小圆点（默认：false）</param>
        /// <returns>文件扩展名</returns>
        public static string GetExtName(string fileName, bool withDot = false)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }
            if (withDot)
            {
                return System.IO.Path.GetExtension(fileName);
            }
            return System.IO.Path.GetExtension(fileName).TrimStart('.');
        }

        /// <summary>
        /// 用新的扩展名更新文件路径
        /// </summary>
        /// <param name="path">原文件路径</param>
        /// <param name="newExt">扩展名</param>
        /// <returns>新的文件路径</returns>
        public static string GetFilePathWithNewExt(string path, string newExt)
        {
            if (null != newExt)
            {
                newExt = newExt.Trim().Trim('.');
            }
            var dot = string.IsNullOrWhiteSpace(newExt) ? string.Empty : ".";
            return ChangePathByFileName(path, string.Format("{0}{1}{2}", GetFileName(path, false), dot, newExt));
        }

        /// <summary>
        /// 根据文件路径，获取文件编码（若获取失败，则返回：Encoding.Default）
        /// 注意：需要引用 UtfUnknown 包
        /// </summary>
        public static Encoding GetFileEncoding(string filename)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var defaultEncoding = System.Text.Encoding.Default;
            var detectorResult = UtfUnknown.CharsetDetector.DetectFromFile(filename);
            if (null == detectorResult || null == detectorResult.Detected)
            {
                return defaultEncoding;
            }
            if (null != detectorResult.Detected.Encoding)
            {
                return detectorResult.Detected.Encoding;
            }
            if (!string.IsNullOrWhiteSpace(detectorResult.Detected.EncodingName))
            {
                return System.Text.Encoding.GetEncoding(detectorResult.Detected.EncodingName);
            }
            return defaultEncoding;
        }

        /// <summary>
        /// 过滤路径列表，只保留为根目录的路径。
        /// 比如：c:\a\b\，c:\a\b\1\，c:\a\b\2\。最终只保留：c:\a\b\
        /// </summary>
        public static HashSet<string> GetDirRootPathList(List<string> pathList)
        {
            var result = new HashSet<string>();
            if (null == pathList || pathList.Count == 0)
            {
                return result;
            }
            if (pathList.Count == 1)
            {
                result.Add(pathList[0]);
                return result;
            }
            //先进行排序，父目录会在前面
            var soredPathList = pathList.AsParallel().OrderBy(k => k).ToList();
            //提取父目录
            soredPathList.ForEach(path =>
            {
                if (string.IsNullOrWhiteSpace(path))
                {
                    return;
                }
                path = path.Trim();
                var parentPath = result.FirstOrDefault(k => path.StartsWith(k, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrWhiteSpace(parentPath))
                {
                    return;
                }
                result.Add(path);
            });
            return result;
        }

        #endregion 其它
    }
}