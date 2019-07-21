using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// IP网络位置实体
    /// </summary>
    [Serializable]
    public class LocationEntity
    {
        /// <summary>
        /// ip地址，如：113.118.186.219
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// 地址，如：广东省深圳市 电信
        /// </summary>
        public string Address { get; set; }
    }
}