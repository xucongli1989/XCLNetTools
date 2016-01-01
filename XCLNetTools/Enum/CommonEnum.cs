using System;

namespace XCLNetTools.Enum
{
    /// <summary>
    /// 常用枚举常量
    /// </summary>
    [Serializable]
    public class CommonEnum
    {
        /// <summary>
        /// 是否
        /// </summary>
        public enum 是否
        {
            /// <summary>
            /// 是
            /// </summary>
            是 = 1,

            /// <summary>
            /// 否
            /// </summary>
            否 = 0
        }

        /// <summary>
        /// 静态资源类型
        /// </summary>
        public enum StaticResourceTypeEnum
        {
            /// <summary>
            /// js文件
            /// </summary>
            JS,

            /// <summary>
            /// css文件
            /// </summary>
            CSS,

            /// <summary>
            /// icon
            /// </summary>
            ICON
        }
    }
}