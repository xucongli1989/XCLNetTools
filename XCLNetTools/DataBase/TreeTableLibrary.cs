using System;

namespace XCLNetTools.DataBase
{
    /// <summary>
    /// 树状表帮助类
    /// </summary>
    public class TreeTableLibrary<IDType, ModelType>
    {
        /// <summary>
        /// 当节点的parentid为此值时，那么此节点就为根节点，一般情况下为0
        /// </summary>
        public IDType RootParentID { get; set; } = default(IDType);

        /// <summary>
        /// 自定义根据id，返回parentId的方法
        /// </summary>
        public Func<IDType, IDType> GetParentIDFunc { get; set; }

        /// <summary>
        /// 根据指定ID，返回它所在的根节点ID
        /// </summary>
        public IDType GetRootID(IDType id)
        {
            var parentId = this.GetParentIDFunc.Invoke(id);
            if (parentId.Equals(this.RootParentID))
            {
                return parentId;
            }
            else
            {
                return this.GetRootID(parentId);
            }
        }
    }
}