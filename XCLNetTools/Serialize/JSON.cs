/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// JSON序列化相关
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// 提供者枚举
        /// </summary>
        public enum JsonProviderEnum
        {
            /// <summary>
            /// 指定为：System.Web.Script.Serialization
            /// </summary>
            SystemWeb,

            /// <summary>
            /// 指定为：Newtonsoft.Json.JsonConvert
            /// </summary>
            Newtonsoft
        }

        /// <summary>
        /// 将对象序列化为json
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="provider">序列化提供者</param>
        /// <returns>json</returns>
        public static string Serialize(object obj, JsonProviderEnum provider = JsonProviderEnum.SystemWeb)
        {
            string result = string.Empty;
            switch (provider)
            {
                case JsonProviderEnum.SystemWeb:
                    result = new JavaScriptSerializer().Serialize(obj);
                    break;

                case JsonProviderEnum.Newtonsoft:
                    result = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 将json反序列化为一个对象
        /// </summary>
        /// <param name="str">要反序列化的json</param>
        /// <param name="provider">提供者</param>
        /// <returns>对象</returns>
        public static T DeSerialize<T>(string str, JsonProviderEnum provider = JsonProviderEnum.SystemWeb) where T : class
        {
            T result = default(T);
            switch (provider)
            {
                case JsonProviderEnum.SystemWeb:
                    result = new JavaScriptSerializer().Deserialize<T>(str);
                    break;

                case JsonProviderEnum.Newtonsoft:
                    result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
                    break;
            }
            return result;
        }

        /// <summary>
        /// 判断字符串是否为有效的json字符串
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>是、否</returns>
        public static bool IsJSON(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }
            try
            {
                return null != JObject.Parse(json.Trim());
            }
            catch
            {
                //
            }
            return false;
        }
    }
}