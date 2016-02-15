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

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 方法返回值实体，主要是方便一个方法输出多个信息时使用，同时也减少使用out返回结果信息
    /// <typeparam name="T">方法返回的结果类型</typeparam>
    /// </summary>
    [Serializable]
    public class MethodResult<T>
    {
        private bool _isSuccess = true;
        private T _result = default(T);

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
        public T Result
        {
            get
            {
                if (this._result is bool)
                {
                    throw new Exception("当Result为bool类型时，请读取IsSuccess属性即可！");
                }
                return this._result;
            }
            set
            {
                if (this._result is bool)
                {
                    throw new Exception("当Result为bool类型时，请设置IsSuccess属性即可！");
                }
                this._result = value;
            }
        }

        /// <summary>
        /// 其它数据（比如用dictionary存放不同的数据结果）
        /// </summary>
        public object Data { get; set; }
    }
}