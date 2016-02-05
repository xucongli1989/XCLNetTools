/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.1
更新时间：2016-01-01

四：更新内容：
1：将原先基础数据转换方法转移到Common/DataTypeConvert下面
2：其它逻辑优化，如表单参数获取等
3：首次开放所有源代码
 */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// 浏览器书签文件操作类
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// 根据浏览器书签文件地址，返回list
        /// </summary>
        /// <param name="path">书签文件地址</param>
        /// <returns>书签list</returns>
        public static List<XCLNetTools.Entity.BookmarkEntity> GetBookmark(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            path = ComFile.MapPath(path.Trim());
            Regex reg = new Regex(@"(<dt>)|(<p>)|(\n)|(\r)", RegexOptions.IgnoreCase);
            string strFile = System.IO.File.ReadAllText(path, System.Text.Encoding.UTF8);
            strFile = reg.Replace(strFile, "");
            strFile = new Regex(@">\s+<").Replace(strFile, "><");

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(strFile);

            int idx = 0;
            XCLNetTools.Entity.BookmarkEntity model = null;
            List<XCLNetTools.Entity.BookmarkEntity> lst = new List<XCLNetTools.Entity.BookmarkEntity>();

            Action<HtmlAgilityPack.HtmlNode, int> GetNodeItems = null;
            GetNodeItems = (HtmlAgilityPack.HtmlNode rootNode, int parentId) =>
            {
                //收藏夹
                HtmlAgilityPack.HtmlNodeCollection folderNodeList = rootNode.SelectNodes("h3");
                if (null != folderNodeList && folderNodeList.Count > 0)
                {
                    foreach (var m in folderNodeList)
                    {
                        model = new XCLNetTools.Entity.BookmarkEntity();
                        model.IsFolder = true;
                        model.Id = (++idx);
                        model.ParentId = parentId;
                        model.Name = m.InnerText;
                        lst.Add(model);
                        //m的子项
                        var nextNode = m.NextSibling;
                        if (null != nextNode && string.Equals(nextNode.Name, "dl", StringComparison.CurrentCultureIgnoreCase))
                        {
                            GetNodeItems(nextNode, model.Id);
                        }
                    }
                }

                //收藏项
                HtmlAgilityPack.HtmlNodeCollection nodeList = rootNode.SelectNodes("a");
                if (null != nodeList && nodeList.Count > 0)
                {
                    foreach (var m in nodeList)
                    {
                        model = new XCLNetTools.Entity.BookmarkEntity();
                        model.Id = (++idx);
                        model.IcoURL = null == m.Attributes["ICON"] ? "" : m.Attributes["ICON"].Value;
                        model.IsFolder = false;
                        model.ParentId = parentId;
                        model.Name = m.InnerText;
                        model.Url = null == m.Attributes["HREF"] ? "" : m.Attributes["HREF"].Value;
                        lst.Add(model);
                    }
                }
            };

            GetNodeItems(doc.DocumentNode.ChildNodes["dl"], idx);

            return lst;
        }
    }
}