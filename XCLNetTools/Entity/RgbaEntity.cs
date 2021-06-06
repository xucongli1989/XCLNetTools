using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// R、G、B、A 实体
    /// </summary>
    [Serializable]
    public class RgbaEntity
    {
        /// <summary>
        /// 0-255
        /// </summary>
        public byte R { get; set; }

        /// <summary>
        /// 0-255
        /// </summary>
        public byte G { get; set; }

        /// <summary>
        /// 0-255
        /// </summary>
        public byte B { get; set; }

        /// <summary>
        /// 0-1
        /// </summary>
        public double A { get; set; }
    }
}