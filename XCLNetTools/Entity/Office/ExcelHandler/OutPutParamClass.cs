/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Aspose.Cells;
using System.Collections.Generic;
using System.Data;

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// 导出参数类
    /// </summary>
    public class OutPutParamClass
    {
        /// <summary>
        /// 表名,对应dataSet中的table名（用于在OutPutClass参数中查找字段的对应关系，当没有设置字段对应关系时可以为null）
        /// </summary>
        public string[] TableName { get; set; }

        /// <summary>
        /// DataSet中的字段与Sheet中实际显示的字段对应关系设置（为null时，则使用DataSet中的列名）
        /// </summary>
        public List<OutPutClass> OutPutClass { get; set; }

        /// <summary>
        /// 要导出的DataSet
        /// </summary>
        public DataSet Ds { get; set; }

        /// <summary>
        /// 导出的EXCEL文件的名字（仅适用于Http环境）
        /// </summary>
        public string FileTitle { get; set; }

        /// <summary>
        /// Excel中每个Sheet的名称
        /// </summary>
        public string[] ConTitle { get; set; }

        private bool _autoDownLoad = true;

        /// <summary>
        /// 是否自动下载（true：在Http环境自动下载；false：默认行为），默认值为true
        /// </summary>
        public bool AutoDownLoad
        {
            get { return _autoDownLoad; }
            set { _autoDownLoad = value; }
        }

        /// <summary>
        /// 自定义文件名（保存后的完整路径名）
        /// </summary>
        public string CustomFileName
        {
            get;
            set;
        }

        /// <summary>
        /// 自定义保存时，文件保存的格式
        /// </summary>
        public Aspose.Cells.SaveFormat SaveFormat
        {
            get;
            set;
        }

        private int _firstRowIndex = 1;

        /// <summary>
        /// 填充的数据起始行索引号（0为第一行），默认为1
        /// </summary>
        public int FirstRowIndex
        {
            get { return _firstRowIndex; }
            set { _firstRowIndex = value; }
        }

        /// <summary>
        /// 填充的数据起始列索引号（0为第一行）
        /// </summary>
        public int FirstColumnIndex { get; set; }

        private bool _isShowCustomLine = true;

        /// <summary>
        /// 是否显示每个Sheet的第一行的导出状态信息，默认为true
        /// </summary>
        public bool IsShowCustomLine
        {
            get { return _isShowCustomLine; }
            set { _isShowCustomLine = value; }
        }

        private bool _isShowFieldLine = true;

        /// <summary>
        /// 是否显示字段行
        /// </summary>
        public bool IsShowFieldLine
        {
            get { return _isShowFieldLine; }
            set { _isShowFieldLine = value; }
        }

        /// <summary>
        /// 指定被操作的工作薄文件
        /// （用于向已有文件中写入数据并导出的情况）
        /// </summary>
        public string WorkBookFilePath
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前正在操作的WorkBook
        /// </summary>
        public Workbook GetWorkBook
        {
            get;
            set;
        }
    }
}