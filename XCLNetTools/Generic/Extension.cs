using System.Collections.Generic;
using System.Linq;

namespace XCLNetTools.Generic
{
    /// <summary>
    /// 泛型扩展方法
    /// </summary>
    public static class Extension
    {
        #region IEnumerable

        /// <summary>
        /// 判断IEnumerable是否为空
        /// </summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return null == source || source.Count() == 0;
        }

        /// <summary>
        /// 判断IEnumerable是否有值
        /// </summary>
        public static bool IsNotNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return null != source && source.Count() > 0;
        }

        #endregion IEnumerable

        #region 通用

        /// <summary>
        /// 将T转为json字符串
        /// </summary>
        public static string ToJson<T>(this T source)
        {
            if (null == source)
            {
                return null;
            }
            return XCLNetTools.Serialize.JSON.Serialize(source);
        }

        /// <summary>
        /// 判断T是否为null
        /// </summary>
        public static bool IsNull<T>(this T source)
        {
            return null == source;
        }

        /// <summary>
        /// 判断T不为null
        /// </summary>
        public static bool IsNotNull<T>(this T source)
        {
            return null != source;
        }

        #endregion 通用
    }
}