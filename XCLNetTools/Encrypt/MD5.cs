/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Web.Security;

namespace XCLNetTools.Encrypt
{
    /// <summary>
    /// md5相关
    /// </summary>
    public static class MD5
    {
        /// <summary>
        /// MD5加密（大写）
        /// <param name="str">待加密字符串</param>
        /// <param name="key">是否在待加密的字符串中末尾追加该key，从而生成md5</param>
        /// <returns>加密后的字符串（大写）</returns>
        /// </summary>
        public static string EncodeMD5(string str, string key = "")
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(string.Format("{0}{1}", str, key), "md5");
        }

        /// <summary>
        /// 判断明文与密文是否匹配
        /// 如果指定了key，则将明文与key组成的字符串的md5与md5Str进行比较
        /// </summary>
        /// <param name="str">明文</param>
        /// <param name="md5Str">md5密文</param>
        /// <param name="key">key</param>
        /// <returns>是否匹配</returns>
        public static bool IsEqualMD5(string str, string md5Str, string key = "")
        {
            return string.Equals(md5Str, MD5.EncodeMD5(str, key));
        }

        /// <summary>
        /// 判断字符串是否为32位md5（不区分大小写）
        /// </summary>
        public static bool Is32MD5(string str)
        {
            return XCLNetTools.Common.Consts.RegMD5_32Uppercase.IsMatch((str ?? "").ToUpper());
        }
    }
}