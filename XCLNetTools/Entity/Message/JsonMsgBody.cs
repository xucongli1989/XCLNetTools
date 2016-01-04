using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
