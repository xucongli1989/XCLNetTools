using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
        /// 取得用户http客户端IP(穿过代理服务器取远程用户真实IP地址)
        /// </summary>
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
        /// 根据第三方ip查询网站反馈结果获取服务端外网ip地址
        /// </summary>
        public static XCLNetTools.Entity.LocationEntity GetIPFromPublicWeb()
        {
            var model = new XCLNetTools.Entity.LocationEntity();
            var http = new Http.HttpHelper();
            dynamic ipResult = null;
            try
            {
                var result = http.GetHtml(new XCLNetTools.Entity.Http.HttpItem()
                {
                    URL = "https://www.ip.cn/api/index?ip=&type=0",
                    Method = "get",
                    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36",
                    Timeout = 10000
                });
                //如：{"rs":1,"code":0,"address":"中国  广东省 深圳市 电信","ip":"113.87.183.231","isDomain":0}
                var html = result.Html;
                if (!string.IsNullOrWhiteSpace(html))
                {
                    ipResult = Newtonsoft.Json.JsonConvert.DeserializeObject(html);
                }
            }
            catch
            {
                //
            }
            if (ipResult is null)
            {
                return model;
            }
            model.IP = ipResult.ip;
            model.Address = ipResult.address;
            return model;
        }

        /// <summary>
        /// 获取本机有效的本地IP地址
        /// </summary>
        public static string GetLocalIP()
        {
            var localIP = "";
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }
    }
}