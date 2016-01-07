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



using System.Web.Security;

namespace XCLNetTools.Encrypt
{
    /// <summary>
    /// md5相关
    /// </summary>
    public class MD5
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
    }
}