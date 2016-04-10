/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// 随机数操作类
    /// </summary>
    public class RandomHelper
    {
        /// <summary>
        /// 生成指定范围内的随机数（不重复）
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        /// <returns>结果值</returns>
        public static int GetRandomValue(int minValue, int maxValue)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// 将GUID的哈希数作为Random的种子，然后生成一个非负随机数
        /// 例如：1024588704
        /// </summary>
        /// <returns>结果值</returns>
        public static int GenerateIdWithGuid()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next();
        }

        /// <summary>
        /// 根据GUID生成Int（有符号）
        /// 例如：2069396417
        /// </summary>
        /// <returns>结果值</returns>
        public static int GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// 根据guid生成字符串(16位)
        /// 例如：aded0a2611f8aa4a
        /// </summary>
        /// <returns>结果值</returns>
        public static string GenerateStringId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 随机生成数字和字母组合
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="isIgnoreCase">是否区分大小写，默认为:false</param>
        /// <returns>结果值</returns>
        public static string GenerateRandom(int len, bool isIgnoreCase = false)
        {
            if (len <= 0)
            {
                return null;
            }

            char[] dataSource = isIgnoreCase ? XCLNetTools.Common.Consts.EngLowercaseAndNumberChar : XCLNetTools.Common.Consts.EngLetterAndNumberChar;

            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var temp = Guid.NewGuid().GetHashCode() % dataSource.Length;
                temp = temp > 0 ? temp - 1 : dataSource.Length - 1;
                newRandom.Append(dataSource[temp]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 随机生成只有字母的组合
        /// </summary>
        /// <param name="len">长度</param>
        /// <param name="isIgnoreCase">是否区分大小写，默认为:false</param>
        /// <returns>结果值</returns>
        public static string GenerateRandomToChars(int len, bool isIgnoreCase = false)
        {
            if (len <= 0)
            {
                return null;
            }
            char[] dataSource = isIgnoreCase ? XCLNetTools.Common.Consts.EngLowercaseLetterChar : XCLNetTools.Common.Consts.EngLetterChar;

            System.Text.StringBuilder newRandom = new System.Text.StringBuilder();
            for (int i = 0; i < len; i++)
            {
                var temp = Guid.NewGuid().GetHashCode() % dataSource.Length;
                temp = temp > 0 ? temp - 1 : dataSource.Length - 1;
                newRandom.Append(dataSource[temp]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 截取GUID的前几个字符
        /// </summary>
        /// <param name="len">指定的长度</param>
        /// <returns>结果值</returns>
        public static string GetGuidString(int len)
        {
            string g = System.Guid.NewGuid().ToString("N");
            if (len > 0 && len <= g.Length)
            {
                g = g.Substring(0, len);
            }
            return g;
        }
    }
}