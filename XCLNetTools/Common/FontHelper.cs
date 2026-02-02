using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using XCLNetTools.Entity;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 字体帮助类
    /// </summary>
    public static class FontHelper
    {
        /// <summary>
        /// 字体文件所在的文件夹路径列表（默认包含两个文件夹：C:\Windows\Fonts  和  C:\Users\XXX\AppData\Local\Microsoft\Windows\Fonts\）
        /// </summary>
        public static readonly List<string> FontSourcePathList = new List<string>() {
            Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Microsoft\Windows\Fonts")
        };

        /// <summary>
        /// 根据字体文件夹中的所有字体文件来获取当前所有字体的详细信息
        /// 注意：这里的字体不一定能够通过此方式转换为 .net 对象：new System.Drawing.Font(FontValue, 1)
        /// </summary>
        public static List<FontInfoEntity> GetAllFontInfoListFromFontFiles()
        {
            var lst = new ConcurrentBag<FontInfoEntity>();

            FontHelper.FontSourcePathList.ForEach(p =>
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
                        info.Path = fontPath;
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

        /// <summary>
        /// 根据系统中已安装字体（System.Drawing.Text.InstalledFontCollection）来获取所有字体信息（这里的字体可以直接使用此方法转换为字体对象： new System.Drawing.Font(FontValue,1)）。
        /// 注意：这里没有字体文件的路径信息
        /// </summary>
        public static List<FontInfoEntity> GetAllFontInfoListFromInstalled()
        {
            var lst = new List<FontInfoEntity>();
            var fonts = new System.Drawing.Text.InstalledFontCollection();
            fonts.Families.ToList().ForEach(k =>
            {
                var cnFontName = k.GetName(2052);
                var enFontName = k.GetName(1033);

                var info = new FontInfoEntity();
                info.CNName = cnFontName;
                info.ENName = enFontName;
                info.FontName = string.IsNullOrWhiteSpace(cnFontName) ? enFontName : cnFontName;
                info.FontValue = enFontName;
                lst.Add(info);
            });

            //排序：包含中文排前面
            return lst.OrderByDescending(k => XCLNetTools.StringHander.DataCheck.IsHasCHZN(k.FontName)).ThenBy(k => k.FontName).ToList();
        }
    }
}