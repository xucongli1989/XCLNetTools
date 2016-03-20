/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.Entity.Enum
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