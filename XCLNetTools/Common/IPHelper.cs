using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Web;

namespace XCLNetTools.Common
{
    /// <summary>
    /// IP处理
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// 是否为一个有效的IP地址
        /// </summary>
        /// <param name="ip">ip 字符串</param>
        /// <param name="ignoreLoopback">是否忽略环回地址</param>
        public static bool IsValidIP(string ip, bool ignoreLoopback = true)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                return false;
            }
            if (!IPAddress.TryParse(ip, out IPAddress ipInfo))
            {
                return false;
            }
            if (ignoreLoopback)
            {
                return !IPAddress.IsLoopback(ipInfo);
            }
            return true;
        }

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
        /// 根据第三方ip查询网站 ip138.com 获取当前请求的外网ip地址
        /// </summary>
        /// <param name="ip138Token">接口token</param>
        /// <param name="ip">要查询的IP，如果为空，则查询本地IP</param>
        public static XCLNetTools.Entity.LocationEntity GetIPFromPublicWeb(string ip138Token, string ip)
        {
            var model = new XCLNetTools.Entity.LocationEntity();
            if (!string.IsNullOrWhiteSpace(ip) && !IPHelper.IsValidIP(ip))
            {
                return model;
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(2);
                    var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.ip138.com/ip?token={ip138Token}&ip={ip}");
                    var response = client.SendAsync(request).GetAwaiter().GetResult();
                    var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    var ipResult = Newtonsoft.Json.JsonConvert.DeserializeObject(json) as dynamic;
                    model.IP = ipResult.ip;
                    model.Address = string.Join("-", ipResult.data);
                }
            }
            catch
            {
                //
            }
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