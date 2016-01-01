using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 键值类
    /// </summary>
    [Serializable]
    public class KeyValue
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}