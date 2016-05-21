/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// 其它对象序列化相关
    /// </summary>
    public class Lib
    {
        #region Byte相关

        /// <summary>
        /// 把字节反序列化成相应的对象
        /// </summary>
        /// <param name="pBytes">字节流</param>
        /// <returns>T</returns>
        public T DeserializeObject<T>(byte[] pBytes) where T : class
        {
            T result = default(T);
            if (pBytes == null || pBytes.Length == 0)
            {
                return result;
            }
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(pBytes))
            {
                memory.Position = 0;
                BinaryFormatter formatter = new BinaryFormatter();
                result = formatter.Deserialize(memory) as T;
            }
            return result;
        }

        #endregion Byte相关

        #region 序列化方式的深度克隆对象

        /// <summary>
        /// 对象深度clone（被clone对象必须可以序列化）
        /// </summary>
        /// <param name="source">要克隆的对象</param>
        /// <returns>克隆后的新对象</returns>
        public static T DeepClone<T>(T source) where T : class
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }
            T result = default(T);
            if (Object.ReferenceEquals(source, null))
            {
                return result;
            }
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                result = formatter.Deserialize(stream) as T;
            }
            return result;
        }

        #endregion 序列化方式的深度克隆对象

        #region JObject 相关

        /// <summary>
        /// 将JObject的属性填充至指定的dictionary
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="p">JObject的属性</param>
        private static void JObjectFillDictionary(Dictionary<string, string> result, JProperty p)
        {
            if (null == p)
            {
                return;
            }
            var ps = p.Values();
            if (null == ps)
            {
                return;
            }
            var valCount = ps.Count();
            if (valCount == 1)
            {
                result.Add(p.Path, p.Value.ToString());
            }
            else if (valCount > 1)
            {
                foreach (var m in p.Values())
                {
                    JObjectFillDictionary(result, (JProperty)m);
                }
            }
        }

        /// <summary>
        /// 将JObject类型的属性转换为dictionary
        /// </summary>
        /// <returns>dictionary结果</returns>
        public static Dictionary<string, string> ConvertJObjectToDictionary(JObject obj)
        {
            if (null == obj)
            {
                return null;
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var pros = obj.Properties();
            foreach (var m in pros)
            {
                JObjectFillDictionary(dic, m);
            }
            return dic;
        }

        /// <summary>
        /// 将JObject的属性转换为url参数形式
        /// 如：{"a":"1","b":"2","c":{"d":"100"}} -> a=1&amp;b=2&amp;c[d]=100
        /// </summary>
        /// <param name="obj">需要转换的对象</param>
        /// <returns>url参数字符串</returns>
        public static string ConvertJObjectToUrlParameters(JObject obj)
        {
            var dic = ConvertJObjectToDictionary(obj);
            if (null == dic || dic.Count == 0)
            {
                return null;
            }
            List<string> str = new List<string>(dic.Count);
            string tempKey = string.Empty;
            for (int i = 0; i < dic.Count; i++)
            {
                var d = dic.ElementAt(i);
                if (d.Key.IndexOf('.') >= 0)
                {
                    tempKey = (d.Key.Replace(".", "][") + "]");
                    tempKey = tempKey.Remove(tempKey.IndexOf(']'), 1);
                    str.Add(string.Format("{0}={1}", tempKey, System.Web.HttpUtility.UrlEncode(d.Value)));
                }
                else
                {
                    str.Add(string.Format("{0}={1}", d.Key, System.Web.HttpUtility.UrlEncode(d.Value)));
                }
            }

            return string.Join("&", str);
        }

        #endregion JObject 相关
    }
}