/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using XCLNetTools.Entity.Serialize;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// JSON序列化相关
    /// </summary>
    public static class JSON
    {
        /// <summary>
        /// 将对象序列化为json
        /// </summary>
        public static string Serialize(object obj, JsonConvertOption ops = null)
        {
            var result = string.Empty;
            if (null == obj)
            {
                return result;
            }
            if (null == ops)
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            if (ops.IsConvertEnumToString)
            {
                settings.Converters = new List<JsonConverter>() { new StringEnumConverter() };
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, settings);
        }

        /// <summary>
        /// 将json反序列化为一个对象
        /// </summary>
        public static T DeSerialize<T>(string str) where T : class
        {
            T result = default(T);
            if (string.IsNullOrWhiteSpace(str))
            {
                return result;
            }
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
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

            json = json.Trim();
            if (!(json.StartsWith("{") && json.EndsWith("}")) && !(json.StartsWith("[") && json.EndsWith("]")))
            {
                return false;
            }
            try
            {
                return null != JToken.Parse(json);
            }
            catch
            {
                //
            }
            return false;
        }
    }
}