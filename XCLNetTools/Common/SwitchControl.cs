using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 开关控制
    /// </summary>
    public class SwitchControl
    {
        /// <summary>
        /// 将T/F转为bool值
        /// </summary>
        /// <param name="val">T或F</param>
        /// <returns>T:true,F:false</returns>
        public static bool TFToBool(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return false;
            }
            val = val.ToUpper();
            if (val == "T")
            {
                return true;
            }
            if (val == "F")
            {
                return false;
            }
            throw new ArgumentOutOfRangeException("val", "val只能为：T或F");
        }
    }
}
