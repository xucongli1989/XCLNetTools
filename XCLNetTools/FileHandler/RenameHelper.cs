using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using XCLNetTools.Entity;
using XCLNetTools.Generic;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 文件和文件夹重命名帮助类
    /// </summary>
    public static class RenameHelper
    {
        /// <summary>
        /// PC 对象
        /// </summary>
        public static readonly Microsoft.VisualBasic.Devices.Computer PC = new Microsoft.VisualBasic.Devices.Computer();

        /// <summary>
        /// 临时文件名中的分隔标识符
        /// </summary>
        private const string splitChar = "@";

        /// <summary>
        /// 重命名的临时固定前缀名称
        /// </summary>
        public static string RenameTempFixedPreName { get; set; }

        /// <summary>
        /// 重命名文件或文件夹（先按正常逻辑重命名，如果失败，则使用随机前缀重命名并返回该随机前缀和路径，方便后续移除此前缀，注意：此方法不包含移除随机前缀的逻辑，由前端逻辑处理）
        /// </summary>
        /// <param name="isFolder">是否为文件夹</param>
        /// <param name="path">路径</param>
        /// <param name="newName">此文件或文件夹的新名称</param>
        /// <param name="fileId">关联的数据库记录标识</param>
        public static XCLNetTools.Entity.MethodResult<RenameConflictItem> RenameFileOrFolder(bool isFolder, string path, string newName, int fileId)
        {
            var msg = new XCLNetTools.Entity.MethodResult<RenameConflictItem>();
            msg.Result = null;
            msg.IsSuccess = true;
            if (string.IsNullOrWhiteSpace(path))
            {
                return msg;
            }

            try
            {
                RenameHelper.Rename(isFolder, path, newName);
            }
            catch (Exception ex)
            {
                //如果正常重命名报错，则采用随机前缀再次重命名，如：y50avhyl.eex@newName.docx
                var tempName = System.IO.Path.GetRandomFileName().Replace("@", "") + splitChar + newName;
                var tempPath = isFolder ? XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(path, tempName, true) : XCLNetTools.FileHandler.ComFile.ChangePathByFileName(path, tempName);
                try
                {
                    msg.Result = new RenameConflictItem();
                    msg.Result.FileID = fileId;
                    msg.Result.RandomName = tempName;
                    msg.Result.Path = tempPath;
                    msg.Result.RealName = newName;
                    if (isFolder)
                    {
                        PC.FileSystem.RenameDirectory(path, tempName);
                        msg.Result.RealPath = XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(path, newName, true);
                    }
                    else
                    {
                        PC.FileSystem.RenameFile(path, tempName);
                        msg.Result.RealPath = XCLNetTools.FileHandler.ComFile.ChangePathByFileName(path, newName);
                    }
                }
                catch
                {
                    //抛出最开始的异常信息
                    throw ex;
                }
            }
            return msg;
        }

        /// <summary>
        /// 将所有路径转换成唯一的路径，如果有同名路径，则在后面自动标号（如：a.txt => a(1).txt、d:\a\ => d:\a(1)\）
        /// 1、返回的列表与源列表的顺序是一样的
        /// 2、传入的路径必须是标准的路径，并且这些路径可能同时包含文件路径与文件夹路径
        /// </summary>
        public static List<string> ConvertPathToUniqueList(List<string> standardPathList)
        {
            if (standardPathList.IsNullOrEmpty())
            {
                return new List<string>();
            }
            var hs = new HashSet<string>();
            var tpLst = new List<Tuple<string, string>>();
            var reg = new Regex(@"\((?<idx>\d+)\)$");

            standardPathList.ForEach(p =>
            {
                if (string.IsNullOrWhiteSpace(p))
                {
                    tpLst.Add(new Tuple<string, string>(p, p));
                    return;
                }

                var isFolder = ComFile.IsFolderPathByCharFlag(p);
                var tempPath = p;

                //无限循环计算唯一的路径
                while (true)
                {
                    var key = tempPath.ToUpper().TrimEnd('\\');

                    //如果当前路径在当前时刻未重复，则直接返回结果
                    if (!hs.Contains(key))
                    {
                        hs.Add(key);
                        break;
                    }

                    //如果当前不唯一，则更新名称后再判断是否唯一
                    if (isFolder)
                    {
                        var name = ComFile.GetPathFolderName(tempPath, true);
                        var newName = string.Empty;
                        var mt = reg.Match(name);
                        if (mt.Success && mt?.Groups?.Count > 0)
                        {
                            newName = reg.Replace(name, $"({Convert.ToInt32(mt.Groups["idx"].Value) + 1})");
                        }
                        else
                        {
                            newName = $"{name}(1)";
                        }
                        tempPath = ComFile.ChangePathByFolderName(tempPath, newName, true);
                    }
                    else
                    {
                        var name = ComFile.GetFileName(tempPath, false);
                        var ext = ComFile.GetExtName(tempPath);
                        var newName = string.Empty;
                        var mt = reg.Match(name);
                        if (mt.Success && mt?.Groups?.Count > 0)
                        {
                            newName = reg.Replace(name, $"({Convert.ToInt32(mt.Groups["idx"].Value) + 1})");
                        }
                        else
                        {
                            newName = $"{name}(1)";
                        }
                        tempPath = ComFile.ChangePathByFileName(tempPath, ComFile.AppendExtToPath(newName, ext));
                    }
                }

                tpLst.Add(new Tuple<string, string>(p, tempPath));
            });

            return tpLst.Select(k => k.Item2).ToList();
        }

        /// <summary>
        /// 重命名文件或文件夹，如果重命名失败，则会报错（注意：当新名称和旧名称不区分大小写相同时，使用临时随机命名过渡处理，从而不会报错。Computer 中的内置重命名在这种情况下会报错。）
        /// </summary>
        /// <param name="isFolder">是否为文件夹</param>
        /// <param name="path">文件或文件夹路径</param>
        /// <param name="newName">新名称</param>
        public static void Rename(bool isFolder, string path, string newName)
        {
            newName = newName.Trim();
            var tempName = $"{RenameHelper.RenameTempFixedPreName}{System.IO.Path.GetRandomFileName()}";
            if (isFolder)
            {
                var oldName = XCLNetTools.FileHandler.ComFile.GetPathFolderName(path, true);
                if (oldName == newName)
                {
                    return;
                }
                if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
                {
                    PC.FileSystem.RenameDirectory(path, tempName);
                    var tempPath = XCLNetTools.FileHandler.ComFile.ChangePathByFolderName(path, tempName, true);
                    PC.FileSystem.RenameDirectory(tempPath, newName);
                }
                else
                {
                    PC.FileSystem.RenameDirectory(path, newName);
                }
            }
            else
            {
                var oldName = XCLNetTools.FileHandler.ComFile.GetFileName(path);
                if (oldName == newName)
                {
                    return;
                }
                if (string.Equals(oldName, newName, StringComparison.OrdinalIgnoreCase))
                {
                    PC.FileSystem.RenameFile(path, tempName);
                    var tempPath = XCLNetTools.FileHandler.ComFile.ChangePathByFileName(path, tempName);
                    PC.FileSystem.RenameFile(tempPath, newName);
                }
                else
                {
                    PC.FileSystem.RenameFile(path, newName);
                }
            }
        }

        /// <summary>
        /// 将临时路径重命名为磁盘上的正式且唯一的路径，如果成功，则返回处理后的正式路径
        /// </summary>
        public static string RenameTempPathToDiskUniqueExpectedPath(string tempPath, string expectedPath, bool isFolder)
        {
            tempPath = XCLNetTools.FileHandler.ComFile.GetStandardPath(tempPath, isFolder);
            expectedPath = XCLNetTools.FileHandler.ComFile.GetStandardPath(expectedPath, isFolder);

            //临时路径和预期路径必须位于【同层级】位置
            var tempPathDir = XCLNetTools.FileHandler.ComFile.GetParentDirPath(tempPath, isFolder);
            var expectedPathDir = XCLNetTools.FileHandler.ComFile.GetParentDirPath(expectedPath, isFolder);
            if (!string.Equals(tempPathDir, expectedPathDir, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("TempPath and ExpectedPath must be in a same folder!");
            }

            //重命名
            while (true)
            {
                try
                {
                    var logicPath = XCLNetTools.FileHandler.ComFile.GetUniquePathToSave(expectedPath, isFolder);
                    var diskName = XCLNetTools.FileHandler.ComFile.GetFileNameOrFolderName(logicPath, isFolder);
                    XCLNetTools.FileHandler.RenameHelper.Rename(isFolder, tempPath, diskName);
                    return logicPath;
                }
                catch (IOException)
                {
                    //同名被占用异常
                }
            }
        }
    }
}