/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XCLNetTools.Entity.Office.ExcelHandler;

namespace XCLNetTools.Office.ExcelHandler
{
    /// <summary>
    /// excel读取类
    /// </summary>
    public static class ExcelToData
    {
        /// <summary>
        /// 单个工作薄读入（第一个可见的sheet）
        /// </summary>
        /// <param name="path">Excel 文件路径</param>
        /// <param name="isConvertFirstRowToColumnName">是否需要将第一行自动转换为 DataTable 的列名</param>
        /// <param name="isOnlyVisibleRows">是否只加载可见行（注意：如果只加载可见行，最终 dataTable 的行数还是总行数，只是隐藏行的所有单元格的内容是 null）</param>
        public static DataTable ReadExcelToTable(string path, bool isConvertFirstRowToColumnName = false, bool isOnlyVisibleRows = false)
        {
            var dt = new DataTable();
            if (!System.IO.File.Exists(path))
            {
                return dt;
            }

            var workbook = new Workbook(path);
            Worksheet worksheet = null;
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                worksheet = workbook.Worksheets[i];
                if (worksheet.IsVisible)
                {
                    break;
                }
            }
            if (null == worksheet)
            {
                return dt;
            }
            var displayRange = worksheet.Cells.MaxDisplayRange;
            if (displayRange.RowCount <= 0)
            {
                return dt;
            }
            var options = new ExportTableOptions();
            options.ExportColumnName = isConvertFirstRowToColumnName;
            options.ExportAsString = true;
            options.PlotVisibleRows = isOnlyVisibleRows;
            dt = worksheet.Cells.ExportDataTable(0, 0, displayRange.RowCount, displayRange.ColumnCount, options);
            return dt;
        }

        /// <summary>
        /// 多个工作薄读入（所有可见的sheet）
        /// </summary>
        /// <param name="path">Excel 文件路径</param>
        /// <param name="isConvertFirstRowToColumnName">是否需要将第一行自动转换为 DataTable 的列名</param>
        /// <param name="isOnlyVisibleRows">是否只加载可见行（注意：如果只加载可见行，最终 dataTable 的行数还是总行数，只是隐藏行的所有单元格的内容是 null）</param>
        public static DataSet ReadExcelToDataSet(string path, bool isConvertFirstRowToColumnName = false, bool isOnlyVisibleRows = false)
        {
            var ds = new DataSet();
            if (!System.IO.File.Exists(path))
            {
                return ds;
            }
            var workbook = new Workbook(path);
            Worksheet worksheet = null;
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                worksheet = workbook.Worksheets[i];
                if (!worksheet.IsVisible)
                {
                    continue;
                }
                var displayRange = worksheet.Cells.MaxDisplayRange;
                if (displayRange.RowCount <= 0)
                {
                    continue;
                }
                var options = new ExportTableOptions();
                options.ExportColumnName = isConvertFirstRowToColumnName;
                options.ExportAsString = true;
                options.PlotVisibleRows = isOnlyVisibleRows;
                var dt = worksheet.Cells.ExportDataTable(0, 0, displayRange.RowCount, displayRange.ColumnCount, options);
                dt.TableName = worksheet.Name;
                ds.Tables.Add(dt);
            }
            return ds;
        }

        /// <summary>
        /// 读取 Excel 所有可见的 sheet 的所有标题信息
        /// </summary>
        public static List<ExcelTitleInfo> GetExcelTitleInfoList(string excelfilePath, int titleRowNumber = 1)
        {
            var result = new List<ExcelTitleInfo>();
            if (!System.IO.File.Exists(excelfilePath))
            {
                return result;
            }

            var workbook = new Workbook(excelfilePath);
            for (var sheetIndex = 0; sheetIndex < workbook.Worksheets.Count; sheetIndex++)
            {
                var worksheet = workbook.Worksheets[sheetIndex];
                if (!worksheet.IsVisible)
                {
                    continue;
                }
                var item = new ExcelTitleInfo();
                item.SheetIndex = sheetIndex;
                item.SheetName = worksheet.Name;
                item.TitleNameList = new List<string>();
                result.Add(item);

                var tb = worksheet.Cells.ExportDataTableAsString(titleRowNumber - 1, 0, 1, worksheet.Cells.MaxColumn + 1);
                if (null == tb || tb.Rows.Count == 0)
                {
                    continue;
                }
                for (var i = 0; i < tb.Columns.Count; i++)
                {
                    item.TitleNameList.Add(Convert.ToString(tb.Rows[0][i]));
                }
            }

            return result;
        }
    }
}