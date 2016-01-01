using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 浏览器书签实体
    /// </summary>
    [Serializable]
    public class BookmarkEntity
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父id
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// 书签名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ico图标地址
        /// </summary>
        public string IcoURL { get; set; }

        /// <summary>
        /// 书签链接
        /// </summary>
        public string Url { get; set; }
    }
}