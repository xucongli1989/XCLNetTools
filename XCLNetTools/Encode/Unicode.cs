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
        public static string UnicodeEncode(string s)
        {
            return reUnicodeChar.Replace(s, m => string.Format(@"\u{0:x4}", (short)m.Value[0]));
        }
    }
}