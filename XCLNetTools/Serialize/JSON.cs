using System.Web.Script.Serialization;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// JSON序列化相关
    /// </summary>
    public class JSON
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
    }
}