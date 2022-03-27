using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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

        [TestMethod]
        public void GetRange()
        {
            var lst = new List<int>() { 1, 2, 3 };
            var result = XCLNetTools.Generic.ListHelper.GetRange(lst, -1, 1);
            Assert.IsTrue(result.Count == 0);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 0, -1);
            Assert.IsTrue(result.Count == 0);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 0, 2);
            Assert.IsTrue(result.Count == 2 && result[0] == 1 && result[1] == 2);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 0, 100);
            Assert.IsTrue(result.Count == 3 && result[0] == 1 && result[1] == 2 && result[2] == 3);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 2, 2);
            Assert.IsTrue(result.Count == 1 && result[0] == 3);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 4, 2);
            Assert.IsTrue(result.Count == 0);

            result = XCLNetTools.Generic.ListHelper.GetRange(lst, 2, 0);
            Assert.IsTrue(result.Count == 0);
        }
    }
}