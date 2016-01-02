/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.1
更新时间：2016-01-01

四：更新内容：
1：将原先基础数据转换方法转移到Common/DataTypeConvert下面
2：其它逻辑优化，如表单参数获取等
3：首次开放所有源代码
 */



using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace XCLNetTools.DataHandler
{
    /// <summary>
    /// 旧版的excel操作（基于excel.dll），建议不要使用此类（用aspose.cells）
    /// </summary>
    public class ExcelHelper
    {
        #region 成员变量

        private string templetFile = null;
        private string outputFile = null;
        private object missing = Missing.Value;
        private DateTime beforeTime;   //Excel启动之前时间
        private DateTime afterTime;    //Excel启动之后时间
        private Excel.Application app;
        private Excel.Workbook workBook;
        private Excel.Worksheet workSheet;
        private Excel.Range range;
        private Excel.Range range1;
        private Excel.Range range2;
        private Excel.TextBox textBox;
        private int sheetCount = 1;   //WorkSheet数量
        private string sheetPrefixName = "页";

        #endregion 成员变量

        #region 公共属性

        /// <summary>
        /// WorkSheet前缀名，比如：前缀名为“页”，那么WorkSheet名称依次为“页-1，页-2...”
        /// </summary>
        public string SheetPrefixName
        {
            set { this.sheetPrefixName = value; }
        }

        /// <summary>
        /// WorkSheet数量
        /// </summary>
        public int WorkSheetCount
        {
            get { return workBook.Sheets.Count; }
        }

        /// <summary>
        /// Excel模板文件路径
        /// </summary>
        public string TempletFilePath
        {
            set { this.templetFile = value; }
        }

        /// <summary>
        /// 输出Excel文件路径
        /// </summary>
        public string OutputFilePath
        {
            set { this.outputFile = value; }
        }

        #endregion 公共属性

        #region 公共方法

        #region ExcelHelper

        /// <summary>
        /// 构造函数，将一个已有Excel工作簿作为模板，并指定输出路径
        /// </summary>
        /// <param name="templetFilePath">Excel模板文件路径</param>
        /// <param name="outputFilePath">输出Excel文件路径</param>
        public ExcelHelper(string templetFilePath, string outputFilePath)
        {
            if (templetFilePath == null)
                throw new Exception("Excel模板文件路径不能为空！");

            if (outputFilePath == null)
                throw new Exception("输出Excel文件路径不能为空！");

            if (!File.Exists(templetFilePath))
                throw new Exception("指定路径的Excel模板文件不存在！");

            this.templetFile = templetFilePath;
            this.outputFile = outputFilePath;

            //创建一个Application对象并使其可见
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            app.Visible = true;
            afterTime = DateTime.Now;

            //打开模板文件，得到WorkBook对象
            workBook = app.Workbooks.Open(templetFile, missing, missing, missing, missing, missing,
                missing, missing, missing, missing, missing, missing, missing);

            //得到WorkSheet对象
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);
        }

        /// <summary>
        /// 构造函数，打开一个已有的工作簿
        /// </summary>
        /// <param name="fileName">Excel文件名</param>
        public ExcelHelper(string fileName)
        {
            if (!File.Exists(fileName))
                throw new Exception("指定路径的Excel文件不存在！");

            //创建一个Application对象并使其可见
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            app.Visible = false;
            afterTime = DateTime.Now;

            //打开一个WorkBook
            workBook = app.Workbooks.Open(fileName,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            //得到WorkSheet对象
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);
        }

        /// <summary>
        /// 构造函数，新建一个工作簿
        /// </summary>
        public ExcelHelper()
        {
            //创建一个Application对象并使其可见
            beforeTime = DateTime.Now;
            app = new Excel.ApplicationClass();
            //app.Visible = true; 不自动打开
            afterTime = DateTime.Now;

            //新建一个WorkBook
            workBook = app.Workbooks.Add(Type.Missing);

            //得到WorkSheet对象
            workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(1);
        }

        #endregion ExcelHelper

        #region Data Export Methods

        /// <summary>
        /// 将DataTable数据写入Excel文件（自动分页）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left)
        {
            int rowCount = dt.Rows.Count;  //DataTable行数
            int colCount = dt.Columns.Count; //DataTable列数
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数
            //   StringBuilder sb;

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows;  //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将dt中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString();
                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value = ss;

                #region 利用Windwo粘贴板批量拷贝数据（在Web下面行不通）

                /*sb = new StringBuilder();

    for(int j=0;j<endRow-startRow;j++)
    {
     for(int k=0;k<colCount;k++)
     {
      sb.Append( dt.Rows[startRow + j][k].ToString() );
      sb.Append("/t");
     }

     sb.Append("/n");
    }

    System.Windows.Forms.Clipboard.SetDataObject(sb.ToString());

    range = (Excel.Range)workSheet.Cells[top,left];
    workSheet.Paste(range,false);*/

                #endregion 利用Windwo粘贴板批量拷贝数据（在Web下面行不通）
            }
        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        public void DataTableToExcel(DataTable dt, int top, int left)
        {
            int rowCount = dt.Rows.Count;  //DataTable行数
            int colCount = dt.Columns.Count; //DataTable列数

            //利用二维数组批量写入
            string[,] arr = new string[rowCount + 1, colCount + 1];
            for (int i = 0; i < colCount; i++)
            {
                arr[0, i] = dt.Columns[i].ColumnName.ToString();
            }

            for (int j = 0; j < rowCount; j++)
            {
                for (int k = 0; k < colCount; k++)
                {
                    arr[j + 1, k] = dt.Rows[j][k].ToString();
                }
            }

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount + 1, colCount + 1);
            range.Value = arr;
        }

        /// <summary>
        /// 将DataTable数据写入Excel文件（自动分页，并指定要合并的列索引）
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">表格数据起始行索引</param>
        /// <param name="left">表格数据起始列索引</param>
        /// <param name="mergeColumnIndex">DataTable中要合并相同行的列索引，从0开始</param>
        public void DataTableToExcel(DataTable dt, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = dt.Rows.Count;  //源DataTable行数
            int colCount = dt.Columns.Count; //源DataTable列数
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数
            //   StringBuilder sb;

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            for (int i = 1; i <= sheetCount; i++)
            {
                int startRow = (i - 1) * rows;  //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将dt中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = dt.Rows[startRow + j][k].ToString();
                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = dt.Rows[startRow + j][k].ToString();
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value = ss;

                //合并相同行
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);
            }
        }

        /// <summary>
        /// 将二维数组数据写入Excel文件（自动分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left)
        {
            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //将二维数组数据写入Excel
            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows;  //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将二维数组中的数据写入WorkSheet
                //    for(int j=0;j<endRow-startRow;j++)
                //    {
                //     for(int k=0;k<colCount;k++)
                //     {
                //      workSheet.Cells[top + j,left + k] = arr[startRow + j,k];
                //     }
                //    }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value = ss;
            }
        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(string[,] arr, int top, int left)
        {
            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);
            range.FormulaArray = arr;
        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        /// <param name="isFormula">填充的数据是否需要计算</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula)
        {
            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //注意：使用range.FormulaArray写合并的单元格会出问题
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value = arr;
        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页），合并指定列的相同行
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        /// <param name="isFormula">填充的数据是否需要计算</param>
        /// <param name="mergeColumnIndex">需要合并行的列索引</param>
        public void ArrayToExcel(string[,] arr, int top, int left, bool isFormula, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            //注意：使用range.FormulaArray写合并的单元格会出问题
            if (isFormula)
                range.FormulaArray = arr;
            else
                range.Value = arr;

            this.MergeRows(workSheet, mergeColumnIndex, top, rowCount);
        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（不分页）
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="arr">二维数组</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        public void ArrayToExcel(int sheetIndex, string[,] arr, int top, int left)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            // 改变当前工作表
            this.workSheet = (Excel.Worksheet)this.workBook.Sheets.get_Item(sheetIndex);

            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）

            range = (Excel.Range)workSheet.Cells[top, left];
            range = range.get_Resize(rowCount, colCount);

            range.Value2 = arr;
        }//end ArrayToExcel

        /// <summary>
        /// 将二维数组数据写入Excel文件（自动分页，并指定要合并的列索引）
        /// </summary>
        /// <param name="arr">二维数组</param>
        /// <param name="rows">每个WorkSheet写入多少行数据</param>
        /// <param name="top">行索引</param>
        /// <param name="left">列索引</param>
        /// <param name="mergeColumnIndex">数组的二维索引，相当于DataTable的列索引，索引从0开始</param>
        public void ArrayToExcel(string[,] arr, int rows, int top, int left, int mergeColumnIndex)
        {
            int rowCount = arr.GetLength(0);  //二维数组行数（一维长度）
            int colCount = arr.GetLength(1); //二维数据列数（二维长度）
            sheetCount = this.GetSheetCount(rowCount, rows); //WorkSheet个数

            //复制sheetCount-1个WorkSheet对象
            for (int i = 1; i < sheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Copy(missing, workBook.Worksheets[i]);
            }

            //将二维数组数据写入Excel
            for (int i = sheetCount; i >= 1; i--)
            {
                int startRow = (i - 1) * rows;  //记录起始行索引
                int endRow = i * rows;   //记录结束行索引

                //若是最后一个WorkSheet，那么记录结束行索引为源DataTable行数
                if (i == sheetCount)
                    endRow = rowCount;

                //获取要写入数据的WorkSheet对象，并重命名
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                workSheet.Name = sheetPrefixName + "-" + i.ToString();

                //将二维数组中的数据写入WorkSheet
                for (int j = 0; j < endRow - startRow; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        workSheet.Cells[top + j, left + k] = arr[startRow + j, k];
                    }
                }

                //利用二维数组批量写入
                int row = endRow - startRow;
                string[,] ss = new string[row, colCount];

                for (int j = 0; j < row; j++)
                {
                    for (int k = 0; k < colCount; k++)
                    {
                        ss[j, k] = arr[startRow + j, k];
                    }
                }

                range = (Excel.Range)workSheet.Cells[top, left];
                range = range.get_Resize(row, colCount);
                range.Value = ss;

                //合并相同行
                this.MergeRows(workSheet, left + mergeColumnIndex, top, rows);
            }
        }//end ArrayToExcel

        #endregion Data Export Methods

        #region WorkSheet Methods

        /// <summary>
        /// 改变当前工作表
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        public void ChangeCurrentWorkSheet(int sheetIndex)
        {
            //若指定工作表索引超出范围，则不改变当前工作表
            if (sheetIndex < 1)
                return;

            if (sheetIndex > this.WorkSheetCount)
                return;

            this.workSheet = (Excel.Worksheet)this.workBook.Sheets.get_Item(sheetIndex);
        }

        /// <summary>
        /// 隐藏指定名称的工作表
        /// </summary>
        /// <param name="sheetName">工作表名称</param>
        public void HiddenWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                        sheet = workSheet;
                }

                if (sheet != null)
                    sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为\"" + sheetName + "\"的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 隐藏指定索引的工作表
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void HiddenWorkSheet(int sheetIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                Excel.Worksheet sheet = null;
                sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);

                sheet.Visible = Excel.XlSheetVisibility.xlSheetHidden;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 在指定名称的工作表后面拷贝指定个数的该工作表的副本，并重命名
        /// </summary>
        /// <param name="sheetName">工作表名称</param>
        /// <param name="sheetCount">工作表个数</param>
        public void CopyWorkSheets(string sheetName, int sheetCount)
        {
            try
            {
                Excel.Worksheet sheet = null;
                int sheetIndex = 0;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;
                    }
                }

                if (sheet != null)
                {
                    for (int i = sheetCount; i >= 1; i--)
                    {
                        sheet.Copy(this.missing, sheet);
                    }

                    //重命名
                    for (int i = sheetIndex; i <= sheetIndex + sheetCount; i++)
                    {
                        workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);
                        workSheet.Name = sheetName + "-" + Convert.ToString(i - sheetIndex + 1);
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为\"" + sheetName + "\"的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 将一个工作表拷贝到另一个工作表后面，并重命名
        /// </summary>
        /// <param name="srcSheetIndex">拷贝源工作表索引</param>
        /// <param name="aimSheetIndex">参照位置工作表索引，新工作表拷贝在该工作表后面</param>
        /// <param name="newSheetName"></param>
        public void CopyWorkSheet(int srcSheetIndex, int aimSheetIndex, string newSheetName)
        {
            if (srcSheetIndex > this.WorkSheetCount || aimSheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                Excel.Worksheet srcSheet = (Excel.Worksheet)workBook.Sheets.get_Item(srcSheetIndex);
                Excel.Worksheet aimSheet = (Excel.Worksheet)workBook.Sheets.get_Item(aimSheetIndex);

                srcSheet.Copy(this.missing, aimSheet);

                //重命名
                workSheet = (Excel.Worksheet)aimSheet.Next;  //获取新拷贝的工作表
                workSheet.Name = newSheetName;
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 根据名称删除工作表
        /// </summary>
        /// <param name="sheetName"></param>
        public void DeleteWorkSheet(string sheetName)
        {
            try
            {
                Excel.Worksheet sheet = null;

                //找到名称位sheetName的工作表
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                    }
                }

                if (sheet != null)
                {
                    sheet.Delete();
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为\"" + sheetName + "\"的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 根据索引删除工作表
        /// </summary>
        /// <param name="sheetIndex"></param>
        public void DeleteWorkSheet(int sheetIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                Excel.Worksheet sheet = null;
                sheet = (Excel.Worksheet)workBook.Sheets.get_Item(sheetIndex);

                sheet.Delete();
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion WorkSheet Methods

        #region TextBox Methods

        /// <summary>
        /// 向指定文本框写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="textboxName">文本框名称</param>
        /// <param name="text">要写入的文本</param>
        public void SetTextBox(string textboxName, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                    textBox.Text = text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("不存在ID为\"" + textboxName + "\"的文本框！");
                }
            }
        }

        /// <summary>
        /// 向指定文本框写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="textboxName">文本框名称</param>
        /// <param name="text">要写入的文本</param>
        public void SetTextBox(int sheetIndex, string textboxName, string text)
        {
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            try
            {
                textBox = (Excel.TextBox)workSheet.TextBoxes(textboxName);
                textBox.Text = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("不存在ID为\"" + textboxName + "\"的文本框！");
            }
        }

        /// <summary>
        /// 向文本框写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存文本框的ID和数据</param>
        public void SetTextBoxes(Hashtable ht)
        {
            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                        textBox.Text = dic.Value.ToString();
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("不存在ID为\"" + dic.Key.ToString() + "\"的文本框！");
                    }
                }
            }
        }

        /// <summary>
        /// 向文本框写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存文本框的ID和数据</param>
        public void SetTextBoxes(int sheetIndex, Hashtable ht)
        {
            if (ht.Count == 0) return;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    textBox = (Excel.TextBox)workSheet.TextBoxes(dic.Key);
                    textBox.Text = dic.Value.ToString();
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("不存在ID为\"" + dic.Key.ToString() + "\"的文本框！");
                }
            }
        }

        #endregion TextBox Methods

        #region Cell Methods

        /// <summary>
        /// 向单元格写入数据，对当前WorkSheet操作
        /// </summary>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int rowIndex, int columnIndex, string text)
        {
            try
            {
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">工作表索引</param>
        /// <param name="rowIndex">行索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="text">要写入的文本值</param>
        public void SetCells(int sheetIndex, int rowIndex, int columnIndex, string text)
        {
            try
            {
                this.ChangeCurrentWorkSheet(sheetIndex); //改变当前工作表为指定工作表
                workSheet.Cells[rowIndex, columnIndex] = text;
            }
            catch
            {
                this.KillExcelProcess();
                throw new Exception("向单元格[" + rowIndex + "," + columnIndex + "]写数据出错！");
            }
        }

        /// <summary>
        /// 向单元格写入数据，对每个WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (ht.Count == 0) return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                foreach (DictionaryEntry dic in ht)
                {
                    try
                    {
                        position = dic.Key.ToString();
                        rowIndex = Convert.ToInt32(position.Split(',')[0]);
                        columnIndex = Convert.ToInt32(position.Split(',')[1]);

                        workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                    }
                    catch
                    {
                        this.KillExcelProcess();
                        throw new Exception("向单元格[" + dic.Key + "]写数据出错！");
                    }
                }
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(int sheetIndex, Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (ht.Count == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            foreach (DictionaryEntry dic in ht)
            {
                try
                {
                    position = dic.Key.ToString();
                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    workSheet.Cells[rowIndex, columnIndex] = dic.Value;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception("向单元格[" + dic.Key + "]写数据出错！");
                }
            }
        }

        /// <summary>
        /// 设置单元格为可计算的
        /// </summary>
        /// <remarks>
        /// 如果Excel的单元格格式设置为数字，日期或者其他类型时，需要设置这些单元格的FormulaR1C1属性，
        /// 否则写到这些单元格的数据将不会按照预先设定的格式显示
        /// </remarks>
        /// <param name="arr">保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(int sheetIndex, string[] arr)
        {
            int rowIndex;
            int columnIndex;
            string position;

            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (arr.Length == 0) return;

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int i = 0; i < arr.Length; i++)
            {
                try
                {
                    position = arr[i];
                    rowIndex = Convert.ToInt32(position.Split(',')[0]);
                    columnIndex = Convert.ToInt32(position.Split(',')[1]);

                    Excel.Range cell = (Excel.Range)workSheet.Cells[rowIndex, columnIndex];
                    cell.FormulaR1C1 = cell.Text;
                }
                catch
                {
                    this.KillExcelProcess();
                    throw new Exception(string.Format("计算单元格{0}出错！", arr[i]));
                }
            }
        }

        /// <summary>
        /// 向单元格写入数据，对指定WorkSheet操作
        /// </summary>
        /// <param name="ht">Hashtable的键值对保存单元格的位置索引（行索引和列索引用“,”隔开）和数据</param>
        public void SetCells(string sheetName, Hashtable ht)
        {
            int rowIndex;
            int columnIndex;
            string position;
            Excel.Worksheet sheet = null;
            int sheetIndex = 0;

            if (ht.Count == 0) return;

            try
            {
                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                        sheetIndex = workSheet.Index;
                    }
                }

                if (sheet != null)
                {
                    foreach (DictionaryEntry dic in ht)
                    {
                        try
                        {
                            position = dic.Key.ToString();
                            rowIndex = Convert.ToInt32(position.Split(',')[0]);
                            columnIndex = Convert.ToInt32(position.Split(',')[1]);

                            sheet.Cells[rowIndex, columnIndex] = dic.Value;
                        }
                        catch
                        {
                            this.KillExcelProcess();
                            throw new Exception("向单元格[" + dic.Key + "]写数据出错！");
                        }
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为\"" + sheetName + "\"的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 合并单元格，并赋值，对每个WorkSheet操作
        /// </summary>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        public void MergeCells(int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);
                range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

                range.ClearContents();  //先把Range内容清除，合并才不会出错
                range.MergeCells = true;
                range.Value = text;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            }
        }

        /// <summary>
        /// 合并单元格，并赋值，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        public void MergeCells(int sheetIndex, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
            range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents();  //先把Range内容清除，合并才不会出错
            range.MergeCells = true;
            range.Value = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        #endregion Cell Methods

        #region Row Methods

        /// <summary>
        /// 将指定索引列的数据相同的行合并，对每个WorkSheet操作
        /// </summary>
        /// <param name="columnIndex">列索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        public void MergeRows(int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (endRowIndex - beginRowIndex < 1)
                return;

            for (int i = 1; i <= this.WorkSheetCount; i++)
            {
                int beginIndex = beginRowIndex;
                int count = 0;
                string text1;
                string text2;
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(i);

                for (int j = beginRowIndex; j <= endRowIndex; j++)
                {
                    range = (Excel.Range)workSheet.Cells[j, columnIndex];
                    text1 = range.Text.ToString();

                    range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                    text2 = range.Text.ToString();

                    if (text1 == text2)
                    {
                        ++count;
                    }
                    else
                    {
                        if (count > 0)
                        {
                            this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                        }

                        beginIndex = j + 1;  //设置开始合并行索引
                        count = 0;  //计数器清0
                    }
                }
            }
        }

        /// <summary>
        /// 将指定索引列的数据相同的行合并，对指定WorkSheet操作
        /// </summary>
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="columnIndex">列索引</param>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        public void MergeRows(int sheetIndex, int columnIndex, int beginRowIndex, int endRowIndex)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            if (endRowIndex - beginRowIndex < 1)
                return;

            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);

            for (int j = beginRowIndex; j <= endRowIndex; j++)
            {
                range = (Excel.Range)workSheet.Cells[j, columnIndex];
                text1 = range.Text.ToString();

                range = (Excel.Range)workSheet.Cells[j + 1, columnIndex];
                text2 = range.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(workSheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }

                    beginIndex = j + 1;  //设置开始合并行索引
                    count = 0;  //计数器清0
                }
            }
        }

        /// <summary>
        /// 插行（在指定行上面插入指定数量行）
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 插行（在指定WorkSheet指定行上面插入指定数量行）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void InsertRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制行（在指定行下面复制指定数量行）
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void CopyRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 1; i <= count; i++)
                    {
                        range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                        range1.Copy(range2);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制行（在指定WorkSheet指定行下面复制指定数量行）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void CopyRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range1 = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 1; i <= count; i++)
                {
                    range2 = (Excel.Range)workSheet.Rows[rowIndex + i, this.missing];
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int rowIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除行
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="count"></param>
        public void DeleteRows(int sheetIndex, int rowIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Rows[rowIndex, this.missing];

                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion Row Methods

        #region Column Methods

        /// <summary>
        /// 插列（在指定列右边插入指定数量列）
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Insert(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 插列（在指定WorkSheet指定列右边插入指定数量列）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void InsertColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Insert(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制列（在指定列右边复制指定数量列）
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void CopyColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    //     range1 = (Excel.Range)workSheet.Columns[columnIndex,this.missing];
                    range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                    for (int i = 1; i <= count; i++)
                    {
                        //      range2 = (Excel.Range)workSheet.Columns[this.missing,columnIndex + i];
                        range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                        range1.Copy(range2);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 复制列（在指定WorkSheet指定列右边复制指定数量列）
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void CopyColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                //    range1 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex];
                range1 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex) + "1", this.IntToLetter(columnIndex) + "10000");

                for (int i = 1; i <= count; i++)
                {
                    //     range2 = (Excel.Range)workSheet.Columns[Type.Missing,columnIndex + i];
                    range2 = (Excel.Range)workSheet.get_Range(this.IntToLetter(columnIndex + i) + "1", this.IntToLetter(columnIndex + i) + "10000");
                    range1.Copy(range2);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int columnIndex, int count)
        {
            try
            {
                for (int n = 1; n <= this.WorkSheetCount; n++)
                {
                    workSheet = (Excel.Worksheet)workBook.Worksheets[n];
                    range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                    for (int i = 0; i < count; i++)
                    {
                        range.Delete(Excel.XlDirection.xlDown);
                    }
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="count"></param>
        public void DeleteColumns(int sheetIndex, int columnIndex, int count)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets[sheetIndex];
                range = (Excel.Range)workSheet.Columns[this.missing, columnIndex];

                for (int i = 0; i < count; i++)
                {
                    range.Delete(Excel.XlDirection.xlDown);
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        #endregion Column Methods

        #region Range Methods

        /// <summary>
        /// 将指定范围区域拷贝到目标区域
        /// </summary>
        /// <param name="sheetIndex">WorkSheet索引</param>
        /// <param name="startCell">要拷贝区域的开始Cell位置（比如：A10）</param>
        /// <param name="endCell">要拷贝区域的结束Cell位置（比如：F20）</param>
        /// <param name="targetCell">目标区域的开始Cell位置（比如：H10）</param>
        public void RangeCopy(int sheetIndex, string startCell, string endCell, string targetCell)
        {
            if (sheetIndex > this.WorkSheetCount)
            {
                this.KillExcelProcess();
                throw new Exception("索引超出范围，WorkSheet索引不能大于WorkSheet数量！");
            }

            try
            {
                workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(sheetIndex);
                range1 = workSheet.get_Range(startCell, endCell);
                range2 = workSheet.get_Range(targetCell, this.missing);

                range1.Copy(range2);
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 将指定范围区域拷贝到目标区域
        /// </summary>
        /// <param name="sheetName">WorkSheet名称</param>
        /// <param name="startCell">要拷贝区域的开始Cell位置（比如：A10）</param>
        /// <param name="endCell">要拷贝区域的结束Cell位置（比如：F20）</param>
        /// <param name="targetCell">目标区域的开始Cell位置（比如：H10）</param>
        public void RangeCopy(string sheetName, string startCell, string endCell, string targetCell)
        {
            try
            {
                Excel.Worksheet sheet = null;

                for (int i = 1; i <= this.WorkSheetCount; i++)
                {
                    workSheet = (Excel.Worksheet)workBook.Sheets.get_Item(i);

                    if (workSheet.Name == sheetName)
                    {
                        sheet = workSheet;
                    }
                }

                if (sheet != null)
                {
                    for (int i = sheetCount; i >= 1; i--)
                    {
                        range1 = sheet.get_Range(startCell, endCell);
                        range2 = sheet.get_Range(targetCell, this.missing);

                        range1.Copy(range2);
                    }
                }
                else
                {
                    this.KillExcelProcess();
                    throw new Exception("名称为\"" + sheetName + "\"的工作表不存在");
                }
            }
            catch (Exception e)
            {
                this.KillExcelProcess();
                throw e;
            }
        }

        /// <summary>
        /// 自动填充
        /// </summary>
        public void RangAutoFill()
        {
            Excel.Range rng = workSheet.get_Range("B4", Type.Missing);
            rng.Value2 = "星期一 ";
            rng.AutoFill(workSheet.get_Range("B4", "B9"),
                Excel.XlAutoFillType.xlFillWeekdays);

            rng = workSheet.get_Range("C4", Type.Missing);
            rng.Value = "一月";
            rng.AutoFill(workSheet.get_Range("C4", "C9"),
                Excel.XlAutoFillType.xlFillMonths);

            rng = workSheet.get_Range("D4", Type.Missing);
            rng.Value2 = "1";
            rng.AutoFill(workSheet.get_Range("D4", "D9"),
                Excel.XlAutoFillType.xlFillSeries);

            rng = workSheet.get_Range("E4", Type.Missing);
            rng.Value2 = "3";
            rng = workSheet.get_Range("E5", Type.Missing);
            rng.Value2 = "6";
            rng = workSheet.get_Range("E4", "E5");
            rng.AutoFill(workSheet.get_Range("E4", "E9"),
                Excel.XlAutoFillType.xlFillSeries);
        }

        /// <summary>
        /// 应用样式
        /// </summary>
        public void ApplyStyle()
        {
            object missingValue = Type.Missing;
            Excel.Range rng = workSheet.get_Range("G23", "G23");
            Excel.Style style;

            try
            {
                style = workBook.Styles["NewStyle"];
            }
            // Style doesn't exist yet.
            catch
            {
                //style = workBook.Styles.Add("NewStyle", missingValue);
                style = workBook.Styles.Add("NewStyle", missingValue);
                style.Font.Name = "Verdana";
                style.Font.Size = 12;
                style.Font.Color = 55;
                style.Interior.Color = (200 << 16) | (200 << 8) | 200;
                style.Interior.Pattern = Excel.XlPattern.xlPatternSolid;
            }

            rng.Value2 = "VENDOR's REPLY";
            rng.Style = "NewStyle";
            rng.Columns.AutoFit();
        }

        #endregion Range Methods

        #region ExcelHelper Kit

        /// <summary>
        /// 将Excel列的字母索引值转换成整数索引值
        /// </summary>
        /// <param name="letter"></param>
        /// <returns></returns>
        public int LetterToInt(string letter)
        {
            int n = 0;

            if (letter.Trim().Length == 0)
                throw new Exception("不接受空字符串！");

            if (letter.Length >= 2)
            {
                char c1 = letter.ToCharArray(0, 2)[0];
                char c2 = letter.ToCharArray(0, 2)[1];

                if (!char.IsLetter(c1) || !char.IsLetter(c2))
                {
                    throw new Exception("格式不正确，必须是字母！");
                }

                c1 = char.ToUpper(c1);
                c2 = char.ToUpper(c2);

                int i = Convert.ToInt32(c1) - 64;
                int j = Convert.ToInt32(c2) - 64;

                n = i * 26 + j;
            }

            if (letter.Length == 1)
            {
                char c1 = letter.ToCharArray()[0];

                if (!char.IsLetter(c1))
                {
                    throw new Exception("格式不正确，必须是字母！");
                }

                c1 = char.ToUpper(c1);

                n = Convert.ToInt32(c1) - 64;
            }

            if (n > 256)
                throw new Exception("索引超出范围，Excel的列索引不能超过256！");

            return n;
        }

        /// <summary>
        /// 将Excel列的整数索引值转换为字符索引值
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public string IntToLetter(int n)
        {
            if (n > 256)
                throw new Exception("索引超出范围，Excel的列索引不能超过256！");

            int i = Convert.ToInt32(n / 26);
            int j = n % 26;

            char c1 = Convert.ToChar(i + 64);
            char c2 = Convert.ToChar(j + 64);

            if (n > 26)
                return c1.ToString() + c2.ToString();
            else if (n == 26)
                return "Z";
            else
                return c2.ToString();
        }

        #endregion ExcelHelper Kit

        #region Output File(注意：如果目标文件已存在的话会出错)

        /// <summary>
        /// 输出Excel文件并退出
        /// </summary>
        public void OutputExcelFile()
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");
            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
            }
            catch (Exception)
            {
                //throw e;
                //this.KillExcelProcess();
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 输出指定格式的文件（支持格式：HTML，CSV，TEXT，EXCEL）
        /// </summary>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void OutputFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    //     case "XML":
                    //     {
                    //      workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //
                    //     }
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 保存文件
        /// </summary>
        public void SaveFile()
        {
            try
            {
                workBook.Save();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 另存文件
        /// </summary>
        public void SaveAsFile()
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");
            try
            {
                workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 将Excel文件另存为指定格式
        /// </summary>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void SaveAsFile(string format)
        {
            if (this.outputFile == null)
                throw new Exception("没有指定输出文件路径！");

            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(outputFile, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    //     case "XML":
                    //     {
                    //      workBook.SaveAs(outputFile,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //     }
                    default:
                        {
                            workBook.SaveAs(outputFile, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 另存文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        public void SaveFile(string fileName)
        {
            try
            {
                workBook.SaveAs(fileName, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// 将Excel文件另存为指定格式
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="format">HTML，CSV，TEXT，EXCEL，XML</param>
        public void SaveAsFile(string fileName, string format)
        {
            try
            {
                switch (format)
                {
                    case "HTML":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "CSV":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlCSV, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    case "TEXT":
                        {
                            workBook.SaveAs(fileName, Excel.XlFileFormat.xlHtml, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                    //     case "XML":
                    //     {
                    //      workBook.SaveAs(fileName,Excel.XlFileFormat.xlXMLSpreadsheet, Type.Missing, Type.Missing,
                    //       Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange,
                    //       Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                    //      break;
                    //     }
                    default:
                        {
                            workBook.SaveAs(fileName, missing, missing, missing, missing, missing, Excel.XlSaveAsAccessMode.xlExclusive, missing, missing, missing, missing);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.Dispose();
            }
        }

        #endregion Output File(注意：如果目标文件已存在的话会出错)

        #endregion 公共方法

        #region 私有方法

        /// <summary>
        /// 合并单元格，并赋值，对指定WorkSheet操作
        /// </summary>
        /// <param name="beginRowIndex">开始行索引</param>
        /// <param name="beginColumnIndex">开始列索引</param>
        /// <param name="endRowIndex">结束行索引</param>
        /// <param name="endColumnIndex">结束列索引</param>
        /// <param name="text">合并后Range的值</param>
        private void MergeCells(Excel.Worksheet sheet, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            if (sheet == null)
                return;

            range = sheet.get_Range(sheet.Cells[beginRowIndex, beginColumnIndex], sheet.Cells[endRowIndex, endColumnIndex]);

            range.ClearContents();  //先把Range内容清除，合并才不会出错
            range.MergeCells = true;
            range.Value = text;
            range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
        }

        /// <summary>
        /// 将指定索引列的数据相同的行合并，对指定WorkSheet操作
        /// </summary>
        /// <param name="columnIndex">要合并的列索引</param>
        /// <param name="beginRowIndex">合并开始行索引</param>
        /// <param name="rows">要合并的行数</param>
        private void MergeRows(Excel.Worksheet sheet, int columnIndex, int beginRowIndex, int rows)
        {
            int beginIndex = beginRowIndex;
            int count = 0;
            string text1;
            string text2;

            if (sheet == null)
                return;

            for (int j = beginRowIndex; j < beginRowIndex + rows; j++)
            {
                range1 = (Excel.Range)sheet.Cells[j, columnIndex];
                range2 = (Excel.Range)sheet.Cells[j + 1, columnIndex];
                text1 = range1.Text.ToString();
                text2 = range2.Text.ToString();

                if (text1 == text2)
                {
                    ++count;
                }
                else
                {
                    if (count > 0)
                    {
                        this.MergeCells(sheet, beginIndex, columnIndex, beginIndex + count, columnIndex, text1);
                    }

                    beginIndex = j + 1;  //设置开始合并行索引
                    count = 0;  //计数器清0
                }
            }
        }

        /// <summary>
        /// 计算WorkSheet数量
        /// </summary>
        /// <param name="rowCount">记录总行数</param>
        /// <param name="rows">每WorkSheet行数</param>
        public int GetSheetCount(int rowCount, int rows)
        {
            int n = rowCount % rows;  //余数

            if (n == 0)
                return rowCount / rows;
            else
                return Convert.ToInt32(rowCount / rows) + 1;
        }

        /// <summary>
        /// 结束Excel进程
        /// </summary>
        public void KillExcelProcess()
        {
            Process[] myProcesses;
            DateTime startTime;
            myProcesses = Process.GetProcessesByName("Excel");

            //得不到Excel进程ID，暂时只能判断进程启动时间
            foreach (Process myProcess in myProcesses)
            {
                startTime = myProcess.StartTime;

                if (startTime > beforeTime && startTime < afterTime)
                {
                    myProcess.Kill();
                }
            }
        }

        private void Dispose()
        {
            workBook.Close(null, null, null);
            app.Workbooks.Close();
            app.Quit();

            if (range != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range);
                range = null;
            }
            if (range1 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range1);
                range1 = null;
            }
            if (range2 != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(range2);
                range2 = null;
            }
            if (textBox != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(textBox);
                textBox = null;
            }
            if (workSheet != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workSheet);
                workSheet = null;
            }
            if (workBook != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workBook);
                workBook = null;
            }
            if (app != null)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
                app = null;
            }

            GC.Collect();
            this.KillExcelProcess();
        }//end Dispose

        #endregion 私有方法
    }
}