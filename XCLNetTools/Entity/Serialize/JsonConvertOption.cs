using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity.Serialize
{
    [Serializable]
    public class JsonConvertOption
    {
        /// <summary>
        /// 是否需要将枚举转换为字符串而不是数字形式
        /// </summary>
        public bool IsConvertEnumToString { get; set; }
    }
}