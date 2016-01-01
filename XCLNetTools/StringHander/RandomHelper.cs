using System;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// 随机数操作类
    /// </summary>
    public class RandomHelper
    {
        #region 私有变量

        /// <summary>
        /// 英文字母+数字
        /// </summary>
        private static readonly char[] constant =
          {
            '0','1','2','3','4','5','6','7','8','9',
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
          };

        /// <summary>
        /// 英文字母
        /// </summary>
        private static readonly char[] constantChar =
          {
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'
          };

        /// <summary>
        /// 小写字母+数字
        /// </summary>
        private static readonly string[] arrString = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

        #endregion 私有变量

        /// <summary>
        /// 生成指定范围内的随机数（不重复）
        /// </summary>
        /// <param name="minValue">最小值（包含）</param>
        /// <param name="maxValue">最大值（不包含）</param>
        public static int GetRandomValue(int minValue, int maxValue)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(minValue, maxValue);
        }

        /// <summary>
        /// 将GUID的哈希数作为Random的种子，然后生成一个非负随机数
        /// 例如：1024588704
        /// </summary>
        public static int GenerateIdWithGuid()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next();
        }

        /// <summary>
        /// 根据GUID生成Int（有符号）
        /// 例如：2069396417
        /// </summary>
        public static int GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// 根据guid生成字符串(16位)
        /// 例如：aded0a2611f8aa4a
        /// </summary>
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
        /// 随机器生成数字和字母组合,区分大小写
        /// </summary>
        public static string GenerateRandom(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(62);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 随机生成只有字符的组合，区分大小写
        /// </summary>
        public static string GenerateRandomToChars(int Length)
        {
            System.Text.StringBuilder newRandom = new System.Text.StringBuilder(52);
            Random rd = new Random();
            for (int i = 0; i < Length; i++)
            {
                newRandom.Append(constantChar[rd.Next(52)]);
            }
            return newRandom.ToString();
        }

        /// <summary>
        /// 随机生成只有字符的组合，不区分大小写
        /// </summary>
        public static string GetRand(int len)
        {
            Random rnd = new Random();
            string strTemp = "";
            int rndNum;
            for (int i = 0; i < len; i++)
            {
                rndNum = rnd.Next(35);
                //得0~9的随机数
                strTemp += arrString[rndNum];
            }
            return strTemp;
        }

        /// <summary>
        /// 根据GUID生成唯一字符标示
        /// </summary>
        public static string GetGuidString(int len)
        {
            return System.Guid.NewGuid().ToString().Substring(0, len);
        }
    }
}