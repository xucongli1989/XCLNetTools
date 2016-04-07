/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

namespace XCLNetTools.Entity.Office.ExcelHandler
{
    /// <summary>
    /// 要导出的字段类
    /// </summary>
    public class OutPutField
    {
        /// <summary>
        /// 该字段的原始名
        /// </summary>
        public string oldName { get; set; }

        /// <summary>
        /// 导出后，该字段在EXCEL中的显示名
        /// </summary>
        public string newName { get; set; }
    }
}