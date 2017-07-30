/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Collections.Generic;

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// 导出字段实体类（数据源中的每个数据表中的字段名与导出后的文件实际显示的字段名的对应关系）
    /// </summary>
    public class OutPutClass
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<OutPutField> Fields { get; set; }
    }
}