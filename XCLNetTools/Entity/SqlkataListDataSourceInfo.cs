using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    [Serializable]
    public class SqlkataListDataSourceInfo
    {
        public string[] ColumnNames { get; set; }
        public object[][] RowValues { get; set; }
    }
}