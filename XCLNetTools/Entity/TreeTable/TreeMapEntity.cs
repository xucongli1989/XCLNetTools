using System;
using System.Collections.Generic;

namespace XCLNetTools.Entity.TreeTable
{
    /// <summary>
    /// 树状层级映射实体
    /// </summary>
    /// <typeparam name="IDType">主键数据类型</typeparam>
    [Serializable]
    public class TreeMapEntity<IDType>
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public IDType ID { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public IDType ParentID { get; set; }

        /// <summary>
        /// 该ID对应的子元素ID
        /// </summary>
        public List<IDType> ChildIDList { get; set; }
    }
}