using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XCLNetTools.Entity.Office.ExcelHandler;

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

        /// <summary>
        /// Excel 数据数量范围类型枚举
        /// </summary>
        public enum ExcelDataCountInfoRangeTypeEnum
        {
            /// <summary>
            /// 所有范围（包含：数据、样式、形状）
            /// </summary>
            all_range,

            /// <summary>
            /// 仅数据范围（只包含有数据的区域，不包含样式、形状）
            /// </summary>
            only_data_range,
        }

        /// <summary>
        /// 返回工作表中的数据行数和列数
        /// </summary>
        public static ExcelDataCountInfoEntity GetDataCountInfo(Aspose.Cells.Worksheet sheet, ExcelDataCountInfoRangeTypeEnum rangeType)
        {
            var cells = sheet.Cells;
            var result = new ExcelDataCountInfoEntity();
            if (null == cells)
            {
                return result;
            }
            if (rangeType == ExcelDataCountInfoRangeTypeEnum.only_data_range)
            {
                result.RowCount = cells.MaxDataRow + 1;
                result.ColumnCount = cells.MaxDataColumn + 1;
            }
            else if (rangeType == ExcelDataCountInfoRangeTypeEnum.all_range)
            {
                var range = cells.MaxDisplayRange;
                if (null != range)
                {
                    result.RowCount = range.RowCount;
                    result.ColumnCount = range.ColumnCount;
                }
            }
            return result;
        }
    }
}