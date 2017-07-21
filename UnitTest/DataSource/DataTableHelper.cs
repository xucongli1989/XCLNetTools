using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace UnitTest.DataSource
{
    [TestClass]
    public class DataTableHelper
    {
        [TestMethod]
        public void ToDataTable()
        {
            var lst = new List<TestEntity.UserModel>();
            var model = new TestEntity.UserModel();
            model.ID = 100;
            model.Age = null;
            model.Name = "Test";
            lst.Add(model);
            var dt = XCLNetTools.DataSource.DataTableHelper.ToDataTable(lst);
            Assert.IsTrue(null != dt && dt.Rows.Count == 1 && dt.Rows[0]["ID"].ToString() == "100" && dt.Rows[0]["Age"] == DBNull.Value && dt.Rows[0]["Name"].ToString() == "Test");
        }
    }
}