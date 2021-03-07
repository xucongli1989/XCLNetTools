/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Text;

namespace XCLNetTools.Encode
{
    /// <summary>
    /// base64相关
    /// </summary>
    public static class Base64
    {
        /// <summary>
        /// Base64 编码
        /// </summary>
        public static string Base64Code(string msg, System.Text.Encoding encoding)
        {
            byte[] bytes = encoding.GetBytes(msg);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64 解码
        /// </summary>
        public static string Base64Decode(string msg, System.Text.Encoding encoding)
        {
            byte[] bytes = Convert.FromBase64String(msg);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// Base64 编码（默认为：Encoding.Default）
        /// </summary>
        public static string Base64Code(string msg)
        {
            return Base64Code(msg, System.Text.Encoding.Default);
        }

        /// <summary>
        /// Base64 解码（默认为：Encoding.Default）
        /// </summary>
        public static string Base64Decode(string msg)
        {
            return Base64Decode(msg, System.Text.Encoding.Default);
        }
    }
}