/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.Entity.Message
{
    /// <summary>
    /// json消息正文信息
    /// </summary>
    [Serializable]
    public class JsonMsgBody
    {
        /// <summary>
        /// 主数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 扩展数据
        /// </summary>
        public object ExtendData { get; set; }
    }
}