/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace XCLNetTools.MVC
{
    /// <summary>
    /// 带时间格式的jsonResult
    /// </summary>
    public class JsonResultFormat : JsonResult
    {
        private string _dateFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// 格式化时间，默认值为"yyyy-MM-dd HH:mm:ss"
        /// </summary>
        public string DateFormat
        {
            get { return this._dateFormat; }
            set { this._dateFormat = value; }
        }

        /// <summary>
        /// 重写执行视图
        /// </summary>
        /// <param name="context">上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (string.IsNullOrEmpty(this.DateFormat))
            {
                base.ExecuteResult(context);
                return;
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("该请求未被允许！");
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                response.Write(JsonConvert.SerializeObject(this.Data, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings()
                {
                    DateFormatString = this.DateFormat
                }));
            }
        }
    }
}