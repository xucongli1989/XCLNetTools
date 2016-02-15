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


using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 文件目录操作类
    /// </summary>
    public class FileDirectory
    {
        #region 目录操作

        /// <summary>
        /// 检测目录是否为空目录（既没有文件夹，也没有文件）
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>true:空目录，false:非空目录</returns>
        public static bool IsEmpty(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return true;
            }
            var files = Directory.GetFiles(path);
            var dirs = Directory.GetDirectories(path);
            return null == files && files.Length == 0 && null == dirs && dirs.Length == 0;
        }

        /// <summary>
        /// 判断目录是否存在
        /// </summary>
        /// <param name="directoryName">目录路径</param>
        /// <returns>true：存在，false：不存在</returns>
        public static bool DirectoryExists(string directoryName)
        {
            return Directory.Exists(directoryName);
        }

        /// <summary>
        /// 建立目录
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <returns>返回boolean,true:目录建立成功, false:目录建立失败</returns>
        public static bool MakeDirectory(string directoryName)
        {
            try
            {
                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除指定的目录
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <returns>true：删除成功，false：删除失败</returns>
        public static bool RMDIR(string directoryName)
        {
            DirectoryInfo di = new DirectoryInfo(directoryName);
            if (di.Exists == false)
            {
                return false;
            }
            else
            {
                di.Delete(true);
                return true;
            }
        }

        /// <summary>
        /// 删除目录并删除其下的子目录及其文件
        /// </summary>
        /// <param name="directoryName">目录名</param>
        /// <returns>true:删除成功,false:删除失败</returns>
        public static bool DelTree(string directoryName)
        {
            if (DirectoryExists(directoryName))
            {
                Directory.Delete(directoryName, true);
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
        /// <param name="rootPath">要清空的目录</param>
        /// <returns>是否操作成功</returns>
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
        /// 获取指定目录下的所有文件及文件夹信息
        /// </summary>
        /// <param name="dirPath">要获取信息的目录路径</param>
        /// <param name="rootPath">根路径（设置该值后，返回的信息实体中将包含相对于该根路径的相对路径信息）</param>
        /// <returns>文件信息list</returns>
        public static List<XCLNetTools.Entity.FileInfoEntity> GetFileList(string dirPath, string rootPath = "")
        {
            if (!string.IsNullOrEmpty(dirPath))
            {
                dirPath = XCLNetTools.FileHandler.ComFile.MapPath(dirPath);
            }
            if (!string.IsNullOrEmpty(rootPath))
            {
                rootPath = XCLNetTools.FileHandler.ComFile.MapPath(rootPath);
            }

            if (string.IsNullOrEmpty(dirPath) || FileDirectory.IsEmpty(dirPath))
            {
                return null;
            }
            int idx = 1;

            List<XCLNetTools.Entity.FileInfoEntity> result = new List<Entity.FileInfoEntity>();

            //文件夹
            var directories = System.IO.Directory.EnumerateDirectories(dirPath);
            if (null != directories && directories.Count() > 0)
            {
                directories.ToList().ForEach(k =>
                {
                    var dir = new System.IO.DirectoryInfo(k);
                    if (null != dir)
                    {
                        result.Add(new XCLNetTools.Entity.FileInfoEntity()
                        {
                            ID = idx++,
                            Name = dir.Name,
                            IsFolder = true,
                            Path = k,
                            RootPath = rootPath,
                            RelativePath = ComFile.GetUrlRelativePath(rootPath, k),
                            ModifyTime = dir.LastWriteTime,
                            CreateTime = dir.CreationTime
                        });
                    }
                });
            }

            //文件
            string[] files = XCLNetTools.FileHandler.ComFile.GetFolderFiles(dirPath);
            if (null != files && files.Length > 0)
            {
                files.ToList().ForEach(k =>
                {
                    var file = new System.IO.FileInfo(k);
                    if (null != file)
                    {
                        result.Add(new XCLNetTools.Entity.FileInfoEntity()
                        {
                            ID = idx++,
                            Name = file.Name,
                            IsFolder = false,
                            Path = k,
                            RootPath = rootPath,
                            RelativePath = ComFile.GetUrlRelativePath(rootPath, k),
                            ModifyTime = file.LastWriteTime,
                            CreateTime = file.CreationTime,
                            Size = file.Length,
                            ExtName = (file.Extension ?? "").Trim('.')
                        });
                    }
                });
            }

            return result;
        }

        #endregion 目录操作

        #region 文件操作

        /// <summary>
        /// 建立一个文件
        /// </summary>
        /// <param name="filePathName">目录名</param>
        /// <returns>true:建立成功,false:建立失败</returns>
        public static bool CreateTextFile(string filePathName)
        {
            FileInfo info = new FileInfo(filePathName);
            if (info.Exists)
            {
                return true;
            }
            using (var fs = info.Create())
            {
                if (null != fs)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 在文件里追加内容
        /// </summary>
        /// <param name="filePathName">文件名</param>
        /// <param name="writeWord">追加内容</param>
        public static void AppendText(string filePathName, string writeWord)
        {
            AppendText(filePathName, writeWord, System.Text.Encoding.Default);
        }

        /// <summary>
        /// 在文件里追加内容
        /// </summary>
        /// <param name="filePathName">文件名</param>
        /// <param name="writeWord">追加内容</param>
        /// <param name="encode">编码</param>
        public static void AppendText(string filePathName, string writeWord, System.Text.Encoding encode)
        {
            //建立文件
            CreateTextFile(filePathName);
            //得到原来文件的内容
            using (FileStream fileRead = new FileStream(filePathName, FileMode.Open, FileAccess.ReadWrite))
            using (StreamReader fileReadWord = new StreamReader(fileRead, encode))
            using (StreamWriter fileWrite = new StreamWriter(fileRead, encode))
            {
                string oldString = fileReadWord.ReadToEnd().ToString();
                oldString = oldString + writeWord;
                fileWrite.Write(writeWord);
            }
        }

        /// <summary>
        /// 读取文件里内容
        /// </summary>
        /// <param name="filePathName">文件名</param>
        /// <returns>文件内容</returns>
        public static string ReadFileData(string filePathName)
        {
            string str = "";
            using (FileStream fileRead = new FileStream(filePathName, FileMode.Open, FileAccess.Read))
            using (StreamReader fileReadWord = new StreamReader(fileRead, System.Text.Encoding.Default))
            {
                str = fileReadWord.ReadToEnd().ToString();
            }
            return str;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="absoluteFilePath">文件绝对地址</param>
        /// <returns>true:删除文件成功,false:删除文件失败</returns>
        public static bool FileDelete(string absoluteFilePath)
        {
            try
            {
                FileInfo objFile = new FileInfo(absoluteFilePath);
                if (objFile.Exists)//如果存在
                {
                    //删除文件.
                    objFile.Delete();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        #endregion 文件操作
    }
}