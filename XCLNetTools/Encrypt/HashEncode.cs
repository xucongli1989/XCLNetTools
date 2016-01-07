/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.1
更新时间：2016-01-01

四：更新内容：
1：将原先基础数据转换方法转移到Common/DataTypeConvert下面
2：其它逻辑优化，如表单参数获取等
3：首次开放所有源代码
 */



using System;
using System.Security.Cryptography;
using System.Text;

namespace XCLNetTools.Encrypt
{
    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public class HashEncode
    {
        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns>密文</returns>
        public static string GetSecurity()
        {
            return HashEncoding(new Random().Next(1, int.MaxValue).ToString());
        }

        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="security">待加密的数据</param>
        /// <returns>密文</returns>
        public static string HashEncoding(string security)
        {
            byte[] value;
            UnicodeEncoding code = new UnicodeEncoding();
            byte[] message = code.GetBytes(security);
            SHA512Managed Arithmetic = new SHA512Managed();
            value = Arithmetic.ComputeHash(message);
            security = "";
            foreach (byte o in value)
            {
                security += (int)o + "O";
            }
            return security;
        }
    }
}