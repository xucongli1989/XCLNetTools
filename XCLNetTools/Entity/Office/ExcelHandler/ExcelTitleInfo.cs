using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// Excel 的标题信息
    /// </summary>
    [Serializable]
    public class ExcelTitleInfo
    {
        /// <summary>
        /// sheet 索引，0 为第一个 sheet
        /// </summary>
        public int SheetIndex { get; set; }

        /// <summary>
        /// sheet 名称
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 所有列的标题
        /// </summary>
        public List<string> TitleNameList { get; set; }
    }
}