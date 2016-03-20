/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Web;

namespace XCLNetTools.Message
{
    /// <summary>
    /// 消息日志
    /// </summary>
    public class Log
    {
        /// <summary>
        /// 以json方式提示的属性名,它的下面有多个成员（如：data.JsonMessageName.Message）
        /// </summary>
        public static string JsonMessageName = string.Format("XCL{0}", XCLNetTools.StringHander.RandomHelper.GenerateIdWithGuid());

        /// <summary>
        /// 记录application error的处理方法,默认直接输出json
        /// </summary>
        public static Action<MessageModel> LogApplicationErrorAction = new Action<MessageModel>((msgModel) => { XCLNetTools.Message.Log.WriteMessage(msgModel); });

        /// <summary>
        /// 直接输出obj的json形式
        /// </summary>
        /// <param name="obj">要输出的对象</param>
        public static void WriteMessage(object obj)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            context.Response.Write(XCLNetTools.Serialize.JSON.Serialize(obj));
            context.Response.End();
        }

        /// <summary>
        /// 直接输出MessageModel的JSON形式（此JSON作为Log.JsonMessageName的一个属性）
        /// </summary>
        /// <param name="model">消息对象</param>
        public static void WriteMessage(MessageModel model)
        {
            HttpContext context = HttpContext.Current;
            context.Response.Clear();
            string msg = XCLNetTools.Serialize.JSON.Serialize(model);
            context.Response.Write(string.Format(@"{{ ""{0}"":{1} }}", Log.JsonMessageName, msg));
            context.Response.End();
        }

        /// <summary>
        /// 输出消息（json）
        /// MessageModel
        /// </summary>
        /// <param name="message">消息</param>
        public static void WriteMessage(string message)
        {
            WriteMessage(new MessageModel()
            {
                Date = DateTime.Now,
                IsSuccess = false,
                Message = message,
                Title = "系统提示",
                Remark = "自定义输出信息（直接输出）"
            });
        }
    }
}