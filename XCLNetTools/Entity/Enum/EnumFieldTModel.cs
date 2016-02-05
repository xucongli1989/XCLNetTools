using System;

namespace XCLNetTools.Entity.Enum
{
    /// <summary>
    /// 枚举model
    /// </summary>
    /// <typeparam name="T">枚举value值类型（可为byte、sbyte、short、ushort、int、uint、long 或 ulong。）</typeparam>
    [Serializable]
    public class EnumFieldTModel<T>
    {
        /// <summary>
        /// text值
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// value值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// description特性
        /// </summary>
        public string Description { get; set; }
    }
}