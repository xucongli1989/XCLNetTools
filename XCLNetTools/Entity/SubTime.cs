using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 时间差的类
    /// </summary>
    [Serializable]
    public class SubTime
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 时间差（小时）
        /// </summary>
        public decimal? SumTime { get; set; }
    }
}