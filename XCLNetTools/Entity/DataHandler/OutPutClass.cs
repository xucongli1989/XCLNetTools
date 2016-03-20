/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Collections.Generic;

namespace XCLNetTools.Entity.DataHandler
{
    /// <summary>
    /// 导出字段实体类
    /// （主要是便于在所有导出信息字段类中查询到要导出的记录的字段对应信息）
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