using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 字体信息实体
    /// </summary>
    [Serializable]
    public class FontInfoEntity
    {
        /// <summary>
        /// 字体中文名称，如：宋体
        /// </summary>
        public string CNName { get; set; }

        /// <summary>
        /// 字体英文名称，如：SimSun
        /// </summary>
        public string ENName { get; set; }
    }
}