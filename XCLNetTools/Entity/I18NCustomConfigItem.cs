using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XCLNetTools.Entity
{
    [Serializable]
    public class I18NCustomConfigItem
    {
        /// <summary>
        /// 语言代码，如：zh-CN
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 键名
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}