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


using System;
using System.Linq;
using System.Text;

namespace XCLNetTools.Encode
{
    /// <summary>
    /// 十六进制处理
    /// </summary>
    public class Hex
    {
        /// <summary>
        /// 16进制字符串转为byte[]
        /// </summary>
        /// <param name="hex">16进制字符串</param>
        /// <returns>byte[]</returns>
        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        /// <summary>
        /// 为字符串中的非英文字符编码
        /// </summary>
        /// <returns>处理后的值</returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        ///指定 一个字符是否应该被编码
        /// </summary>
        /// <returns>true:可以被编码，false:无需编码</returns>
        public static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// 为非英文字符编码
        /// </summary>
        /// <returns>编码后的值</returns>
        public static string ToHexString(char chr)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            StringBuilder builder = new StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取或更新16进制中的第几块区域中的值。比如：0xABC中，第一块为C，第二块为B，...
        /// </summary>
        /// <param name="blockIndex">序号</param>
        /// <param name="source">被操作的数据</param>
        /// <param name="newBlockValue">新值</param>
        /// <returns>如果是获取，则返回获取到的值；如果是更新，则返回新的source值</returns>
        public static int GetOrSetBlockFromHex(int blockIndex, int source, int? newBlockValue = null)
        {
            if (blockIndex <= 0)
            {
                throw new ArgumentException("blockIndex必须大于0");
            }
            if (newBlockValue.HasValue && (newBlockValue < 0 || newBlockValue > 15))
            {
                throw new ArgumentException("blockValue必须>=0并且<=15");
            }

            int moveBits = (blockIndex - 1) * 4;
            int blockValue = (source & (0xF << moveBits)) >> moveBits;

            if (newBlockValue.HasValue)
            {
                return source - (blockValue << moveBits) + (newBlockValue.Value << moveBits);
            }
            else
            {
                return blockValue;
            }
        }
    }
}