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


using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// xml序列化相关
    /// </summary>
    public class XML
    {
        #region 反序列化

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns>对象</returns>
        public static T Deserialize<T>(string xml) where T : class
        {
            T result = default(T);
            using (StringReader sr = new StringReader(xml))
            {
                XmlSerializer xmldes = new XmlSerializer(typeof(T));
                result = xmldes.Deserialize(sr) as T;
            }
            return result;
        }

        /// <summary>
        /// 从xml文件中反序列化
        /// </summary>
        /// <param name="xmlFilePath">xml路径</param>
        /// <returns>对象</returns>
        public static T DeserializeFromXMLFile<T>(string xmlFilePath) where T : class
        {
            T result = default(T);
            using (XmlTextReader rd = new XmlTextReader(xmlFilePath))
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(T));
                result = xmlSer.Deserialize(rd) as T;
            }
            return result;
        }

        #endregion 反序列化

        #region 序列化

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>xml</returns>
        public static string Serializer<T>(T obj) where T : new()
        {
            string str = string.Empty;
            using (MemoryStream sm = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                xml.Serialize(sm, obj);
                sm.Position = 0;
                using (StreamReader sr = new StreamReader(sm))
                {
                    str = sr.ReadToEnd();
                }
            }
            return str;
        }

        #endregion 序列化
    }
}