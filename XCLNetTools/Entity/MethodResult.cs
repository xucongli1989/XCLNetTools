/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-01

四：更新内容：
1：
 */

using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 方法返回值实体，主要是方便一个方法输出多个信息时使用，同时也减少使用out返回结果信息
    /// </summary>
    [Serializable]
    public class MethodResult<T>
    {
        private bool _isSuccess = true;

        /// <summary>
        /// 该方法执行的逻辑是否成功（默认为true）
        /// </summary>
        public bool IsSuccess
        {
            get { return this._isSuccess; }
            set { this._isSuccess = value; }
        }

        /// <summary>
        /// 该方法返回的消息提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 该方法返回的结果
        /// </summary>
        public T Result { get; set; }
    }
}