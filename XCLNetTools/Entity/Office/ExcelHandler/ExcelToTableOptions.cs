using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// Excel 转 DataTable 时的参数选项
    /// </summary>
    [Serializable]
    public class ExcelToTableOptions
    {
        /// <summary>
        /// 是否需要将第一行自动转换为 DataTable 的列名
        /// </summary>
        public bool IsConvertFirstRowToColumnName { get; set; }

        /// <summary>
        /// 是否只加载可见行（注意：如果只加载可见行，最终 DataTable 的行数还是总行数，只是隐藏行的所有单元格的内容是 null）
        /// </summary>
        public bool IsOnlyVisibleRows { get; set; }

        /// <summary>
        /// 是否自动忽略 DataTable 为空的行（此选项可以解决 IsOnlyVisibleRows 带来的问题 ）
        /// </summary>
        public bool IgnoreEmptyDataRows { get; set; }
    }
}