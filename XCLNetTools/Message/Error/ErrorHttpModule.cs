/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Text;
using System.Web;

namespace XCLNetTools.Message.Error
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ErrorHttpModule : IHttpModule
    {
        private HttpApplication app = null;

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(HttpApplication context)
        {
            context.Error += new EventHandler(Application_Error);
            this.app = context;
        }

        /// <summary>
        /// 错误
        /// </summary>
        private void Application_Error(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (null == context)
            {
                return;
            }

            string msg = string.Format("错误页：{0}\r\n来源URL：{1}\r\n", context.Request.Url, context.Request.UrlReferrer);
            Exception exp = context.Error.GetBaseException();

            MessageModel errModel = new MessageModel();

            errModel.Title = "系统提示";
            errModel.Date = DateTime.Now;
            errModel.Message = exp.Message;
            errModel.Url = Convert.ToString(context.Request.Url);
            errModel.FromUrl = Convert.ToString(context.Request.UrlReferrer);
            errModel.IsSuccess = false;
            errModel.Remark = "所捕捉的Exception异常信息";

            StringBuilder strMore = new StringBuilder();
            //var stack = new StackTrace(exp, true);
            //if (stack.FrameCount > 0)
            //{
            //    strMore.Append(stack.GetFrame(0).ToString());
            //}
            strMore.AppendFormat("跟踪：{0}", exp.StackTrace);
            errModel.MessageMore = strMore.ToString();

            var httpExp = exp as HttpException;
            if (null != httpExp)
            {
                errModel.ErrorCode = Convert.ToString(httpExp.GetHttpCode());
            }

            XCLNetTools.Message.Log.LogApplicationErrorAction.Invoke(errModel);

            context.Server.ClearError();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (null != this.app)
            {
                this.app.Error -= new EventHandler(Application_Error);
                this.app = null;
            }
        }
    }
}