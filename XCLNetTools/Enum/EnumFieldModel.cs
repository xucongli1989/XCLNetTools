using System;

namespace XCLNetTools.Enum
{
    /// <summary>
    /// Enum项model
    /// </summary>
    [Serializable]
    public class EnumFieldModel
    {
        /// <summary>
        /// text值
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// value值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// description特性
        /// </summary>
        public string Description { get; set; }
    }
}