using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// Excel 数据行数信息
    /// </summary>
    [Serializable]
    public class ExcelDataCountInfoEntity
    {
        /// <summary>
        /// 总行数
        /// </summary>
        public int RowCount { get; set; }

        /// <summary>
        /// 总列数
        /// </summary>
        public int ColumnCount { get; set; }
    }
}