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