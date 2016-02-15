/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-02

四：更新内容：
1：更新表单获取的参数类型
2：更改Message/JsonMsg类的目录
3：删除多余的方法
4：修复一处未dispose问题
5：整理部分代码
6：添加 MethodResult.cs
7：获取枚举list时可以使用byte/short等
 */


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
    }
}