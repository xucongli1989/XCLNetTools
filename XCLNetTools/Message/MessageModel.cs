/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-02

四：更新内容：
1：更新表单获取的参数类型
2：更改Message/JsonMsg类的目录
3：删除多余的方法
4：修复一处未dispose问题
5：整理部分代码
6：添加 MethodResult.cs
7：获取枚举list时可以使用byte/short等
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
        public string Title { get; set; }

        /// <summary>
        /// 消息提示时间
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// 消息提示内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息详细信息
        /// </summary>
        public string MessageMore { get; set; }

        /// <summary>
        /// 消息发生页地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 消息页来源地址(reffer)
        /// </summary>
        public string FromUrl { get; set; }

        /// <summary>
        /// 是否成功与失败的标识
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 是否需要刷新
        /// </summary>
        public bool IsRefresh { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否需要跳转
        /// </summary>
        public bool IsRedirect { get; set; }

        /// <summary>
        /// 要跳转的url
        /// </summary>
        public string RedirectURL { get; set; }

        /// <summary>
        /// 跳转方式
        /// </summary>
        public RedirectTargetEnum RedirectTarget { get; set; }

        /// <summary>
        /// 当前请求是否为ajax请求
        /// </summary>
        public bool IsAjax
        {
            get
            {
                return XCLNetTools.StringHander.Common.IsAjax();
            }
        }

        /// <summary>
        /// 错误码
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// 自定义输出对象
        /// </summary>
        public object CustomObject { get; set; }
    }

    /// <summary>
    /// 跳转方式
    /// </summary>
    public enum RedirectTargetEnum
    {
        /// <summary>
        /// 在当前页打开
        /// </summary>
        None = 0,

        /// <summary>
        /// 新窗口打开
        /// </summary>
        NewBlank = 1
    }
}