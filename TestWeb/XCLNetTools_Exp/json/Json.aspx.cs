using System;
using System.Data;

namespace TestWeb.XCLNetTools_Exp.json
{
    public partial class Json : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s =XCLNetTools.Serialize.LitJson.ConvertDataTableToJson(MyTable(), "sb");
            XCLNetTools.StringHander.Common.ResponseClearWrite(s);
        }

        private static DataTable MyTable()
        {
            DataTable dt = new DataTable();

            DataColumn dc = new DataColumn();

            dc.ColumnName = "ID";

            dc.DataType = typeof(int);

            dt.Columns.Add(dc);

            dc = new DataColumn();

            dc.ColumnName = "Name";

            dc.DataType = typeof(String);

            dt.Columns.Add(dc);

            dc = new DataColumn();

            dc.ColumnName = "Score";

            dc.DataType = typeof(String);

            dt.Columns.Add(dc);

            DataRow dr = dt.NewRow();

            dr["ID"] = DBNull.Value;

            dr["Name"] = "Jin";

            dr["Score"] = null;

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "01";

            dr["Name"] = "Xin";

            dr["Score"] = "20";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "01";

            dr["Name"] = "Xu";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Xin";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Jin";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Jin";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Jin";

            dr["Score"] = "40";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Jin";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            dr = dt.NewRow();

            dr["ID"] = "02";

            dr["Name"] = "Jin";

            dr["Score"] = "30";

            dt.Rows.Add(dr);

            return dt;
        }
    }
}