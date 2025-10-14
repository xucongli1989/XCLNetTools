using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    [Serializable]
    public class PathInfoEntity
    {
        /// <summary>
        /// 完整路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsFolder { get; set; }
    }
}