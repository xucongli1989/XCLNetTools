using System;

namespace XCLNetTools.Entity.Message
{
    /// <summary>
    /// json消息信息
    /// </summary>
    [Serializable]
    public class JsonMsg
    {
        /// <summary>
        /// 构造函数，初始化head和body
        /// </summary>
        public JsonMsg()
        {
            this.Head = new JsonMsgHead();
            this.Body = new JsonMsgBody();
        }

        /// <summary>
        /// head
        /// </summary>
        public JsonMsgHead Head { get; set; }

        /// <summary>
        /// body
        /// </summary>
        public JsonMsgBody Body { get; set; }
    }
}