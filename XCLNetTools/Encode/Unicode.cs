/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-02

四：更新内容：
1：更新表单获取的参数类型
2：更改Message/JsonMsg类的目录
3：删除多余的方法
4：修复一处未dispose问题
5：整理部分代码
6：添加 MethodResult.cs
7：获取枚举list时可以使用byte/short等
 */


using System.Globalization;
using System.Text.RegularExpressions;

namespace XCLNetTools.Encode
{
    /// <summary>
    /// Unicode相关
    /// </summary>
    public class Unicode
    {
        private static Regex reUnicode = new Regex(@"\\u([0-9a-fA-F]{4})", RegexOptions.Compiled);
        private static Regex reUnicodeChar = new Regex(@"[^\u0000-\u00ff]", RegexOptions.Compiled);

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <param name="s">待解码的字符串</param>
        /// <returns>解码后的值</returns>
        public static string UnicodeDecode(string s)
        {
            return reUnicode.Replace(s, m =>
            {
                short c;
                if (short.TryParse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture, out c))
                {
                    return "" + (char)c;
                }
                return m.Value;
            });
        }

        /// <summary>
        /// Unicode编码
        /// </summary>
        /// <param name="s">待编码的字符串</param>
        /// <returns>编码后的值</returns>
        public static string UnicodeEncode(string s)
        {
            return reUnicodeChar.Replace(s, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }
    }
}