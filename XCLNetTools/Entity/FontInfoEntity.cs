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
        /// 字体文件存放路径
        /// </summary>
        public string SourcePath { get; set; }

        /// <summary>
        /// 字体文件路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 字体文件名称（如：abc.ttf 中的 abc）
        /// </summary>
        public string FileNameWithoutExt { get; set; }

        /// <summary>
        /// 用于显示的字体中文名称，如：宋体
        /// </summary>
        public string CNName { get; set; }

        /// <summary>
        /// 用于显示的字体英文名称，如：SimSun
        /// </summary>
        public string ENName { get; set; }

        /// <summary>
        /// 用于显示的字体名称
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// 字体值，可以根据该字体值找到字体对象，默认为 ENName。（注意：不同的库在查询字体时对值的定义可能不一样，因此可以根据需要修改此值）
        /// </summary>
        public string FontValue { get; set; }
    }
}