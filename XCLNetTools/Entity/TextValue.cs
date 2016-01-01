using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 键值类
    /// </summary>
    [Serializable]
    public class TextValue
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}