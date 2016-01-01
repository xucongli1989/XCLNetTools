﻿using Aspose.Cells;
using System.Data;

namespace XCLNetTools.DataHandler
{
    /// <summary>
    /// excel读取类
    /// </summary>
    public class ExcelToData
    {
        /// <summary>
        /// 单个工作薄读入（第一个可见的sheet）
        /// <param name="excelfilePath">文件路径</param>
        /// </summary>
        public static DataTable ReadExcelToTable(string excelfilePath)
        {
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
            DataTable dataTable = new DataTable();
            if (worksheet.Cells.MaxRow > -1 && worksheet.Cells.MaxColumn > -1)
            {
                dataTable = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1);
            }
            return dataTable;
        }

        /// <summary>
        /// 将多个工作薄导入到DS中（所有可见的sheet）
        /// <param name="excelfilePath">文件路径</param>
        /// </summary>
        public static DataSet ReadExcelToDataSet(string excelfilePath)
        {
            DataSet ds = new DataSet();
            Workbook workbook = new Workbook(excelfilePath);
            Worksheet worksheet = null;
            for (int i = 0; i < workbook.Worksheets.Count; i++)
            {
                worksheet = workbook.Worksheets[i];
                if (worksheet.IsVisible && worksheet.Cells.MaxRow > -1 && worksheet.Cells.MaxColumn > -1)
                {
                    DataTable dataTable = new DataTable();
                    dataTable = worksheet.Cells.ExportDataTableAsString(0, 0, worksheet.Cells.MaxRow + 1, worksheet.Cells.MaxColumn + 1);
                    dataTable.TableName = string.Format("dt{0}", i);
                    ds.Tables.Add(dataTable);
                }
            }
            return ds;
        }
    }
}