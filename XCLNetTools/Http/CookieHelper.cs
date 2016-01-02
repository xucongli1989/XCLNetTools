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
using System.Text.RegularExpressions;
using System.Web;
using XCLNetTools.Generic;

namespace XCLNetTools.Http
{
    /// <summary>
    /// Cookie操作帮助类
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// 根据字符生成Cookie列表
        /// </summary>
        /// <param name="cookie">Cookie集合的字符串形式</param>
        /// <returns>key value实体</returns>
        public static List<XCLNetTools.Entity.KeyValue> GetCookieList(string cookie)
        {
            List<XCLNetTools.Entity.KeyValue> cookielist = new List<XCLNetTools.Entity.KeyValue>();
            foreach (string item in cookie.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Regex.IsMatch(item, @"([\s\S]*?)=([\s\S]*?)$"))
                {
                    Match m = Regex.Match(item, @"([\s\S]*?)=([\s\S]*?)$");
                    cookielist.Add(new XCLNetTools.Entity.KeyValue() { Key = m.Groups[1].Value, Value = m.Groups[2].Value });
                }
            }
            return cookielist;
        }

        /// <summary>
        /// 根据key值得到Cookie值
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="cookie">Cookie集合的字符串形式</param>
        /// <returns>key的值</returns>
        public static string GetCookieValue(string key, string cookie)
        {
            var lst = GetCookieList(cookie);
            if (lst.IsNotNullOrEmpty())
            {
                var item = lst.Find(k => string.Equals(k.Key, key, StringComparison.OrdinalIgnoreCase));
                if (null != item)
                {
                    return item.Value;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 格式化Cookie为标准格式
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="value">Value值</param>
        /// <returns></returns>
        public static string CookieFormat(string key, string value)
        {
            return string.Format("{0}={1};", key, value);
        }

        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="mainName">主键</param>
        /// <param name="mainValue">值</param>
        /// <param name="days">天数</param>
        /// <returns>是否设置成功</returns>
        public static bool SetCookies(string mainName, string mainValue, int days)
        {
            try
            {
                HttpCookie cookie = null != System.Web.HttpContext.Current.Request.Cookies ? System.Web.HttpContext.Current.Request.Cookies[mainName] : null;
                if (cookie == null)
                {
                    cookie = new HttpCookie(mainName, mainValue);
                }
                else
                {
                    cookie.Value = mainValue;
                }
                cookie.Expires = DateTime.Now.AddDays(days);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取cookies
        /// </summary>
        public static string GetCookies(string name)
        {
            HttpCookieCollection collection = System.Web.HttpContext.Current.Request.Cookies;
            if (null != collection && null != collection[name])
            {
                return collection[name].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取cookies集合
        /// </summary>
        public static NameValueCollection GetCookiesCollection(string name)
        {
            HttpCookieCollection collection = System.Web.HttpContext.Current.Request.Cookies;
            if (null != collection && null != collection[name])
            {
                return collection[name].Values;
            }
            return null;
        }

        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <returns>是否删除成功</returns>
        public static bool DelCookies(string name)
        {
            HttpCookie cookie = null != System.Web.HttpContext.Current.Request.Cookies ? System.Web.HttpContext.Current.Request.Cookies[name] : null;
            if (null == cookie)
            {
                return true;
            }
            try
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}