/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-02

四：更新内容：
1：更新表单获取的参数类型
2：更改Message/JsonMsg类的目录
3：删除多余的方法
4：修复一处未dispose问题
5：整理部分代码
6：添加 MethodResult.cs
7：获取枚举list时可以使用byte/short等
 */


using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    ///文件操作公共类
    /// </summary>
    public class ComFile
    {
        #region 删除文件

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>若为true，则删除成功</returns>
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
        /// <param name="srcPath">源文件</param>
        /// <param name="dstPath">目标文件</param>
        /// <returns>复制成功返回TRUE,复制失败返回FALSE.</returns>
        public static bool CopyFile(string srcPath, string dstPath)
        {
            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(GetFileFolderPath(dstPath));
            return CopyFile(srcPath, dstPath, true);
        }

        /// <summary>
        /// 复制文件，若目标目录不存在，则自动创建
        /// </summary>
        /// <param name="srcPath">源文件</param>
        /// <param name="dstPath">目标文件</param>
        /// <param name="overwrite">是否覆盖目标文件</param>
        /// <returns>复制成功返回TRUE,复制失败返回FALSE</returns>
        public static bool CopyFile(string srcPath, string dstPath, bool overwrite)
        {
            XCLNetTools.FileHandler.FileDirectory.MakeDirectory(GetFileFolderPath(dstPath));
            File.Copy(ComFile.MapPath(srcPath), ComFile.MapPath(dstPath), overwrite);
            return File.Exists(ComFile.MapPath(dstPath));
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
            return System.IO.Directory.GetFiles(ComFile.MapPath(path));
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
            string[] subPaths = System.IO.Directory.GetDirectories(rootPath);
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
        /// <param name="directoryPath">存放文件的物理路径。</param>
        /// <returns>返回存放文件的目录。</returns>
        public static string GetSaveDirectory(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            return directoryPath;
        }

        /// <summary>
        /// 获取文件所在的文件夹【不带'\'】
        /// </summary>
        public static string GetFileFolderPath(string filePath)
        {
            return filePath.Substring(0, filePath.LastIndexOf('\\') + 1).TrimEnd('\\');
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
        /// 获取文件名
        /// </summary>
        /// <param name="fileName">路径（相对或绝对均可）</param>
        /// <param name="isWithExt">是否包含扩展名</param>
        /// <returns>文件名</returns>
        public static string GetFileName(string fileName, bool isWithExt = true)
        {
            var info = new System.IO.FileInfo(fileName);
            if (isWithExt || string.IsNullOrEmpty(info.Name) || string.IsNullOrEmpty(info.Extension))
            {
                return info.Name;
            }
            else
            {
                return XCLNetTools.StringHander.Common.TrimEnd(info.Name, info.Extension);
            }
        }

        /// <summary>
        ///给上传的文件随机命名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>新的文件名</returns>
        public static string GetRandomFileName(string filename)
        {
            int i;
            string[] files = filename.Split('.');
            string exfilename = '.' + files.GetValue(files.Length - 1).ToString();
            char[] s = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

            string num = "";
            Random r = new Random();
            for (i = 0; i <= 9; i++)
                num = num + s[r.Next(0, s.Length)].ToString();

            DateTime time = DateTime.Now;
            string name = time.Year.ToString() + time.Month.ToString().PadLeft(2, '0') + time.Day.ToString().PadLeft(2, '0') + time.Hour.ToString().PadLeft(2, '0') + time.Minute.ToString().PadLeft(2, '0') + time.Second.ToString().PadLeft(2, '0') + num + exfilename;
            return name;
        }

        /// <summary>
        /// 取得文件扩展名(不包含小圆点)【小写】
        /// </summary>
        /// <param name="fileName">文件完整路径或文件名</param>
        /// <returns>文件扩展名(不包含小圆点)</returns>
        public static string GetExtName(string fileName)
        {
            return (new FileInfo(fileName).Extension ?? "").Trim('.');
        }

        #endregion 其它
    }
}