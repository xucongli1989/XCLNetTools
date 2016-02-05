/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.1
更新时间：2016-01-01

四：更新内容：
1：将原先基础数据转换方法转移到Common/DataTypeConvert下面
2：其它逻辑优化，如表单参数获取等
3：首次开放所有源代码
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