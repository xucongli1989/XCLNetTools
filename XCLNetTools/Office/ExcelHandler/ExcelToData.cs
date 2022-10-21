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
using System.Text;
using XCLNetTools.Entity.Office.ExcelHandler;

namespace XCLNetTools.Office.ExcelHandler
{
    /// <summary>
    /// excel读取类
    /// </summary>
    public static class ExcelToData
    {
        /// <summary>
        /// 根据路径返回 Workbook 对象（如果是 csv，则自动识别 csv 编码）
        /// </summary>
        public static Workbook GetWorkbook(string path)
        {
            if (XCLNetTools.FileHandler.ComFile.GetExtName(path) == "csv")
            {
                var fileEncode = XCLNetTools.FileHandler.ComFile.GetFileEncoding(path);
                return new Workbook(path, new Aspose.Cells.TxtLoadOptions()
                {
                    Encoding = fileEncode
                });
            }
            return new Workbook(path);
        }

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
            if (null == displayRange)
            {
                return dt;
            }
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

            var workbook = GetWorkbook(path);
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
            var workbook = GetWorkbook(path);
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
        /// <param name="excelfilePath">文件路径</param>
        /// <param name="titleRowNumber">标题所在行号</param>
        /// <param name="isIgnoreTailEmptyTitleColumn">是否忽略尾部没有标题名称的列，因为有些文件在读取时的列数为 excel 的最大列数，而实际上只有前几列有用</param>
        public static List<ExcelTitleInfo> GetExcelTitleInfoList(string excelfilePath, int titleRowNumber = 1, bool isIgnoreTailEmptyTitleColumn = true)
        {
            var result = new List<ExcelTitleInfo>();
            if (!System.IO.File.Exists(excelfilePath))
            {
                return result;
            }

            var workbook = GetWorkbook(excelfilePath);
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

                //没有数据
                if (worksheet.Cells.MaxDataRow == -1)
                {
                    continue;
                }

                var displayRange = worksheet.Cells.MaxDisplayRange;
                if (null == displayRange)
                {
                    continue;
                }

                int firstRow = titleRowNumber - 1;
                int firstColumn = 0;
                int totalColumns = displayRange.ColumnCount;
                var tb = worksheet.Cells.ExportDataTableAsString(firstRow, firstColumn, 1, totalColumns);
                if (null == tb || tb.Rows.Count == 0)
                {
                    continue;
                }
                for (var i = 0; i < tb.Columns.Count; i++)
                {
                    item.TitleNameList.Add(Convert.ToString(tb.Rows[0][i]));
                }
            }

            if (isIgnoreTailEmptyTitleColumn)
            {
                result.ForEach(k =>
                {
                    k.TitleNameList = XCLNetTools.StringHander.Common.TrimEnd(k.TitleNameList);
                });
            }

            return result;
        }
    }
}