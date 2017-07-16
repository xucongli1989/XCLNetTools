using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace XCLNetTools.Common
{
    /// <summary>
    /// IP处理
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// 取得用户客户端IP(穿过代理服务器取远程用户真实IP地址)
        /// </summary>
        /// <returns>ip地址</returns>
        public static string GetClientIP()
        {
            var context = HttpContext.Current;
            if (null == context)
            {
                return string.Empty;
            }
            string result = string.Empty;
            try
            {
                string ipAddress = string.Empty;
                if (context.Request.ServerVariables.HasKeys() && context.Request.ServerVariables.AllKeys.ToList().Exists(k => string.Equals(k, "HTTP_X_FORWARDED_FOR", StringComparison.OrdinalIgnoreCase)))
                {
                    ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                    if (!string.IsNullOrEmpty(ipAddress))
                    {
                        string[] addresses = ipAddress.Split(',');
                        if (addresses.Length != 0)
                        {
                            result = addresses[0];
                        }
                    }
                }

                if (string.IsNullOrEmpty(result))
                {
                    result = context.Request.ServerVariables["REMOTE_ADDR"];
                }
            }
            catch
            {
                //
            }
            return string.Equals("::1", result) ? "127.0.0.1" : result;
        }

        /// <summary>
        /// 根据ip138网站反馈结果获取服务端外网ip地址
        /// </summary>
        /// <returns>ip地址</returns>
        public static string GetIpByIP138()
        {
            string url = "http://1212.ip138.com/ic.asp";
            string ip = string.Empty;
            XCLNetTools.Http.HttpHelper http = new Http.HttpHelper();
            try
            {
                var result = http.GetHtml(new XCLNetTools.Entity.Http.HttpItem()
                {
                    URL = url,
                    Method = "get"
                });
                string html = result.Html;//如：您的IP是：[112.13.32.16] 来自：浙江省绍兴市 移动
                if (!string.IsNullOrEmpty(html))
                {
                    Regex reg = new Regex(@"您的IP是：\[(.*)\]");
                    if (reg.IsMatch(html))
                    {
                        ip = reg.Match(html).Groups[1].Value.ToUpper().Trim();//如：112.13.32.16
                    }
                }
            }
            catch
            {
                //
            }
            return ip;
        }
    }
}