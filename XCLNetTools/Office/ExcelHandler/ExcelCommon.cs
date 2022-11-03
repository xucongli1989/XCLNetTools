using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XCLNetTools.Office.ExcelHandler
{
    /// <summary>
    /// Excel 公共帮助类
    /// </summary>
    public static class ExcelCommon
    {
        /// <summary>
        /// 将字符串转换为有效的 Excel 工作表名称
        /// 1、1<=长度<=31
        /// 2、不能包含这此字符【:\/?*[]】
        /// </summary>
        public static string ConvertToExcelSheetName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Sheet";
            }
            name = new Regex(@"[:\\/?*\[\]]").Replace(name, "");
            if (name.Length >= 32)
            {
                name = name.Substring(0, 31);
            }
            return name;
        }
    }
}