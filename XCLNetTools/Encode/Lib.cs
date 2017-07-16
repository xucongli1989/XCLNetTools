/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

namespace XCLNetTools.Encode
{
    /// <summary>
    /// 其它相关
    /// </summary>
    public static class Lib
    {
        /// <summary>
        /// 对字符串进行js的unescape解码
        /// </summary>
        /// <param name="str">待解码的字符串</param>
        /// <returns>解码后的值</returns>
        public static string Unescape(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return Microsoft.JScript.GlobalObject.unescape(str);
        }

        /// <summary>
        /// 对字符串进行js的escape编码
        /// </summary>
        /// <param name="str">待编码的字符串</param>
        /// <returns>编码后的值</returns>
        public static string Escape(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            return Microsoft.JScript.GlobalObject.escape(str);
        }
    }
}