using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 开始与结束值实体
    /// </summary>
    [Serializable]
    public class RangeValueEntity
    {
        /// <summary>
        /// 开始值
        /// </summary>
        public int StartValue { get; set; }

        /// <summary>
        /// 结束值
        /// </summary>
        public int EndValue { get; set; }

        /// <summary>
        /// 匹配到符合范围要求的数量
        /// </summary>
        public int Count
        {
            get
            {
                return this.EndValue - this.StartValue + 1;
            }
        }
    }
}