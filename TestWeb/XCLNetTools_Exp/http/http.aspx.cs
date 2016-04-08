using System;

namespace TestWeb.XCLNetTools_Exp.http
{
    public partial class http : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s = XCLNetTools.Common.IPHelper.GetIpByIP138();
            this.ltMsg.Text = s;
        }
    }
}