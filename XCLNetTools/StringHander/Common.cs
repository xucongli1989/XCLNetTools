/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using XCLNetTools.Entity;
using XCLNetTools.Generic;
using static XCLNetTools.Enum.CommonEnum;

namespace XCLNetTools.StringHander
{
    /// <summary>
    ///公用类
    /// </summary>
    public static class Common
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
            html = HttpUtility.HtmlEncode(html).Trim();
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
            string flag;
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
                    var model = config.StaticResourceList.FirstOrDefault(k => string.Equals(nameList[i].Trim(), k.Name, StringComparison.OrdinalIgnoreCase));
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
                for (int i = 0; i < temp.Length; i++)
                {
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

        /// <summary>
        /// 将指定字符串中的常见分隔符统一替换为指定的分隔符，目前只替换这些符号： ,，、/
        /// </summary>
        public static string ReplaceSplitChar(string str, string splitChar = ",")
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            return new Regex("[,，、/]").Replace(str, splitChar);
        }

        #endregion

        #region 其它字符串处理

        /// <summary>
        /// 删除开头的字符串
        /// </summary>
        public static string RemoveStart(string str, string cutStr, bool isIgnoreCase = false)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(cutStr))
            {
                return str;
            }
            if (str.StartsWith(cutStr, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return str.Substring(cutStr.Length);
            }
            return str;
        }

        /// <summary>
        /// 删除结尾的字符串
        /// </summary>
        public static string RemoveEnd(string str, string cutStr, bool isIgnoreCase = false)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(cutStr))
            {
                return str;
            }
            if (str.EndsWith(cutStr, isIgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
            {
                return str.Substring(0, str.Length - cutStr.Length);
            }
            return str;
        }

        /// <summary>
        /// 移除 List 末尾的 null 或 空白项
        /// </summary>
        public static List<T> TrimEnd<T>(List<T> lst)
        {
            if (lst.IsNullOrEmpty())
            {
                return lst;
            }
            //最后一个有效值的索引
            var maxIndexWithValidValue = -1;
            if (typeof(T) == typeof(string) || typeof(T) == typeof(object))
            {
                maxIndexWithValidValue = lst.FindLastIndex(k => !string.IsNullOrWhiteSpace(Convert.ToString(k)));
            }
            else
            {
                maxIndexWithValidValue = lst.FindLastIndex(k => null != k);
            }
            if (maxIndexWithValidValue == -1)
            {
                return new List<T>();
            }
            //移除末尾的无效项
            if (maxIndexWithValidValue < lst.Count - 1)
            {
                lst.RemoveRange(maxIndexWithValidValue + 1, lst.Count - maxIndexWithValidValue - 1);
            }
            return lst;
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
        /// 将网站根Uri转为开头带http://或https://的url地址
        /// 如:"http://www.xcl.com:2156/ or https://www.xcl.com:2156/VirtualWeb/"
        /// </summary>
        /// <param name="httpType">http类型，默认为http://</param>
        /// <returns>处理后的url</returns>
        public static string GetRootUri(HttpTypeEnum httpType = HttpTypeEnum.Http)
        {
            return Common.ToHttpUrl(Common.RootUri, httpType);
        }

        /// <summary>
        /// 增强GetRootUri方法，从当前url中判断是http还是https
        /// 如:"http://www.xcl.com:2156/ or https://www.xcl.com:2156/VirtualWeb/"
        /// </summary>
        public static string GetRootUri2()
        {
            if (null == HttpContext.Current || null == HttpContext.Current.Request)
            {
                return string.Empty;
            }
            if (Common.IsHttps(HttpContext.Current.Request.Url.AbsoluteUri))
            {
                return GetRootUri(HttpTypeEnum.Https);
            }
            return GetRootUri(HttpTypeEnum.Http);
        }

        /// <summary>
        /// 从指定的header中获取网站根路径，如果没有指定header或内容为空，则返回GetRootUri2()的内容
        /// 如:"http://www.xcl.com:2156/ or https://www.xcl.com:2156/VirtualWeb/"
        /// 注：末尾带'/'
        /// </summary>
        public static string GetRootUriByHeader(string headerName = "X-ROOT-URL")
        {
            var current = HttpContext.Current;
            if (null == current || null == current.Request)
            {
                return string.Empty;
            }
            var headerValue = string.Empty;
            if (null != current.Request.Headers)
            {
                headerValue = current.Request.Headers[headerName];
            }
            if (!string.IsNullOrWhiteSpace(headerValue))
            {
                return headerValue.TrimEnd('/') + "/";
            }
            return GetRootUri2();
        }

        /// <summary>
        /// 将http开头协议不规则的url字符串转化为指定的http://或https://形式的url
        /// </summary>
        /// <param name="url">要转换的url</param>
        /// <param name="httpType">http类型，默认为http://</param>
        /// <returns>转换后的url</returns>
        public static string ToHttpUrl(string url, HttpTypeEnum httpType = HttpTypeEnum.Http)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return string.Empty;
            }
            url = url.Trim().TrimStart(new char[] { ':', '/' });
            url = XCLNetTools.Common.Consts.RegHttpScheme.Replace(url, string.Empty);
            return string.Format("{0}://{1}", httpType.ToString().ToLower(), url);
        }

        /// <summary>
        /// 重载ToHttpUrl，默认url：Request.Url.AbsoluteUri
        /// </summary>
        public static string ToHttpUrl(HttpTypeEnum httpType = HttpTypeEnum.Http)
        {
            if (null == HttpContext.Current)
            {
                return string.Empty;
            }
            return ToHttpUrl(HttpContext.Current.Request.Url.AbsoluteUri, httpType);
        }

        /// <summary>
        /// 判断url字符串是否为"http://"开头（不区分大小写）
        /// </summary>
        public static bool IsHttp(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            return url.ToUpper().StartsWith("HTTP://");
        }

        /// <summary>
        /// 重载IsHttp，默认url：Request.Url.AbsoluteUri
        /// </summary>
        public static bool IsHttp()
        {
            if (null == HttpContext.Current)
            {
                return false;
            }
            return IsHttp(HttpContext.Current.Request.Url.AbsoluteUri);
        }

        /// <summary>
        /// 判断url字符串是否为"https://"开头（不区分大小写）
        /// </summary>
        public static bool IsHttps(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }
            return url.ToUpper().StartsWith("HTTPS://");
        }

        /// <summary>
        /// 重载IsHttps，默认url：Request.Url.AbsoluteUri
        /// </summary>
        public static bool IsHttps()
        {
            if (null == HttpContext.Current)
            {
                return false;
            }
            return IsHttps(HttpContext.Current.Request.Url.AbsoluteUri);
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
        /// 指定起止数字，返回每次加1的数字列表，如：（3,7）=> [3,4,5,6,7]
        /// </summary>
        public static List<int> GetStartEndNumList(int startNum, int endNum)
        {
            var lst = new List<int>();
            for (var i = startNum; i <= endNum; i++)
            {
                lst.Add(i);
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
                if (null != heads && null != heads.AllKeys && heads.AllKeys.Contains("X-Requested-With"))
                {
                    flag = string.Equals(Convert.ToString(heads["X-Requested-With"]), "XMLHttpRequest", StringComparison.OrdinalIgnoreCase);
                }
            }
            return flag;
        }

        /// <summary>
        /// 判断指定值是否可以转换为指定的类型
        /// </summary>
        /// <typeparam name="T">是否可以转换为此类型</typeparam>
        /// <param name="val">需要判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsCanConvert<T>(object val)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return converter.IsValid(val);
        }

        /// <summary>
        /// 将url的name value参数形式的字符串转换为字典
        /// </summary>
        /// <param name="val">要转换的字符串，如：a=b&amp;c=d&amp;c=x</param>
        /// <returns>处理后的结果</returns>
        public static Dictionary<string, IList<string>> ConvertUrlNameValueStringToDic(string val)
        {
            var dic = new Dictionary<string, IList<string>>();
            if (string.IsNullOrWhiteSpace(val))
            {
                return dic;
            }
            string[] tempArr;
            string k, v;
            var arr = val.Trim().Split('&');
            for (var i = 0; i < arr.Length; i++)
            {
                tempArr = arr[i].Split('=');
                k = tempArr[0];
                v = string.Empty;
                if (tempArr.Length >= 2)
                {
                    v = tempArr[1];
                }
                if (dic.ContainsKey(k))
                {
                    dic[k].Add(v);
                }
                else
                {
                    dic.Add(k, new List<string>() { v });
                }
            }
            return dic;
        }

        /// <summary>
        /// 将文件名中无效的特殊字符替换成指定字符
        /// </summary>
        public static string ReplaceInvalidCharsFileName(string fileName, string str = "_")
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return fileName;
            }
            return XCLNetTools.Common.Consts.RegInvalidFileNameChars.Replace(fileName, str);
        }

        /// <summary>
        /// 根据属性名设置对象的属性
        /// </summary>
        public static void SetObjectValueByPropertyName(object obj, string propertyName, object value)
        {
            if (null == obj || string.IsNullOrWhiteSpace(propertyName))
            {
                return;
            }
            obj.GetType().GetProperty(propertyName).SetValue(obj, value);
        }

        /// <summary>
        /// 指定集合的起始值与结束值，与最大值和最小值进行比较并返回符合限制范围内的实体信息（如果起始值或结束值超出限制范围，则该值自动取最小值或最大值）
        /// </summary>
        public static RangeValueEntity GetRangeValueEntity(int startValue, int endValue, int minValue, int maxValue)
        {
            //不符合比较条件
            if (startValue > endValue || minValue > maxValue)
            {
                return null;
            }

            //完全无交集
            if ((startValue < minValue && endValue < minValue) || (startValue > maxValue && endValue > maxValue))
            {
                return null;
            }

            //纠正临界值
            if (startValue < minValue)
            {
                startValue = minValue;
            }
            if (startValue > maxValue)
            {
                startValue = maxValue;
            }
            if (endValue < minValue)
            {
                endValue = minValue;
            }
            if (endValue > maxValue)
            {
                endValue = maxValue;
            }

            var result = new RangeValueEntity();
            result.StartValue = startValue;
            result.EndValue = endValue;
            return result;
        }

        /// <summary>
        /// 将范围文本字符串解析为实体列表，并且自动将倒数的数字转为正数的数字，文本的格式如下：
        /// 【1】表示第1项，【2】表示第2项，【2:5】表示第2项到第5项，【-1】表示最后一项，【-2】表示倒数第2项，【-5:-2】表示倒数第5项到倒数第2项，【2,4:7,-5:-2】表示第2项和第4到7项和倒数第5项至倒数第2项
        /// </summary>
        /// <param name="str">范围字符串</param>
        /// <param name="minValue">最小值限制</param>
        /// <param name="maxValue">最大值限制</param>
        /// <param name="isNeedOptimize">是否需要优化处理后的结果（去重+合并有交叉的范围）</param>
        public static List<RangeValueEntity> GetRangeValueEntityListFromText(string str, int minValue, int maxValue, bool isNeedOptimize = false)
        {
            str = str?.Replace('，', ',').Replace('：', ':').Trim().Trim(',');

            var lst = new List<RangeValueEntity>();
            if (string.IsNullOrWhiteSpace(str))
            {
                return lst;
            }

            //开始解析
            str = new Regex(@"\s+").Replace(str, "");
            str.Split(',').ToList().ForEach(item =>
            {
                var arr = item.Split(':');
                if (arr.Length > 2)
                {
                    return;
                }
                var model = new RangeValueEntity();
                if (arr.Length == 1)
                {
                    model.StartValue = XCLNetTools.Common.DataTypeConvert.ToInt(arr[0]);
                    model.EndValue = model.StartValue;
                }
                if (arr.Length == 2)
                {
                    model.StartValue = XCLNetTools.Common.DataTypeConvert.ToInt(arr[0]);
                    model.EndValue = XCLNetTools.Common.DataTypeConvert.ToInt(arr[1]);
                }
                if (model.StartValue == 0 || model.EndValue == 0)
                {
                    return;
                }
                //将负数转成正数
                if (model.StartValue < 0)
                {
                    model.StartValue = maxValue + model.StartValue + 1;
                }
                if (model.EndValue < 0)
                {
                    model.EndValue = maxValue + model.EndValue + 1;
                }
                model = Common.GetRangeValueEntity(model.StartValue, model.EndValue, minValue, maxValue);
                if (null == model)
                {
                    return;
                }
                lst.Add(model);
            });

            if (lst.IsNullOrEmpty())
            {
                return lst;
            }

            //优化集合范围列表，比如范围中有两个值：2到5、3到6，则最终会合并成：2到6
            if (isNeedOptimize)
            {
                var indexLst = new List<int>();
                lst.ForEach(k =>
                {
                    for (var i = k.StartValue; i <= k.EndValue; i++)
                    {
                        indexLst.Add(i);
                    }
                });
                indexLst = indexLst.Distinct().OrderBy(k => k).ToList();//此时会得到已去重且按升序排列的纯数字：[1,2,3,6,7,8,9,10,22,100]
                var result = new List<RangeValueEntity>();
                result.Add(new RangeValueEntity()
                {
                    StartValue = indexLst[0],
                    EndValue = indexLst[0]
                });
                if (indexLst.Count >= 2)
                {
                    for (var i = 1; i < indexLst.Count; i++)
                    {
                        var currentIndex = indexLst[i];
                        var lastItem = result.Last();
                        if (currentIndex == lastItem.EndValue + 1)
                        {
                            lastItem.EndValue = currentIndex;
                            continue;
                        }
                        var model = new RangeValueEntity();
                        model.StartValue = currentIndex;
                        model.EndValue = currentIndex;
                        result.Add(model);
                    }
                }
                return result;
            }

            return lst;
        }

        /// <summary>
        /// 根据单个范围值返回【表示位置】的范围对象
        /// 0：整个内容最左边的位置；2：第二项右边的位置；-1：最后一项右边的位置
        /// </summary>
        public static RangeValueEntity GetPositionRangeValueEntity(int positionRangeValue, int minValue, int maxValue)
        {
            if (positionRangeValue == 0)
            {
                if (minValue == 0)
                {
                    return new RangeValueEntity() { StartValue = 0, EndValue = 0 };
                }
                return null;
            }
            return GetRangeValueEntityListFromText(positionRangeValue.ToString(), minValue, maxValue).FirstOrDefault();
        }

        /// <summary>
        /// 自动截断过长的日志字符串（取字符串的前面一部分和后面一部分）
        /// </summary>
        public static string GetShortLogMessage(string str)
        {
            var maxLen = 2000;
            if (string.IsNullOrEmpty(str) || str.Length <= maxLen)
            {
                return str;
            }
            return $"{str.Substring(0, maxLen / 2)}...内容过多，已省略部分内容...{str.Substring(str.Length - maxLen / 2)}";
        }

        /// <summary>
        /// 将日志列表转换为日志字符串（最多只保留 2000 个字符长度）
        /// </summary>
        public static string GetLogStringFromList(List<string> lst)
        {
            if (lst.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return GetShortLogMessage(string.Join("；" + Environment.NewLine, lst));
        }

        /// <summary>
        /// 将字符串转换为占位符格式的字符串（如：a -> {a}）
        /// </summary>
        public static string ConvertStrToPlaceholder(string str)
        {
            return $"{{{(str ?? "").Trim()}}}";
        }

        /// <summary>
        /// 删除字符串开头的下划线
        /// </summary>
        public static string RemoveStartUnderline(string str)
        {
            return str?.TrimStart('_');
        }

        /// <summary>
        /// 将普通字符串中的换行符 \n 替换成当前系统的换行符 \r\n
        /// </summary>
        public static string ConvertToSystemNewLine(string str)
        {
            str = str ?? string.Empty;
            return str.Replace(Environment.NewLine, "\n").Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// 获取字符串中的每一行（支持 \n 和 \r\n）
        /// </summary>
        public static List<string> GetLines(string str)
        {
            str = ConvertToSystemNewLine(str ?? string.Empty);
            return str.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary>
        /// 获取两个字符串的所有行，并存放到一个相对应的列表中
        /// </summary>
        public static List<LineItem<string>> GetLines(string a, string b)
        {
            var result = new List<LineItem<string>>();
            var aLines = XCLNetTools.StringHander.Common.GetLines(a);
            var bLines = XCLNetTools.StringHander.Common.GetLines(b);

            var aCount = aLines.Count;
            var bCount = bLines.Count;
            var maxCount = Math.Max(aCount, bCount);

            if (aCount < maxCount)
            {
                aLines.AddRange(new string[maxCount - aCount]);
            }
            if (bCount < maxCount)
            {
                bLines.AddRange(new string[maxCount - bCount]);
            }

            for (var i = 0; i < aLines.Count; i++)
            {
                var item = new LineItem<string>();
                item.LineNumber = i + 1;
                item.Item1 = aLines[i];
                item.Item2 = bLines[i];
                result.Add(item);
            }
            return result;
        }

        /// <summary>
        /// 获取原始字符串的字面量形式，比如：null 返回字符串 "null"；换行符直接返回字符串 "\r\n"
        /// </summary>
        public static string GetRaw(string str)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(str), writer, null);
                    return writer.ToString();
                }
            }
        }

        /// <summary>
        /// 根据 css 中的 RGBA 颜色获取 Color 对象（因为 css 中的 color 是按 RGBA 排序，而 c# 中的 color 转换方法是按 ARGB 排序）
        /// </summary>
        /// <param name="htmlHex">RGBA 颜色，如：#ff663312</param>
        public static System.Drawing.Color GetColorFromHtmlHexString(string htmlHex)
        {
            var hex = htmlHex.Replace("#", "").Trim();
            if (string.IsNullOrWhiteSpace(hex))
            {
                return System.Drawing.Color.FromArgb(0, 0, 0);
            }
            var len = hex.Length;
            if (len != 6 && len != 8)
            {
                throw new ArgumentOutOfRangeException("颜色格式不正确！");
            }
            if (len == 6)
            {
                hex = $"ff{hex}";
            }
            else if (len == 8)
            {
                var rgb = hex.Substring(0, 6);
                var a = hex.Substring(6, 2);
                hex = a + rgb;
            }
            return System.Drawing.ColorTranslator.FromHtml($"#{hex}");
        }

        /// <summary>
        /// 转义正则中的新内容（将 $ 替换为 $$，从而防止分组替换解析）
        /// </summary>
        public static string EscapeRegexNewValue(string newValue)
        {
            return newValue?.Replace("$", "$$");
        }

        /// <summary>
        /// 删除字符串中的一部分（与原生方法 str.Remove 的区别是这个方法中的参数有溢出时不会报错）
        /// </summary>
        public static string RemoveWithSafe(this string str, int startIndex, int? count = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            if (startIndex < 0 || startIndex > str.Length - 1)
            {
                return str;
            }
            if (null == count)
            {
                return str.Remove(startIndex);
            }
            var c = count.GetValueOrDefault();
            if (c <= 0)
            {
                return str;
            }
            if (c > str.Length - (startIndex + 1) + 1)
            {
                c = str.Length - (startIndex + 1) + 1;
            }
            return str.Remove(startIndex, c);
        }

        /// <summary>
        /// 计算数学表达式
        /// </summary>
        public static double? Calculate(string str)
        {
            try
            {
                var dt = new DataTable();
                return Convert.ToDouble(dt.Compute(str, ""));
            }
            catch
            {
                return null;
            }
        }

        #endregion 其它
    }
}