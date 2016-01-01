using System.Collections.Generic;

namespace XCLNetTools.DataHandler
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
        public string tableName { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<OutPutField> fields { get; set; }
    }
}