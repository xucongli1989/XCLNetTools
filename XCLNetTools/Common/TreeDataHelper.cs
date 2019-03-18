using System;
using System.Collections.Generic;
using XCLNetTools.Entity.TreeData;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 树状数据结构帮助类
    /// </summary>
    /// <typeparam name="IDType">主键ID的数据类型，一般为int、long</typeparam>
    /// <typeparam name="ModelType">要查询的记录实体类型</typeparam>
    public class TreeTableLibrary<IDType, ModelType>
    {
        /// <summary>
        /// 当节点的parentid为此值时，那么此节点就为根节点，一般情况下为0
        /// </summary>
        public IDType RootParentID { get; set; } = default(IDType);

        /// <summary>
        /// 自定义根据id返回其父id的方法，如果找不到此id的记录，请抛出异常而不是返回0或null或其它值，以此来避免无法找到根节点
        /// </summary>
        public Func<IDType, IDType> GetParentIDFunc { get; set; }

        /// <summary>
        /// 自定义根据id返回其子id的方法
        /// </summary>
        public Func<IDType, List<IDType>> GetChildIDListFunc { get; set; }

        /// <summary>
        /// 自定义根据id返回数据记录实体的方法
        /// </summary>
        public Func<IDType, ModelType> GetModelByIDFunc { get; set; }

        /// <summary>
        /// 是否为根节点
        /// </summary>
        public bool IsRootID(IDType id)
        {
            var parentId = this.GetParentIDFunc.Invoke(id);
            return parentId.Equals(this.RootParentID);
        }

        /// <summary>
        /// 是否为叶子节点
        /// </summary>
        public bool IsLeaf(IDType id)
        {
            var lst = this.GetChildIDListFunc.Invoke(id);
            return null == lst || lst.Count == 0;
        }

        /// <summary>
        /// 根据指定ID，返回它所在的根节点ID
        /// </summary>
        public IDType GetRootID(IDType id)
        {
            var parentId = this.GetParentIDFunc.Invoke(id);
            if (parentId.Equals(this.RootParentID))
            {
                return id;
            }
            else
            {
                return this.GetRootID(parentId);
            }
        }

        /// <summary>
        /// 根据指定ID，返回它所在的根节点实体
        /// </summary>
        public ModelType GetRootModel(IDType id)
        {
            var rootId = this.GetRootID(id);
            return this.GetModelByIDFunc(rootId);
        }

        /// <summary>
        /// 根据指定ID，返回从该节点一直到根节点的路径上的所有ID
        /// 注：返回结果的第一项为根节点，最后一项为传入的节点
        /// </summary>
        public List<IDType> GetLayerIDList(IDType id)
        {
            var lst = new List<IDType>();
            lst.Add(id);
            while (true)
            {
                var parentId = this.GetParentIDFunc.Invoke(id);
                if (parentId.Equals(this.RootParentID))
                {
                    break;
                }
                id = parentId;
                lst.Add(id);
            }
            lst.Reverse();
            return lst;
        }

        /// <summary>
        /// 根据指定ID，返回从该节点一直到根节点的路径上的所有ID实体
        /// 注：返回结果的第一项为根节点，最后一项为传入的节点
        /// </summary>
        public List<ModelType> GetLayerModelList(IDType id)
        {
            var lst = new List<ModelType>();
            var idList = this.GetLayerIDList(id);
            idList.ForEach(k =>
            {
                var m = this.GetModelByIDFunc(k);
                if (null != m)
                {
                    lst.Add(m);
                }
            });
            return lst;
        }

        /// <summary>
        /// 根据指定根节点id，返回它下面的所有子孙的树状层级结构的实体信息（这里的根节点可以是任意节点）
        /// </summary>
        public TreeMapEntity<IDType, ModelType> GetTreeModelList(IDType rootId)
        {
            Func<IDType, int, TreeMapEntity<IDType, ModelType>> fun = null;
            fun = new Func<IDType, int, TreeMapEntity<IDType, ModelType>>((IDType id, int level) =>
             {
                 var m = this.GetModelByIDFunc(id);
                 if (null == m)
                 {
                     return null;
                 }
                 var childList = this.GetChildIDListFunc(id);
                 var model = new TreeMapEntity<IDType, ModelType>();
                 model.ChildList = new List<TreeMapEntity<IDType, ModelType>>();
                 model.NodeLevel = level;
                 model.ID = id;
                 model.IsLeaf = null == childList || childList.Count == 0;
                 model.IsRoot = id.Equals(rootId);
                 model.Model = m;
                 model.ParentID = this.GetParentIDFunc(model.ID);
                 if (null != childList && childList.Count > 0)
                 {
                     level++;
                     childList.ForEach(childId =>
                     {
                         var s = fun.Invoke(childId, level);
                         if (null != s)
                         {
                             model.ChildList.Add(s);
                         }
                     });
                 }
                 return model;
             });
            return fun.Invoke(rootId, 1);
        }

        /// <summary>
        /// 根据指定根节点id，返回它下面的所有子孙的实体信息列表（这里的根节点可以是任意节点）
        /// </summary>
        public List<ExtendDataEntity<IDType, ModelType>> GetExtendDataEntityList(IDType rootId)
        {
            var lst = new List<ExtendDataEntity<IDType, ModelType>>();
            Action<IDType, int> act = null;
            act = new Action<IDType, int>((IDType id, int level) =>
            {
                var m = this.GetModelByIDFunc(id);
                if (null == m)
                {
                    return;
                }
                var childList = this.GetChildIDListFunc(id);
                var model = new ExtendDataEntity<IDType, ModelType>();
                model.NodeLevel = level;
                model.ID = id;
                model.IsLeaf = null == childList || childList.Count == 0;
                model.IsRoot = id.Equals(rootId);
                model.Model = m;
                model.ParentID = this.GetParentIDFunc(model.ID);
                if (null != childList && childList.Count > 0)
                {
                    level++;
                    childList.ForEach(childId =>
                    {
                        act.Invoke(childId, level);
                    });
                }
                lst.Add(model);
            });
            act.Invoke(rootId, 1);
            lst.Reverse();
            return lst;
        }
    }
}