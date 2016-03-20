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
using System.Web;
using XCLNetTools.Entity.DataHandler;

namespace XCLNetTools.DataHandler
{
    /// <summary>
    /// 操作EXCEL导出数据报表的类
    /// </summary>
    public class DataToExcel
    {
        /// <summary>
        /// 数据导出excel
        /// </summary>
        /// <param name="tableName">【表名】（主要是便于在字段信息xml的list中查找到当前导出的信息字段对应关系）</param>
        /// <param name="outPutClass">导出字段对应关系list</param>
        /// <param name="ds">数据源</param>
        /// <param name="fileTitle">文件名</param>
        /// <param name="conTitle">文件内容第一行的名称</param>
        public static void OutPutExcel(string[] tableName, List<OutPutClass> outPutClass, DataSet ds, string fileTitle, string[] conTitle)
        {
            OutPutParamClass paramClass = new OutPutParamClass();
            paramClass.TableName = tableName;
            paramClass.OutPutClass = outPutClass;
            paramClass.Ds = ds;
            paramClass.FileTitle = fileTitle;
            paramClass.ConTitle = conTitle;
            OutPutExcel(paramClass);
        }

        /// <summary>
        /// 数据导出excel
        /// </summary>
        /// <param name="paramClass">导出参数</param>
        public static void OutPutExcel(OutPutParamClass paramClass)
        {
            StringBuilder str = new StringBuilder();

            #region 合法性检测

            if (null == paramClass.Ds || paramClass.Ds.Tables.Count == 0)
            {
                str.Append("要导出的数据不能为空，导出失败！；");
            }
            if (null != paramClass.ConTitle && null != paramClass.Ds)
            {
                if (paramClass.ConTitle.Length != paramClass.Ds.Tables.Count)
                {
                    str.Append("自定义的标题与要导出的数据源的数量不匹配，导出失败！；");
                }
                if (paramClass.ConTitle.ToList().Distinct().Count() != paramClass.ConTitle.Length)
                {
                    str.Append("自定义的标题项不能重复，因为此项要作为Sheet的名称，导出失败！；");
                }
            }
            if (null != paramClass.OutPutClass && paramClass.OutPutClass.Count > 0)
            {
                if (paramClass.TableName.Length != paramClass.Ds.Tables.Count)
                {
                    str.Append("表名与dataSet的table数量不一致，导出失败！；");
                }
            }
            if (str.Length > 0)
            {
                throw new Exception(str.ToString());
            }

            #endregion 合法性检测

            #region 是否指定被操作的工作薄

            Workbook workbook = null;
            if (!string.IsNullOrEmpty(paramClass.WorkBookFilePath))
            {
                workbook = new Workbook(paramClass.WorkBookFilePath);
            }
            else
            {
                workbook = new Workbook();
            }

            #endregion 是否指定被操作的工作薄

            for (int i = 0; i < paramClass.Ds.Tables.Count; i++)
            {
                Worksheet sheet = workbook.Worksheets[i];

                if (null != paramClass.ConTitle && paramClass.ConTitle.Length > 0)
                {
                    sheet.Name = paramClass.ConTitle[i];
                }

                if (i != paramClass.Ds.Tables.Count - 1)
                {
                    workbook.Worksheets.Add();
                }

                DataTable dt = paramClass.Ds.Tables[i];
                List<string> dtColNameLst = new List<string>();
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    dtColNameLst.Add(dt.Columns[k].ColumnName);
                }

                #region 写入列名

                List<string> newNamesLst = new List<string>();
                if (null != paramClass.OutPutClass && paramClass.OutPutClass.Count > 0)
                {
                    OutPutClass outPutModel = paramClass.OutPutClass.First(k => k.TableName == paramClass.TableName[i].Trim());
                    foreach (var m in outPutModel.Fields)
                    {
                        newNamesLst.Add(m.newName);
                        if (dtColNameLst.Contains(m.oldName))
                        {
                            dt.Columns[m.oldName].ColumnName = m.newName;
                        }
                    }
                }
                else
                {
                    newNamesLst = dtColNameLst;
                }

                #endregion 写入列名

                #region 向sheet中写入数据

                dt.AcceptChanges();
                sheet.Cells.ImportDataTable(dt.DefaultView.ToTable("dtNew", true, newNamesLst.ToArray()), paramClass.IsShowFieldLine, paramClass.FirstRowIndex, paramClass.FirstColumnIndex, dt.Rows.Count + 1, dt.Columns.Count, true, "yyyy-MM-dd HH:mm:ss");
                sheet.AutoFitColumns();

                #endregion 向sheet中写入数据

                #region 添加样式

                Cells cells = sheet.Cells;
                if (paramClass.IsShowCustomLine)
                {
                    cells[0, 0].Value = string.Format("数据导出：{0}；导出时间：{1}；记录总数：{2}", paramClass.ConTitle[i], DateTime.Now, dt.Rows.Count);
                    Aspose.Cells.Style styleCell0 = cells[0, 0].GetStyle();
                    styleCell0.Font.Color = System.Drawing.Color.Red;
                    cells[0, 0].SetStyle(styleCell0);
                }
                if (paramClass.IsShowFieldLine)
                {
                    Range range = sheet.Cells.CreateRange(1, 0, 1, newNamesLst.Count);
                    range.Name = "Range1";
                    Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    style.Font.Color = System.Drawing.Color.Blue;
                    style.Font.IsBold = true;
                    StyleFlag styleFlag = new StyleFlag();
                    styleFlag.All = true;
                    range.ApplyStyle(style, styleFlag);
                }

                #endregion 添加样式
            }
            paramClass.GetWorkBook = workbook;

            #region 保存

            if (!string.IsNullOrEmpty(paramClass.CustomFileName))
            {
                workbook.Save(paramClass.CustomFileName, paramClass.SaveFormat);
            }
            if (paramClass.AutoDownLoad)
            {
                string fileName = string.Format("{0}_数据导出.xlsx", paramClass.FileTitle);
                bool isFirefox = HttpContext.Current.Request.Browser.Type.ToLower().Contains("firefox");
                fileName = isFirefox ? fileName : HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8);
                workbook.Save(HttpContext.Current.Response, fileName, ContentDisposition.Attachment, new OoxmlSaveOptions(SaveFormat.Xlsx));
            }

            #endregion 保存
        }
    }
}