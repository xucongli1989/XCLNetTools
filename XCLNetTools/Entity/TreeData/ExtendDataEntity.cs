using System;

namespace XCLNetTools.Entity.TreeData
{
    /// <summary>
    /// 扩展数据实体
    /// </summary>
    /// <typeparam name="IDType">主键数据类型</typeparam>
    /// <typeparam name="ModelType">要查询的记录实体类型</typeparam>
    [Serializable]
    public class ExtendDataEntity<IDType, ModelType>
    {
        /// <summary>
        /// 是否为根节点
        /// </summary>
        public bool IsRoot { get; set; }

        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        public bool IsLeaf { get; set; }

        /// <summary>
        /// 层级，根节点为1，然后累加
        /// </summary>
        public int NodeLevel { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public IDType ID { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        public IDType ParentID { get; set; }

        /// <summary>
        /// 数据实体
        /// </summary>
        public ModelType Model { get; set; }
    }
}