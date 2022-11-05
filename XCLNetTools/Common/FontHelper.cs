using System.Collections.Generic;
using XCLNetTools.Entity;
using System.Linq;
using System;
using System.IO;
using System.Collections.Concurrent;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 字体帮助类
    /// </summary>
    public static class FontHelper
    {
        /// <summary>
        /// 获取当前所有字体信息（默认包含两个文件夹：C:\Windows\Fonts  和  C:\Users\XXX\AppData\Local\Microsoft\Windows\Fonts\）
        /// </summary>
        public static List<FontInfoEntity> GetAllFontInfoList()
        {
            var lst = new ConcurrentBag<FontInfoEntity>();
            var pathList = new List<string>();
            pathList.Add(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            pathList.Add(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Fonts"));

            pathList.ForEach(p =>
            {
                XCLNetTools.FileHandler.ComFile.GetFolderFilesByRecursion(p).AsParallel().ForAll(fontPath =>
                {
                    try
                    {
                        var ttf = new System.Windows.Media.GlyphTypeface(new Uri(fontPath));
                        var cnFontName = ttf.Win32FamilyNames[new System.Globalization.CultureInfo("zh-CN")];
                        var enFontName = ttf.Win32FamilyNames[new System.Globalization.CultureInfo("en-US")];
                        if (string.IsNullOrWhiteSpace(cnFontName) && string.IsNullOrWhiteSpace(enFontName))
                        {
                            return;
                        }
                        var info = new FontInfoEntity();
                        info.SourcePath = p;
                        info.Path = fontPath;
                        info.FileNameWithoutExt = XCLNetTools.FileHandler.ComFile.GetFileName(fontPath, false);
                        info.CNName = cnFontName;
                        info.ENName = enFontName;
                        info.FontName = string.IsNullOrWhiteSpace(cnFontName) ? enFontName : cnFontName;
                        info.FontValue = enFontName;
                        lst.Add(info);
                    }
                    catch
                    {
                        //
                    }
                });
            });

            //排序：包含中文排前面
            return lst.OrderByDescending(k => XCLNetTools.StringHander.DataCheck.IsHasCHZN(k.FontName)).ThenBy(k => k.FontName).ToList();
        }
    }
}