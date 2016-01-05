using System;
using System.Data;

namespace TestWeb.XCLNetTools_Exp.DataHandler
{
    public partial class OutExcel : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("编号", typeof(int));
            dt.Columns.Add("姓名", typeof(string));
            dt.Columns.Add("年龄", typeof(int));
            dt.Columns.Add("备注", typeof(string));
            for (int i = 0; i < 100; i++)
            {
                DataRow dr = dt.NewRow();
                dr["编号"] = i;
                dr["姓名"] = "Name" + i;
                dr["年龄"] = 21 + i;
                dr["备注"] = "Remark" + i;
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            DataTable dt2 = dt.Copy();
            dt2.TableName = "dt2";
            ds.Tables.Add(dt);
            ds.Tables.Add(dt2);

            XCLNetTools.DataHandler.DataToExcel.OutPutExcel(new XCLNetTools.Entity.DataHandler.OutPutParamClass()
            {
                tableName = null,
                outPutClass = null,
                conTitle = new string[] { "用户信息表", "所有用户" },
                CustomFileName = "导出的文件.xlsx",
                ds = ds,
                fileTitle = "文件导出",
            });
        }
    }
}