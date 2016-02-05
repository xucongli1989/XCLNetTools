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

using System;
using System.IO;
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
    }
}