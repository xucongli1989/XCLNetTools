/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// sql 分页条件实体
    /// </summary>
    [Serializable]
    public class SqlPagerConditionEntity
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="tableName">表名</param>
        public SqlPagerConditionEntity(string tableName)
        {
            this.TableName = tableName;
        }

        /// <summary>
        /// 数据库类型（默认sql server）
        /// </summary>
        public XCLNetTools.Enum.CommonEnum.DatabaseTypeEnum DatabaseType { get; set; } = XCLNetTools.Enum.CommonEnum.DatabaseTypeEnum.MSSQL;

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// where条件（前面无需加"where"）
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 页码（默认1）
        /// </summary>
        public int PageIndex { get; set; } = 1;

        /// <summary>
        /// 每页最多显示的记录数（默认10）
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 是否需要查询当前所有页结果的总记录数（默认：true）
        /// </summary>
        public bool IsNeedAllCount { get; set; } = true;

        /// <summary>
        /// 排序字段（前面无需添加"ORDER BY"）
        /// </summary>
        public string OrderBy { get; set; }

        /// <summary>
        /// 要查询的字段（若无值，则默认为所有字段）
        /// </summary>
        public List<string> FieldNameList { get; set; }
    }
}