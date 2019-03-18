using System;
using System.Collections.Generic;

namespace XCLNetTools.Entity.TreeData
{
    /// <summary>
    /// 树状层级映射实体
    /// </summary>
    /// <typeparam name="IDType">主键数据类型</typeparam>
    /// <typeparam name="ModelType">要查询的记录实体类型</typeparam>
    [Serializable]
    public class TreeMapEntity<IDType, ModelType> : ExtendDataEntity<IDType, ModelType>
    {
        /// <summary>
        /// 子数据实体
        /// </summary>
        public List<TreeMapEntity<IDType, ModelType>> ChildList { get; set; }
    }
}