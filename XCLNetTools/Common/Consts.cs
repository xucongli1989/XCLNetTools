using System.Text.RegularExpressions;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 常量
    /// </summary>
    public class Consts
    {
        #region 正则

        /// <summary>
        /// http Scheme
        /// </summary>
        public static Regex HttpSchemeMatch = new Regex("^http[s]?://");

        #endregion 正则
    }
}