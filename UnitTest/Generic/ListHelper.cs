using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;

namespace UnitTest.Generic
{
    [TestClass]
    public class ListHelper
    {
        [TestMethod]
        public void DataTableToList()
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            var dr = dt.NewRow();
            dr["ID"] = 100;
            dr["Age"] = DBNull.Value;
            dr["Name"] = "Test";
            dt.Rows.Add(dr);
            var lst = XCLNetTools.Generic.ListHelper.DataTableToList<TestEntity.UserModel>(dt);
            Assert.IsTrue(null != lst && lst.Count == 1 && lst[0].ID == 100 && lst[0].Age == null && lst[0].Name == "Test");
        }

        [TestMethod]
        public void DataRowToModel()
        {
            var dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Age", typeof(int));
            dt.Columns.Add("Name", typeof(string));

            var dr = dt.NewRow();
            dr["ID"] = 100;
            dr["Age"] = DBNull.Value;
            dr["Name"] = "Test";

            var model = XCLNetTools.Generic.ListHelper.DataRowToModel<TestEntity.UserModel>(dr);
            Assert.IsTrue(null != model && model.Age == null && model.ID == 100 && model.Name == "Test");
        }
    }
}