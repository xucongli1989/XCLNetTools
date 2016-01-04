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
using System.Text;

namespace XCLNetTools.Encode
{
    /// <summary>
    /// base64相关
    /// </summary>
    public class Base64
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="msg">要加密的内容</param>
        /// <returns>加密后的值</returns>
        public static string Base64Code(string msg)
        {
            byte[] bytes = Encoding.Default.GetBytes(msg);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="msg">要解密的内容</param>
        /// <returns>解密后的值</returns>
        public static string Base64Decode(string msg)
        {
            byte[] bytes = Convert.FromBase64String(msg);
            return Encoding.Default.GetString(bytes);
        }
    }
}