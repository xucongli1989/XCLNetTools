/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.Message
{
    /// <summary>
    /// 消息提示实体类(用于json属性)
    /// </summary>
    [Serializable]
    public class MessageModel
    {
        /// <summary>
        /// 提示标题
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 消息提示时间
        /// </summary>
        public virtual DateTime? Date { get; set; }

        /// <summary>
        /// 消息提示内容
        /// </summary>
        public virtual string Message { get; set; }

        /// <summary>
        /// 消息详细信息
        /// </summary>
        public virtual string MessageMore { get; set; }

        /// <summary>
        /// 消息发生页地址
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 消息页来源地址(reffer)
        /// </summary>
        public virtual string FromUrl { get; set; }

        /// <summary>
        /// 是否成功与失败的标识
        /// </summary>
        public virtual bool IsSuccess { get; set; }

        /// <summary>
        /// 是否需要刷新
        /// </summary>
        public virtual bool IsRefresh { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 是否需要跳转
        /// </summary>
        public virtual bool IsRedirect { get; set; }

        /// <summary>
        /// 要跳转的url
        /// </summary>
        public virtual string RedirectURL { get; set; }

        /// <summary>
        /// 跳转方式
        /// </summary>
        public virtual XCLNetTools.Enum.CommonEnum.RedirectTargetEnum RedirectTarget { get; set; }

        /// <summary>
        /// 当前请求是否为ajax请求
        /// </summary>
        public virtual bool IsAjax
        {
            get
            {
                return XCLNetTools.StringHander.Common.IsAjax();
            }
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public virtual string ErrorCode { get; set; }

        /// <summary>
        /// 自定义输出对象
        /// </summary>
        public virtual object CustomObject { get; set; }
    }
}