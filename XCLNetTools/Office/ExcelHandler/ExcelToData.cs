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
        /// 将 WorkSheet 转换为 DataTable
        /// </summary>
        public static DataTable WorkSheetToDataTable(Worksheet worksheet, ExcelToTableOptions excelToTableOptions)
        {
            var dt = new DataTable();
            if (null == worksheet)
            {
                return dt;
            }
            if (null == excelToTableOptions)
            {
                excelToTableOptions = new ExcelToTableOptions();
            }
            var options = new ExportTableOptions();
            options.ExportColumnName = excelToTableOptions.IsConvertFirstRowToColumnName;
            options.ExportAsString = true;
            options.PlotVisibleRows = excelToTableOptions.IsOnlyVisibleRows;
            var displayRange = worksheet.Cells.MaxDisplayRange;
            var readTotalCount = displayRange.RowCount - excelToTableOptions.StartRowIndex;
            if (readTotalCount <= 0)
            {
                return dt;
            }
            dt = worksheet.Cells.ExportDataTable(excelToTableOptions.StartRowIndex, 0, readTotalCount, displayRange.ColumnCount, options);
            dt.TableName = worksheet.Name;
            if (excelToTableOptions.IgnoreEmptyDataRows)
            {
                for (var i = dt.Rows.Count - 1; i >= 0; i--)
                {
                    if (dt.Rows[i].ItemArray.All(k => DBNull.Value == k || string.IsNullOrWhiteSpace(Convert.ToString(k))))
                    {
                        dt.Rows.RemoveAt(i);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// 单个工作薄读入（第一个可见的sheet）
        /// </summary>
        public static DataTable ReadExcelToTable(string path, ExcelToTableOptions excelToTableOptions = null)
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
            return WorkSheetToDataTable(worksheet, excelToTableOptions);
        }

        /// <summary>
        /// 多个工作薄读入（所有可见的sheet）
        /// </summary>
        public static DataSet ReadExcelToDataSet(string path, ExcelToTableOptions excelToTableOptions = null)
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
                var dt = WorkSheetToDataTable(worksheet, excelToTableOptions);
                if (null == dt || dt.Rows.Count == 0)
                {
                    continue;
                }
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

                var displayRange = worksheet.Cells.MaxDisplayRange;
                int firstRow = titleRowNumber - 1;
                int firstColumn = 0;
                int totalRows = displayRange.RowCount;
                int totalColumns = displayRange.ColumnCount;
                if (totalColumns == 0 || totalRows == 0)
                {
                    continue;
                }

                var tb = worksheet.Cells.ExportDataTableAsString(firstRow, firstColumn, totalRows, totalColumns);
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