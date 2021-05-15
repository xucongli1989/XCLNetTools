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
        /// 读取 Excel 所有可见的 sheet 的所有标题信息（返回结果的字典key为 sheet 名称，value 为列名称）
        /// </summary>
        public static Dictionary<string, List<string>> GetExcelTitleInfo(string excelfilePath, int titleRowNumber = 1)
        {
            var dic = new Dictionary<string, List<string>>();
            if (!System.IO.File.Exists(excelfilePath))
            {
                return dic;
            }
            var workbook = new Workbook(excelfilePath);
            foreach (Worksheet worksheet in workbook.Worksheets)
            {
                if (!worksheet.IsVisible)
                {
                    continue;
                }
                var tb = worksheet.Cells.ExportDataTableAsString(titleRowNumber - 1, 0, 1, worksheet.Cells.MaxColumn + 1);
                if (null == tb || tb.Rows.Count == 0)
                {
                    continue;
                }
                var nameList = new List<string>();
                for (var i = 0; i < tb.Columns.Count; i++)
                {
                    nameList.Add(Convert.ToString(tb.Rows[0][i]));
                }
                dic.Add(worksheet.Name, nameList);
            }
            return dic;
        }
    }
}