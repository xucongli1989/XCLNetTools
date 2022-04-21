using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 数据行实体
    /// </summary>
    [Serializable]
    public class LineItem<T>
    {
        /// <summary>
        /// 行号，从 1 开始
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// 第一项
        /// </summary>
        public T Item1 { get; set; }

        /// <summary>
        /// 第二项
        /// </summary>
        public T Item2 { get; set; }
    }
}