/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using XCLNetTools.Entity;

namespace XCLNetTools.StringHander
{
    /// <summary>
    ///公用类
    /// </summary>
    public class Common
    {
        #region 防止代码注入

        /// <summary>
        /// 防止HTML代码注入
        /// 替换尖括号为html实体
        /// </summary>
        /// <param name="str">待处理的数据</param>
        /// <returns>处理后的数据</returns>
        public static string ExchangeNote(string str)
        {
            return (str ?? "").Replace("<", "&lt").Replace(">", "&gt");
        }

        /// <summary>
        /// 防止SQL注入
        /// </summary>
        /// <param name="inputStr">输入的sql语句</param>
        /// <returns>过滤后的语句</returns>
        public static string No_SqlHack(string inputStr)
        {
            //要过滤的关键字集合
            string sqlHackGet = inputStr;
            string[] AllStr = "|;|and|chr(|exec|insert|select|delete|from|update|mid(|master.|".Split('|');

            //分离关键字
            string[] GetStr = sqlHackGet.Split(' ');
            if (sqlHackGet != "")
            {
                for (int j = 0; j < GetStr.Length; j++)
                {
                    for (int i = 0; i < AllStr.Length; i++)
                    {
                        if (GetStr[j].ToLower() == AllStr[i].ToLower())
                        {
                            GetStr[j] = "";
                            break;
                        }
                    }
                }
                sqlHackGet = "";
                for (int i = 0; i < GetStr.Length; i++)
                {
                    sqlHackGet += GetStr[i].ToString() + " ";
                }
                return sqlHackGet.TrimEnd(' ').Replace("'", "_").Replace(",", "_");
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 过滤HTML 和SQL
        /// </summary>
        /// <param name="str">待处理的数据</param>
        /// <returns>处理后的数据</returns>
        public static string NoSqlAndHtml(string str)
        {
            string s = string.Empty;
            if (!string.IsNullOrEmpty(str))
            {
                s = NoHTML(No_SqlHack(str));
            }
            return s;
        }

        /// <summary>
        /// 删除所有HTML标记
        /// </summary>
        /// <param name="html">待处理的数据</param>
        /// <returns>处理后的数据</returns>
        public static string NoHTML(string html)
        {
            if (string.IsNullOrEmpty(html))
            {
                return "";
            }
            //删除脚本
            html = Regex.Replace(html, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            html = Regex.Replace(html, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"-->", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"<!--.*", "", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            html = Regex.Replace(html, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            html = html.Replace("\r\n", "");
            html = html.Replace("'", "");
            html = HttpContext.Current.Server.HtmlEncode(html).Trim();
            return html;
        }

        #endregion 防止代码注入

        #region 百分比相关

        /// <summary>
        /// 返回百分制
        /// </summary>
        /// <param name="m">要转换的值</param>
        /// <param name="count">保留几位小数</param>
        /// <returns>百分值</returns>
        public static decimal GetPercent(decimal? m, int count)
        {
            return Math.Round(XCLNetTools.Common.DataTypeConvert.ToDecimal(m) * 100, count);
        }

        #endregion 百分比相关

        #region KB,MB,GB,TB相关

        /// <summary>
        /// 返回文件大小KB,MB,GB,TB,PB形式的表示
        /// </summary>
        /// <param name="size">kb</param>
        /// <param name="count">保留几位小数</param>
        /// <returns>如：5.5GB</returns>
        public static string GetSizeStringByKB(decimal size, int count)
        {
            string flag = "KB";
            decimal result = 0;
            if (size >= 1024 * 1024 * 1024 * 1024.0m)
            {
                result = Math.Round(size / (1024 * 1024 * 1024 * 1024.0m), count);
                flag = "PB";
            }
            else if (size >= 1024 * 1024 * 1024)
            {
                result = Math.Round(size / (1024 * 1024 * 1024.0m), count);
                flag = "TB";
            }
            else if (size >= 1024 * 1024)
            {
                result = Math.Round(size / (1024 * 1024.0m), count);
                flag = "GB";
            }
            else if (size >= 1024)
            {
                result = Math.Round(size / (1024.0m), count);
                flag = "MB";
            }
            else
            {
                result = Math.Round(size, count);
                flag = "KB";
            }
            return result.ToString() + flag;
        }

        #endregion KB,MB,GB,TB相关

        #region 输出相关

        /// <summary>
        /// 输出
        /// </summary>
        /// <param name="str">待输出的内容</param>
        public static void ResponseClearWrite(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(str);
                HttpContext.Current.Response.End();
            }
        }

        #endregion 输出相关

        #region 静态资源相关

        /// <summary>
        /// 从xml中加载静态资源配置信息
        /// </summary>
        /// <param name="xmlPath">xml路径</param>
        /// <returns>配置对象</returns>
        public static XCLNetTools.Entity.StaticResourceConfig GetStaticResourceConfig(string xmlPath)
        {
            XCLNetTools.Entity.StaticResourceConfig model = null;
            if (!string.IsNullOrEmpty(xmlPath))
            {
                model = XCLNetTools.Serialize.XML.DeserializeFromXMLFile<XCLNetTools.Entity.StaticResourceConfig>(xmlPath);
            }
            return model;
        }

        /// <summary>
        /// 获取静态资源文件引用
        /// </summary>
        /// <param name="config">配置</param>
        /// <param name="nameList">指定输出项，若为null，则输出所有</param>
        /// <returns>引用的地址</returns>
        public static string GetStaticResourceUrl(XCLNetTools.Entity.StaticResourceConfig config, List<string> nameList = null)
        {
            StringBuilder str = new StringBuilder();
            List<XCLNetTools.Entity.StaticResource> modelList = null;
            if (null == nameList || nameList.Count == 0)
            {
                modelList = config.StaticResourceList;
            }
            else
            {
                var noExist = nameList.Where(k => !config.StaticResourceList.Exists(m => string.Equals(k.Trim(), m.Name.Trim(), StringComparison.OrdinalIgnoreCase))).ToList();
                if (null != noExist && noExist.Count > 0)
                {
                    throw new Exception(string.Format("静态资源（{0}）在配置文件中未找到！", string.Join("，", noExist.ToArray())));
                }
                //要按nameList排序
                modelList = new List<StaticResource>();
                for (int i = 0; i < nameList.Count; i++)
                {
                    var model = config.StaticResourceList.Where(k => string.Equals(nameList[i].Trim(), k.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    if (null != model)
                    {
                        modelList.Add(model);
                    }
                }
            }
            if (null != modelList && modelList.Count > 0)
            {
                modelList.ForEach(model =>
                {
                    str.Append(model.ToString());
                });
            }
            return str.ToString();
        }

        #endregion 静态资源相关

        #region 字符串截取

        /// <summary>
        /// 截取指定长度的字符串，一个汉字算两个字符
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="length">长度</param>
        /// <param name="s">需要替代的字符串</param>
        /// <returns>字符串</returns>
        public static string GetSubString(string str, int length, string s)
        {
            string temp = NoHTML(str);
            if (!String.IsNullOrEmpty(temp))
            {
                int j = 0;
                int k = 0;
                ASCIIEncoding ascii = new ASCIIEncoding();
                for (int i = 0; i < temp.Length; i++)
                {
                    //if (ascii.GetByteCount(temp.Substring(i, 1))==2)
                    if (Regex.IsMatch(temp.Substring(i, 1), @"[^\x00-\xff]+"))
                    {
                        j += 2;
                    }
                    else
                    {
                        j += 1;
                    }
                    if (j <= length)
                    {
                        k += 1;
                    }
                    if (j >= length)
                    {
                        return temp.Substring(0, k) + s;
                    }
                }
            }
            return temp;
        }

        #endregion 字符串截取

        #region 全角半角处理

        /// <summary>
        /// 半角转全角
        /// </summary>
        /// <param name="BJstr">半角</param>
        /// <returns>全角</returns>
        public static string GetQuanJiao(string BJstr)
        {
            #region
            char[] c = BJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 0)
                    {
                        b[0] = (byte)(b[0] - 32);
                        b[1] = 255;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }

            string strNew = new string(c);
            return strNew;

            #endregion 全角半角处理
        }

        /// <summary>
        /// 全角转半角
        /// </summary>
        /// <param name="QJstr">全角</param>
        /// <returns>半角</returns>
        public static string GetBanJiao(string QJstr)
        {
            #region
            char[] c = QJstr.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            string strNew = new string(c);
            return strNew;
            #endregion
        }

        #endregion

        #region 字符串引号处理

        /// <summary>
        /// 将指定字符串中的英文引号替换为中文引号（中文引号没有考虑正反）
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <returns>处理后的值</returns>
        public static string ReplaceQuoteENToCN(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "‘").Replace("\"", "“");
            }
            return str;
        }

        /// <summary>
        /// 将指定字符串中的英文引号替换为html引号实体
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <returns>处理后的值</returns>
        public static string ReplaceQuoteToHTML(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("\"", "&quot;").Replace("'", "&apos;");
            }
            return str;
        }

        /// <summary>
        /// 移除指定字符串中的英文引号
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <returns>处理后的值</returns>
        public static string RemoveQuote(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "").Replace("\"", "");
            }
            return str;
        }

        #endregion

        #region 其它字符串处理

        /// <summary>
        /// 删除结尾的字符串
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="cutStr">要删除的字符串</param>
        /// <returns>处理后的值</returns>
        public static string TrimEnd(string str, string cutStr)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str.TrimEnd(cutStr.ToCharArray());
        }

        /// <summary>
        /// 删除开头的字符串
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="cutStr">要删除的字符串</param>
        /// <returns>处理后的值</returns>
        public static string TrimStart(string str, string cutStr)
        {
            return string.IsNullOrEmpty(str) ? string.Empty : str.TrimStart(cutStr.ToCharArray());
        }

        #endregion

        #region 其它

        /// <summary>
        /// 网站根路径，如:"/"
        /// 注：末尾带'/'
        /// </summary>
        public static string RootURL
        {
            get
            {
                var context = HttpContext.Current;
                if (null == context)
                {
                    return string.Empty;
                }
                string m_ApplicationPath = context.Request.ApplicationPath;
                if (String.IsNullOrEmpty(m_ApplicationPath))
                    m_ApplicationPath = "/";
                if (!m_ApplicationPath.EndsWith("/"))
                    m_ApplicationPath += "/";
                return m_ApplicationPath;
            }
        }

        /// <summary>
        /// 网站根Uri，如:"//www.xcl.com:2156/ or //www.xcl.com:2156/VirtualWeb/"
        /// 注：末尾带'/'
        /// </summary>
        public static string RootUri
        {
            get
            {
                var context = HttpContext.Current;
                if (null == context)
                {
                    return string.Empty;
                }
                string url = string.Empty;
                HttpRequest Req = context.Request;
                string urlAuthority = Req.Url.GetLeftPart(UriPartial.Authority);

                if (Req.ApplicationPath == null || Req.ApplicationPath == "/")
                {
                    //直接安装在   Web   站点
                    url = urlAuthority;
                }
                else
                {
                    //安装在虚拟子目录下
                    url = urlAuthority + Req.ApplicationPath;
                }
                if (!string.IsNullOrEmpty(url))
                {
                    url = url.TrimEnd('/') + "/";
                    url = XCLNetTools.Common.Consts.RegHttpScheme.Replace(url, "//");//替换http(s):// 为"//" 以适应http(s)环境
                }
                return url;
            }
        }

        /// <summary>
        /// 指定起始数字，返回这些数据的List
        /// </summary>
        /// <param name="startNum">开始数字</param>
        /// <param name="endNum">结束数字</param>
        /// <param name="step">步长,默认为1</param>
        /// <returns>如：(0,5,1),则返回0,1,2,3,4,5的list</returns>
        public static List<TextValue> GetStartEndNum(int startNum, int endNum, params int[] step)
        {
            int s = 1;
            if (null != step && step.Length > 0)
            {
                s = step[0];
            }
            List<TextValue> lst = new List<TextValue>();
            if (s > 0)
            {
                while (endNum >= startNum)
                {
                    lst.Add(new TextValue() { Text = startNum.ToString(), Value = startNum.ToString() });
                    startNum += s;
                }
            }
            else
            {
                while (endNum <= startNum)
                {
                    lst.Add(new TextValue() { Text = startNum.ToString(), Value = startNum.ToString() });
                    startNum += s;
                }
            }
            return lst;
        }

        /// <summary>
        /// 判断当前请求是否为ajax请求
        /// </summary>
        /// <returns>是否为ajax请求</returns>
        public static bool IsAjax()
        {
            var flag = false;
            if (null != HttpContext.Current)
            {
                var heads = HttpContext.Current.Request.Headers;
                if (null != heads && heads.AllKeys.Contains("X-Requested-With"))
                {
                    flag = string.Equals(Convert.ToString(heads["X-Requested-With"]), "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
                }
            }
            return flag;
        }

        /// <summary>
        /// 判断指定值是否为指定的类型
        /// </summary>
        /// <typeparam name="T">是否为此类型</typeparam>
        /// <param name="val">需要判断的值</param>
        /// <returns>判断结果</returns>
        public static bool Is<T>(string val)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.IsValid(val);
        }

        #endregion 其它
    }
}