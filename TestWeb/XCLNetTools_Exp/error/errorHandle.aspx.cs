using System;
using System.Web;
using XCLNetTools.Message;

namespace TestWeb.XCLNetTools_Exp.error
{
    public partial class errorHandle : System.Web.UI.Page
    {
        private XCLNetTools.Message.Log log = new XCLNetTools.Message.Log();
        protected string type = "", method = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.type = Request.Params["type"] ?? "";
            this.method = Request.Params["method"] ?? "";
            if (string.Equals(this.method, "ajax"))
            {
                this.AjaxMethod();
            }
        }

        /// <summary>
        /// ajax json输入自定义消息
        /// </summary>
        private void AjaxMethod()
        {
            switch (type)
            {
                case "add":
                    XCLNetTools.Message.Log.WriteMessage("添加成功！");
                    break;

                case "update":
                    string date = Request.Params["date"] ?? "";
                    if (string.IsNullOrEmpty(date))
                    {
                        XCLNetTools.Message.Log.WriteMessage("时间不能为空！");
                    }
                    DateTime dt = Convert.ToDateTime(date);//转换失败则引发系统异常！
                    XCLNetTools.Message.Log.WriteMessage("更新成功！");
                    break;
            }
        }

        /// <summary>
        /// 使用默认输出异常信息
        /// </summary>
        public void Btn_ErrorClick(object sender, EventArgs e)
        {
            int a = 1, b = 0;
            int c = a / b;
        }

        /// <summary>
        /// 自定义输出异常信息
        /// </summary>
        public void Btn_ErrorCustomClick(object sender, EventArgs e)
        {
            XCLNetTools.Message.Log.LogApplicationErrorAction = new Action<MessageModel>((model) =>
            {
                var context = HttpContext.Current;
                context.Response.Clear();
                context.Response.Write(model.Message);
                context.Response.End();
            });

            int a = 1, b = 0;
            int c = a / b;
        }
    }
}