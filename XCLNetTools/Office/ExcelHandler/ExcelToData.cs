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
        /// <param name="excelfilePath">Excel 文件路径</param>
        /// <param name="isConvertFirstRowToColumnName">是否需要将第一行自动转换为列名</param>
        public static DataTable ReadExcelToTable(string excelfilePath, bool isConvertFirstRowToColumnName = false)
        {
            DataTable dataTable = new DataTable();
            if (!System.IO.File.Exists(excelfilePath))
            {
                return dataTable;
            }

            Workbook workbook = new Workbook(excelfilePath);
            Worksheet worksheet = null;
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                worksheet = workbook.Worksheets[i];
                if (worksheet.IsVisible)
                {
                    break;
                }
            }
            if (null != worksheet && worksheet.Cells.MaxRow > -1 && worksheet.Cells.MaxColumn > -1)
            {
                dataTable = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, isConvertFirstRowToColumnName);
            }
            return dataTable;
        }

        /// <summary>
        /// 将多个工作薄导入到DS中（所有可见的sheet）
        /// </summary>
        /// <param name="excelfilePath">Excel 文件路径</param>
        /// <param name="isConvertFirstRowToColumnName">是否需要将第一行自动转换为列名</param>
        public static DataSet ReadExcelToDataSet(string excelfilePath, bool isConvertFirstRowToColumnName = false)
        {
            DataSet ds = new DataSet();
            if (!System.IO.File.Exists(excelfilePath))
            {
                return ds;
            }
            Workbook workbook = new Workbook(excelfilePath);
            Worksheet worksheet = null;
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                worksheet = workbook.Worksheets[i];
                if (worksheet.IsVisible && worksheet.Cells.MaxRow > -1 && worksheet.Cells.MaxColumn > -1)
                {
                    var dataTable = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1, isConvertFirstRowToColumnName);
                    dataTable.TableName = worksheet.Name;
                    ds.Tables.Add(dataTable);
                }
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