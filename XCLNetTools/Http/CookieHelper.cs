﻿/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

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
            var cookielist = new List<XCLNetTools.Entity.KeyValue>();
            foreach (var item in cookie.Split(new string[] { ";", "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Regex.IsMatch(item, @"([\s\S]*?)=([\s\S]*?)$"))
                {
                    var m = Regex.Match(item, @"([\s\S]*?)=([\s\S]*?)$");
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
        /// <returns>格式化的值，如：a=b;</returns>
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
                var cookie = null != HttpContext.Current.Request.Cookies ? HttpContext.Current.Request.Cookies[mainName] : null;
                if (cookie == null)
                {
                    cookie = new HttpCookie(mainName, mainValue);
                }
                else
                {
                    cookie.Value = mainValue;
                }
                cookie.Expires = DateTime.Now.AddDays(days);
                HttpContext.Current.Response.Cookies.Add(cookie);
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
        /// <param name="name">cookie名</param>
        /// <returns>cookie的值</returns>
        public static string GetCookies(string name)
        {
            var collection = HttpContext.Current.Request.Cookies;
            if (null != collection && null != collection[name])
            {
                return collection[name].Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取cookies集合
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <returns>值的集合</returns>
        public static NameValueCollection GetCookiesCollection(string name)
        {
            var collection = HttpContext.Current.Request.Cookies;
            if (null != collection && null != collection[name])
            {
                return collection[name].Values;
            }
            return new NameValueCollection();
        }

        /// <summary>
        /// 删除Cookies
        /// </summary>
        /// <param name="name">cookie名</param>
        /// <returns>是否删除成功</returns>
        public static bool DelCookies(string name)
        {
            var cookie = null != HttpContext.Current.Request.Cookies ? HttpContext.Current.Request.Cookies[name] : null;
            if (null == cookie)
            {
                return true;
            }
            try
            {
                //直接 new 一个新 cookie 对象，而不是复用已有的对象（因为在 web.config 中设置的 httpCookies 在新对象时有效）
                var newCookie = new HttpCookie(cookie.Name, cookie.Value);
                newCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(newCookie);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}