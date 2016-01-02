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
using System.Collections.Specialized;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// URL的操作类
    /// </summary>
    public class UrlOper
    {
        #region 操作URL参数

        /// <summary>
        /// 添加URL参数
        /// </summary>
        public static string AddParam(string url, string name, string value)
        {
            NameValueCollection nv = new NameValueCollection();
            nv.Add(name, value);
            return AddParam(url, nv);
        }

        /// <summary>
        /// 添加URL参数
        /// </summary>
        public static string AddParam(string url, NameValueCollection param)
        {
            if (string.IsNullOrEmpty(url) || (null == param || param.Count == 0))
            {
                return url;
            }

            url = url.Trim().TrimEnd(new char[] { '?', '&', ' ' }).Trim();

            Uri uri = null;
            if (!Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri))
            {
                return url;
            }

            string firstStr = string.Empty;
            if (uri.IsAbsoluteUri)
            {
                //绝对
                firstStr = string.IsNullOrEmpty(uri.Query) ? (url + "?") : (url + "&");
            }
            else
            {
                //相对
                firstStr = url.Contains("?") ? (url + "&") : (url + "?");
            }

            List<string> paramLst = new List<string>();
            foreach (string m in param)
            {
                paramLst.Add(string.Format("{0}={1}", m, HttpContext.Current.Server.UrlEncode(param[m])));
            }

            return firstStr + string.Join("&", paramLst.ToArray());
        }

        /// <summary>
        /// 更新URL参数
        /// </summary>
        public static string UpdateParam(string url, string paramName, string value)
        {
            string keyWord = paramName + "=";
            int index = url.IndexOf(keyWord) + keyWord.Length;
            int index1 = url.IndexOf("&", index);
            if (index1 == -1)
            {
                url = url.Remove(index, url.Length - index);
                url = string.Concat(url, value);
                return url;
            }
            url = url.Remove(index, index1 - index);
            url = url.Insert(index, value);
            return url;
        }

        /// <summary>
        /// 删除url参数
        /// </summary>
        public static string RemoveParam(string url, string paramName)
        {
            string[] s = url.Split('?');
            if (s.Length == 2 && s[1].Length > 0)
            {
                string[] ps = s[1].Split('&');
                if (ps.Length > 0)
                {
                    Dictionary<string, string> lst = new Dictionary<string, string>();
                    for (int i = 0; i < ps.Length; i++)
                    {
                        string[] keys = ps[i].Split('=');
                        if (keys.Length == 2)
                        {
                            if (!lst.ContainsKey(keys[0]))
                            {
                                lst.Add(keys[0], keys[1]);
                            }
                        }
                    }
                    lst.Remove(paramName);
                    if (null != lst && lst.Count > 0)
                    {
                        List<string> urlLst = new List<string>();
                        foreach (KeyValuePair<string, string> entry in lst)
                        {
                            urlLst.Add(string.Format("{0}={1}", entry.Key, entry.Value));
                        }
                        url = string.Format("{0}?{1}", s[0], string.Join("&", urlLst.ToArray()));
                    }
                    else
                    {
                        url = s[0];
                    }
                }
            }
            return url;
        }

        #endregion 操作URL参数

        #region 分析URL所属的域

        /// <summary>
        /// 分析 url 字符串中的参数信息
        /// </summary>
        /// <param name="url">输入的 URL</param>
        /// <param name="baseUrl">输出 URL 的基础部分</param>
        /// <param name="nvc">输出分析后得到的 (参数名,参数值) 的集合</param>
        public static void ParseUrl(string url, out string baseUrl, out NameValueCollection nvc)
        {
            if (url == null)
                throw new ArgumentNullException("url");

            nvc = new NameValueCollection();
            baseUrl = "";

            if (url == "")
                return;

            int questionMarkIndex = url.IndexOf('?');

            if (questionMarkIndex == -1)
            {
                baseUrl = url;
                return;
            }
            baseUrl = url.Substring(0, questionMarkIndex);
            if (questionMarkIndex == url.Length - 1)
                return;
            string ps = url.Substring(questionMarkIndex + 1);

            // 开始分析参数对
            Regex re = new Regex(@"(^|&)?(\w+)=([^&]+)(&|$)?", RegexOptions.Compiled);
            MatchCollection mc = re.Matches(ps);

            foreach (Match m in mc)
            {
                nvc.Add(m.Result("$2").ToLower(), m.Result("$3"));
            }
        }

        #endregion 分析URL所属的域

        /// <summary>
        /// 获取指定URL的请求状态：
        ///200 - 确定。客户端请求已成功。
        ///201 - 已创建。
        ///202 - 已接受。
        ///400 - 错误的请求
        ///401 - 访问被拒绝
        ///403 - 禁止访问
        ///404 - 未找到
        ///405 - 用来访问本页面的 HTTP 谓词不被允许（方法不被允许）
        ///406 - 客户端浏览器不接受所请求页面的 MIME 类型。
        ///407 - 要求进行代理身份验证。
        ///412 - 前提条件失败。
        ///413 – 请求实体太大。
        ///414 - 请求 URI 太长。
        ///415 – 不支持的媒体类型。
        ///416 – 所请求的范围无法满足。
        ///417 – 执行失败。
        ///423 – 锁定的错误
        ///500 - 内部服务器错误。
        ///501 - 页眉值指定了未实现的配置。
        ///502 - Web 服务器用作网关或代理服务器时收到了无效响应。
        ///503 - 服务不可用。这个错误代码为 IIS 6.0 所专用。
        ///504 - 网关超时。
        ///505 - HTTP 版本不受支持。
        ///</summary>
        ///<param name="curl">要请求的URL</param>
        public static int GetUrlError(string curl)
        {
            int num = 200;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(curl));
            ServicePointManager.Expect100Continue = false;
            try
            {
                ((HttpWebResponse)request.GetResponse()).Close();
            }
            catch (WebException exception)
            {
                if (exception.Status != WebExceptionStatus.ProtocolError)
                {
                    return num;
                }
                if (exception.Message.IndexOf("500 ") > 0)
                {
                    return 500;
                }
                if (exception.Message.IndexOf("401 ") > 0)
                {
                    return 401;
                }
                if (exception.Message.IndexOf("404") > 0)
                {
                    num = 404;
                }
            }
            return num;
        }
    }
}