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
using System.Text.RegularExpressions;
using System.Web;
using XCLNetTools.Entity;
using XCLNetTools.Generic;

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
        public static bool DeleteFile(string filePath, bool isMoveToRecycle = false)
        {
            bool isSuccess = false;
            filePath = ComFile.MapPath(filePath);
            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    if (isMoveToRecycle)
                    {
                        Microsoft.VisualBasic.FileIO.FileSystem.DeleteFile(filePath, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.RecycleOption.SendToRecycleBin, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
                    }
                    else
                    {
                        System.IO.File.Delete(filePath);
                    }
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

        /// <summary>
        /// 删除文件或文件夹
        /// </summary>
        public static bool DeletePath(string path)
        {
            if (XCLNetTools.FileHandler.ComFile.IsPathExists(path, true))
            {
                return XCLNetTools.FileHandler.FileDirectory.DelTree(path);
            }
            else
            {
                return XCLNetTools.FileHandler.ComFile.DeleteFile(path);
            }
        }

        #endregion 删除文件

        #region 复制文件

        /// <summary>
        /// 复制文件，若目标目录不存在，则自动创建
        /// </summary>
        public static bool CopyFile(string srcPath, string dstPath, bool overwrite = true)
        {
            srcPath = ComFile.MapPath(srcPath);
            dstPath = ComFile.MapPath(dstPath);

            //原文件的时间
            var createTime = File.GetCreationTime(srcPath);
            var modifyTime = File.GetLastWriteTime(srcPath);
            var accessTime = File.GetLastAccessTime(srcPath);

            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(GetFileFolderPath(dstPath));
            File.Copy(srcPath, dstPath, overwrite);

            if (File.Exists(dstPath))
            {
                try
                {
                    //还原为原文件的时间
                    File.SetCreationTime(dstPath, createTime);
                    File.SetLastWriteTime(dstPath, modifyTime);
                    File.SetLastAccessTime(dstPath, accessTime);
                }
                catch
                {
                    //
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 复制整个文件夹中的内容到目标文件夹中
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

        /// <summary>
        /// 复制整个文件夹的内容并将此文件夹作为目标文件夹的一个子文件夹
        /// </summary>
        /// <param name="sourceDirName">原路径</param>
        /// <param name="destDirName">目标路径</param>
        /// <param name="copySubDirs">是否复制子目录</param>
        public static void CopyDirAsSub(string sourceDirName, string destDirName, bool copySubDirs)
        {
            destDirName = Path.Combine(destDirName, GetPathFolderName(sourceDirName, true));
            ComFile.CopyDir(sourceDirName, destDirName, copySubDirs);
        }

        #endregion 复制文件

        #region 取得文件夹中的文件列表

        /// <summary>
        /// 取得文件夹中的文件列表
        /// </summary>
        /// <param name="path">文件夹路径</param>
        /// <param name="isContainsHiddenFile">是否包含隐藏文件，默认为：true</param>
        /// <returns>字符串数组(存储了一个或多个文件名)</returns>
        public static string[] GetFolderFiles(string path, bool isContainsHiddenFile = true)
        {
            path = ComFile.MapPath(path);
            try
            {
                var arr = System.IO.Directory.GetFiles(path);
                if (arr.Length > 0 && !isContainsHiddenFile)
                {
                    arr = arr.AsParallel().Where(k => !ComFile.IsHiddenFile(k)).ToArray();
                }
                return arr;
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
            path = ComFile.MapPath(path);
            try
            {
                return System.IO.Directory.GetDirectories(path);
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

        #region 路径相关

        /// <summary>
        /// 判断某个路径是否为本地磁盘根路径，如：c:\  或  \\test-pc\
        /// </summary>
        public static bool IsLocalDiskRootPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            path = ComFile.GetStandardPath(path, true);
            return new Regex(@"^[^\\]+:\\$").IsMatch(path) || new Regex(@"^\\\\[^\\]+\\$").IsMatch(path);
        }

        /// <summary>
        /// 获取路径中的磁盘名称，如：c:\a\b\c\ -> c，\\test-pc\a\b\c\ -> test-pc
        /// </summary>
        public static string GetLocalPathDiskName(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            path = ComFile.GetStandardPath(path, true);//此逻辑不区分文件还是文件夹，只是方便后面读取
            //普通路径
            var mt = new Regex(@"^[^\\]+(?=:)").Match(path);
            if (mt.Success)
            {
                return mt.Value;
            }
            //网络路径
            mt = new Regex(@"^\\\\([^\\]+)(?=\\)").Match(path);
            if (mt.Success)
            {
                return mt.Groups[1].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 判断一个文件或文件夹的路径是否在根目录中，如：c:\test.docx，\\pc\test.docx
        /// </summary>
        public static bool IsFileOrFolderInRootPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }
            path = ComFile.GetStandardPath(path, true);//此逻辑不区分文件还是文件夹，只是方便后面读取
            if (ComFile.IsLocalDiskRootPath(path))
            {
                return true;
            }
            return new Regex(@"^[^\\]+:\\[^\\]+\\$").IsMatch(path) || new Regex(@"^\\\\[^\\]+\\[^\\]+\\$").IsMatch(path);
        }

        /// <summary>
        /// 获取文件所在的文件夹路径，如："C:\a\b\c\file.txt" --> "C:\a\b\c\"
        /// </summary>
        public static string GetFileFolderPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            path = ComFile.GetStandardPath(path, false);
            if (ComFile.IsFileOrFolderInRootPath(path))
            {
                return path.Substring(0, path.LastIndexOf(@"\") + 1);
            }
            return ComFile.GetStandardPath(System.IO.Path.GetDirectoryName(path), true);
        }

        /// <summary>
        /// 使用新的文件名，更新指定路径path中的文件名。如：（"C:\demo\a.txt","abcd.doc"）=>C:\demo\abcd.doc
        /// </summary>
        public static string ChangePathByFileName(string path, string name)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            return System.IO.Path.Combine(ComFile.GetFileFolderPath(path), name);
        }

        /// <summary>
        /// 使用新的不包含扩展名的文件名，更新指定路径path中的文件名。如：（"C:\demo\a.txt","abcd"）=>C:\demo\abcd.txt
        /// </summary>
        public static string ChangePathByFileNameWithoutExt(string path, string name)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            var dir = ComFile.GetFileFolderPath(path);
            var ext = ComFile.GetExtName(path, false, false);
            var newName = ComFile.AppendExtToPath(name, ext);
            var newPath = Path.Combine(dir, newName);
            return newPath;
        }

        /// <summary>
        /// 获取路径所在的文件夹名称，如：c:\  --> c；  c:\a\b\ --> b;  c:\a\b\c.pdf --> b
        /// </summary>
        public static string GetPathFolderName(string path, bool isFolderPath)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            path = ComFile.GetStandardPath(path, isFolderPath);
            var arr = path.Split('\\');
            return arr[arr.Length - 2].Replace(":", "");
        }

        /// <summary>
        /// 使用新的文件夹名，更新指定路径path中的文件夹名。如：（"C:\demo\a\","b"）=>C:\demo\b\  ;  （"C:\demo\a\b.pdf","c"）=>C:\demo\c\b.pdf
        /// </summary>
        public static string ChangePathByFolderName(string path, string name, bool isFolderPath)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            path = ComFile.GetStandardPath(path, isFolderPath);
            if (ComFile.IsLocalDiskRootPath(path) || (!isFolderPath && ComFile.IsFileOrFolderInRootPath(path)))
            {
                throw new ArgumentOutOfRangeException("path", "不允许修改路径中的根名称！");
            }
            var arr = path.Split('\\');
            arr[arr.Length - 2] = name;
            return string.Join(@"\", arr);
        }

        /// <summary>
        /// 将文件夹、文件名、扩展名拼接成文件路径
        /// </summary>
        public static string CombineFilePath(string folderPath, string nameWithoutExt, string ext)
        {
            var p = Path.Combine(folderPath, nameWithoutExt);
            return AppendExtToPath(p, ext);
        }

        /// <summary>
        /// 在一个路径的后面以子目录的方式追加一个完整的路径。与原生 Path.Combine 的区别是如果第二个参数是完整的物理路径，则会自动将盘符转换为子目录的形式，而不是原生中的直接返回第二个参数。
        /// 如：(c:\a\b\,d:\x\y\) -> c:\a\b\d\x\y\
        /// </summary>
        /// <param name="path1">路径1</param>
        /// <param name="path2">路径2（支持 d:\test\ 或 \\test-pc\test 格式）</param>
        /// <param name="isFolder">最终返回的路径是否为文件夹</param>
        public static string AppendPathAsSubPath(string path1, string path2, bool isFolder)
        {
            path2 = path2.Trim().Replace("/", @"\").Replace(":", "").TrimStart('\\');
            return GetStandardPath(Path.Combine(path1, path2), isFolder);
        }

        /// <summary>
        /// 获取某个文件夹路径下的所有【文件+文件夹】的路径列表
        /// </summary>
        /// <param name="rootDirPath">根目录</param>
        /// <param name="isRootDirPathInResult">是否需要将根目录放到返回的结果中</param>
        /// <param name="isNeedRecursion">是否需要递归子目录</param>
        public static List<PathInfoEntity> GetPathList(string rootDirPath, bool isRootDirPathInResult = false, bool isNeedRecursion = false)
        {
            var lst = new List<PathInfoEntity>();
            if (string.IsNullOrWhiteSpace(rootDirPath) || !System.IO.Directory.Exists(rootDirPath))
            {
                return lst;
            }
            rootDirPath = XCLNetTools.FileHandler.ComFile.GetStandardPath(rootDirPath, true);

            //将根目录放到结果列表中
            if (isRootDirPathInResult)
            {
                lst.Add(new PathInfoEntity()
                {
                    Path = rootDirPath,
                    IsFolder = true
                });
            }

            //递归遍历
            Action<string> act = null;
            act = new Action<string>((dir) =>
            {
                //文件
                var files = XCLNetTools.FileHandler.ComFile.GetFolderFiles(dir).ToList();
                files.ForEach(p =>
                {
                    var model = new PathInfoEntity();
                    model.Path = p;
                    model.IsFolder = false;
                    lst.Add(model);
                });

                //文件夹
                var folders = XCLNetTools.FileHandler.ComFile.GetFolders(dir).ToList();
                folders.ForEach(p =>
                {
                    var model = new PathInfoEntity();
                    model.Path = XCLNetTools.FileHandler.ComFile.GetStandardPath(p, true);
                    model.IsFolder = true;
                    lst.Add(model);

                    //递归
                    if (isNeedRecursion)
                    {
                        act(p);
                    }
                });
            });

            act(rootDirPath);

            return lst;
        }

        #endregion 路径相关

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
        /// 判断文件是否是文本文件（非严格模式下，优先根据扩展名来判断是否为文本）
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="isStrict">是否强制根据文件内容来判断是否为文本文件</param>
        public static bool IsTextFile(string filePath, bool isStrict = false)
        {
            var result = true;

            //非严格模式下先通过扩展名来判断
            if (!isStrict && Common.Consts.TextFileExtNameList.Contains(GetExtName(filePath)))
            {
                return result;
            }

            //1、根据文件内容中是否有结束符来判断（因为字符串的结束标记为 \0）
            using (var file = new System.IO.FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var byteData = new byte[1];
                while (file.Read(byteData, 0, byteData.Length) > 0)
                {
                    if (byteData[0] == 0)
                    {
                        result = false;
                        break;
                    }
                }
            }

            //2、根据是否能正确获取到文件编码类型判断是否为文本文件
            if (!result)
            {
                var encoding = GetFileEncoding(filePath, false);
                if (null != encoding)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据路径末尾是否带有字符（\）来判断是否为文件夹路径
        /// </summary>
        public static bool IsFolderPathByCharFlag(string path)
        {
            return (path ?? "").TrimEnd().EndsWith(@"\");
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
            if (string.IsNullOrWhiteSpace(path))
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
            if (string.IsNullOrWhiteSpace(rootPath) || string.IsNullOrWhiteSpace(path))
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
        /// 取得文件扩展名（默认：不包含小圆点 + 小写）
        /// </summary>
        public static string GetExtName(string fileName, bool withDot = false, bool isConvertToLower = true)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return string.Empty;
            }
            var ext = "";
            if (withDot)
            {
                ext = System.IO.Path.GetExtension(fileName);
            }
            else
            {
                ext = System.IO.Path.GetExtension(fileName).TrimStart('.');
            }
            return isConvertToLower ? ext.ToLower() : ext;
        }

        /// <summary>
        /// 用新的扩展名更新文件路径
        /// </summary>
        /// <param name="path">原文件路径</param>
        /// <param name="newExt">扩展名</param>
        /// <returns>新的文件路径</returns>
        public static string GetFilePathWithNewExt(string path, string newExt)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            if (null != newExt)
            {
                newExt = newExt.Trim().Trim('.');
            }
            var dot = string.IsNullOrWhiteSpace(newExt) ? string.Empty : ".";
            return ChangePathByFileName(path, string.Format("{0}{1}{2}", GetFileName(path, false), dot, newExt));
        }

        /// <summary>
        /// 根据文件路径，获取文件编码（注意：需要引用 UtfUnknown 包）
        /// </summary>
        public static Encoding GetFileEncoding(string filename, bool useDefaultEncodingWhenFail = true)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            bool? hasBOM = null;

            var func = new Func<Encoding>(() =>
            {
                var defaultEncoding = useDefaultEncodingWhenFail ? System.Text.Encoding.Default : null;
                var detectorResult = UtfUnknown.CharsetDetector.DetectFromFile(filename);
                if (null == detectorResult || null == detectorResult.Detected)
                {
                    return defaultEncoding;
                }
                hasBOM = detectorResult.Detected.HasBOM;
                if (null != detectorResult.Detected.Encoding)
                {
                    return detectorResult.Detected.Encoding;
                }
                if (!string.IsNullOrWhiteSpace(detectorResult.Detected.EncodingName))
                {
                    return System.Text.Encoding.GetEncoding(detectorResult.Detected.EncodingName);
                }
                return defaultEncoding;
            });

            var resultEncoding = func();

            //如果是 utf8 且包含 BOM，则强制转换为 UTF8Encoding 构造函数的实例（因为上面检测到的 utf8 对象使用默认的 System.Text.Encoding.UTF8，而这个默认的对象是带 BOM 的）
            if (resultEncoding is System.Text.UTF8Encoding)
            {
                if (hasBOM.GetValueOrDefault())
                {
                    resultEncoding = System.Text.Encoding.UTF8;
                }
                else
                {
                    resultEncoding = new System.Text.UTF8Encoding(false);
                }
            }

            return resultEncoding;
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

        /// <summary>
        /// 给路径字符串的后面添加扩展名，如（"a/b/c","txt"）=> "a/b/c.txt"
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="ext">扩展名（会自动忽略其中的点）</param>
        /// <returns>新路径</returns>
        public static string AppendExtToPath(string path, string ext)
        {
            var extName = (ext ?? "").Trim().TrimStart('.').Trim();
            if (string.IsNullOrWhiteSpace(extName))
            {
                return path;
            }
            return (path ?? "") + "." + extName;
        }

        /// <summary>
        /// 根据文件系统属性来判断该文件是否为隐藏文件
        /// </summary>
        public static bool IsHiddenFile(string filePath)
        {
            var d = new FileInfo(filePath);
            if ((d.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden || (d.Attributes & FileAttributes.System) == FileAttributes.System)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 将指定路径转换为标准的路径，标准的路径分隔符为 \，如果是文件夹，则最后也带 \
        /// </summary>
        public static string GetStandardPath(string path, bool isFolder)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            path = path.Trim().Replace('/', '\\').TrimEnd('\\');
            if (isFolder)
            {
                return path + '\\';
            }
            return path;
        }

        /// <summary>
        /// 过滤现有的路径列表，只保留指定扩展名的路径
        /// </summary>
        /// <param name="pathList">路径列表</param>
        /// <param name="allowExtNameList">可允许的扩展名，不带点</param>
        public static List<string> FilterPathList(List<string> pathList, List<string> allowExtNameList)
        {
            var result = new List<string>();
            if (pathList.IsNullOrEmpty() || allowExtNameList.IsNullOrEmpty())
            {
                return result;
            }
            var dic = allowExtNameList.Distinct().ToDictionary(k => k.ToLower());
            return pathList.Where(k => dic.ContainsKey(GetExtName(k))).ToList();
        }

        /// <summary>
        /// 获取最终用于磁盘存储的文件名（包含扩展名）或文件夹名
        /// 这里会删除非法字符和前后空白，如果最终的名称是空的，则强制返回下划线
        /// </summary>
        public static string ConvertToFinalFileOrFolderName(string name)
        {
            name = name ?? string.Empty;
            //去掉非法字符
            name = XCLNetTools.Common.Consts.RegInvalidFileNameChars.Replace(name, string.Empty);
            //去掉前后空白（只需要 Trim 就可以了，原因是：文件夹名、有扩展名的文件名、无扩展名的文件名，系统都会自动去掉前后空白。注意：a.txt  当 a 的右侧有空格时，操作系统不会自动去空白，这种情况相当于是正常情况）
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                return "_";
            }
            return name;
        }

        /// <summary>
        /// 将文件路径转换为文件夹路径，并且将扩展名的点改为下划线
        /// </summary>
        public static string ConvertFilePathToDirPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            var fileName = XCLNetTools.FileHandler.ComFile.GetFileName(path).Replace(".", "_");
            var newPath = XCLNetTools.FileHandler.ComFile.ChangePathByFileName(path, fileName);
            return XCLNetTools.FileHandler.ComFile.GetStandardPath(newPath, true);
        }

        /// <summary>
        /// 检查已有路径是否已经存在，如果已经存在，则在名称末尾自增加 1，如：test(1).txt, test(2).txt...
        /// </summary>
        public static string GetUniquePathToSave(string path, bool isFolder)
        {
            var newPath = path;
            while (IsPathExists(newPath, isFolder))
            {
                newPath = PathIncrement(newPath, isFolder);
            }
            return XCLNetTools.FileHandler.ComFile.GetStandardPath(newPath, isFolder);
        }

        /// <summary>
        /// 检查路径是否存在
        /// </summary>
        public static bool IsPathExists(string path, bool isFolder)
        {
            if (isFolder)
            {
                return System.IO.Directory.Exists(path);
            }
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 在路径的文件名上自增加 1，如：c:\a\b\test.txt  -> c:\a\b\test(1).txt
        /// </summary>
        public static string PathIncrement(string path, bool isFolder)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return string.Empty;
            }
            var indexReg = new Regex(@"\((\d+)\)$");
            var getCurrentIdx = new Func<string, int>((m) =>
             {
                 var mt = indexReg.Match(m);
                 var idx = mt?.Groups?.Count >= 2 ? Convert.ToInt32(mt.Groups[1].Value) : 0;
                 return idx;
             });
            var newPath = "";
            if (isFolder)
            {
                var name = XCLNetTools.FileHandler.ComFile.GetPathFolderName(path, true);
                var nameWithoutIndex = indexReg.Replace(name, "");
                var idx = getCurrentIdx(name) + 1;
                var newName = $"{nameWithoutIndex}({idx})";
                newPath = XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(path, newName, true);
            }
            else
            {
                var name = XCLNetTools.FileHandler.ComFile.GetFileName(path, false);
                var nameWithoutIndex = indexReg.Replace(name, "");
                var idx = getCurrentIdx(name) + 1;
                var newName = $"{nameWithoutIndex}({idx})";
                newPath = XCLNetTools.FileHandler.ComFile.ChangePathByFileNameWithoutExt(path, newName);
            }
            return XCLNetTools.FileHandler.ComFile.GetStandardPath(newPath, isFolder);
        }

        /// <summary>
        /// 获取一个路径所在的文件夹路径。如：d:\a\test.doc -> d:\a\ ，d:\a\b\  ->  d:\a\
        /// </summary>
        public static string GetParentDirPath(string path, bool isFolder)
        {
            var parentPath = string.Empty;
            if (isFolder)
            {
                parentPath = XCLNetTools.FileHandler.FileDirectory.GetDirParentPath(path);
            }
            else
            {
                parentPath = XCLNetTools.FileHandler.ComFile.GetFileFolderPath(path);
            }
            return XCLNetTools.FileHandler.ComFile.GetStandardPath(parentPath, true);
        }

        /// <summary>
        /// 获取文件的文件名或文件夹的文件夹名。如：d:\a\test.doc -> test.doc ，d:\a\b\  ->  b
        /// </summary>
        public static string GetFileNameOrFolderName(string path, bool isFolder)
        {
            return isFolder ? XCLNetTools.FileHandler.ComFile.GetPathFolderName(path, true) : XCLNetTools.FileHandler.ComFile.GetFileName(path);
        }

        /// <summary>
        /// 给文件名或文件夹名称添加前缀或后缀
        /// </summary>
        public static string AppendPrefixAndSuffixNameToPath(string path, bool isFolder, string prefix, string suffix)
        {
            if (string.IsNullOrWhiteSpace(prefix) && string.IsNullOrWhiteSpace(suffix))
            {
                return path;
            }

            var newName = $"{prefix}{ComFile.GetFileNameOrFolderName(path, isFolder)}";
            if (isFolder)
            {
                newName += suffix;
            }
            else
            {
                newName = AppendSuffixNameToFileName(newName, suffix);
            }

            if (isFolder)
            {
                return XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(path, newName, true);
            }
            else
            {
                return XCLNetTools.FileHandler.ComFile.ChangePathByFileName(path, newName);
            }
        }

        /// <summary>
        /// 在文件名的最右侧（不包含扩展名）添加后缀。如：abc.docx -> abcXXX.docx
        /// </summary>
        public static string AppendSuffixNameToFileName(string fileName, string suffix)
        {
            if (string.IsNullOrWhiteSpace(suffix))
            {
                return fileName;
            }
            fileName = fileName ?? string.Empty;
            var dotIndex = fileName.LastIndexOf('.');
            if (dotIndex < 0)
            {
                return $"{fileName}{suffix}";
            }
            return fileName.Insert(dotIndex, suffix);
        }

        #endregion 其它
    }
}