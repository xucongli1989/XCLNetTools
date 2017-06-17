/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 方法返回值实体，主要是方便一个方法输出多个信息时使用，同时也减少使用out返回结果信息
    /// </summary>
    /// <typeparam name="TResult">方法返回的结果类型</typeparam>
    [Serializable]
    public class MethodResult<TResult>
    {
        private bool _isSuccess = true;
        private TResult _result = default(TResult);

        /// <summary>
        /// 该方法执行的逻辑是否成功（默认为true）
        /// （如果TResult与IsSuccess属性均为bool，则Result与IsSuccess一致）
        /// </summary>
        public bool IsSuccess
        {
            get { return this._isSuccess; }
            set
            {
                this._isSuccess = value;
                if (this._result is bool)
                {
                    this._result = (TResult)(object)value;
                }
            }
        }

        /// <summary>
        /// 该方法返回的消息提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 该方法返回的错误消息提示
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 该方法返回的结果
        /// （如果TResult与IsSuccess属性均为bool，则Result与IsSuccess一致）
        /// </summary>
        public TResult Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
                if (this._result is bool)
                {
                    this._isSuccess = (bool)(object)value;
                }
            }
        }

        /// <summary>
        /// 其它数据（比如用dictionary存放不同的数据结果）
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// 方法返回值实体，主要是方便一个方法输出多个信息时使用，同时也减少使用out返回结果信息
    /// </summary>
    /// <typeparam name="TResult">方法返回的结果类型</typeparam>
    /// <typeparam name="TData">Data属性的类型</typeparam>
    [Serializable]
    public class MethodResult<TResult, TData>
    {
        private bool _isSuccess = true;
        private TResult _result = default(TResult);

        /// <summary>
        /// 该方法执行的逻辑是否成功（默认为true）
        /// （如果TResult与IsSuccess属性均为bool，则Result与IsSuccess一致）
        /// </summary>
        public bool IsSuccess
        {
            get { return this._isSuccess; }
            set
            {
                this._isSuccess = value;
                if (this._result is bool)
                {
                    this._result = (TResult)(object)value;
                }
            }
        }

        /// <summary>
        /// 该方法返回的消息提示
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 该方法返回的错误消息提示
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 该方法返回的结果
        /// （如果TResult与IsSuccess属性均为bool，则Result与IsSuccess一致）
        /// </summary>
        public TResult Result
        {
            get
            {
                return this._result;
            }
            set
            {
                this._result = value;
                if (this._result is bool)
                {
                    this._isSuccess = (bool)(object)value;
                }
            }
        }

        /// <summary>
        /// 其它数据（比如用dictionary存放不同的数据结果）
        /// </summary>
        public TData Data { get; set; }
    }
}