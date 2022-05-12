using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 重命名冲突时的项
    /// </summary>
    [Serializable]
    public class RenameConflictItem
    {
        /// <summary>
        /// ID
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// 临时随机的文件或文件夹名称
        /// </summary>
        public string RandomName { get; set; }

        /// <summary>
        /// 带有随机标记的路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 更正后的路径
        /// </summary>
        public string RealPath { get; set; }

        /// <summary>
        /// 更正后的文件或文件夹名称
        /// </summary>
        public string RealName { get; set; }
    }
}