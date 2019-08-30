/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Text.RegularExpressions;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// 页面数据校验类
    /// </summary>
    public static class DataCheck
    {
        #region 数字字符串检查

        /// <summary>
        /// 是否为纯数字（不带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsNumber(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字（可带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsNumberSign(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数（不带正负）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsDecimal(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数（可带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsDecimalSign(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion 数字字符串检查

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsHasCHZN(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegCHZN.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 检测是否有全部是中文字符
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsHasCHZNAll(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegCHZNAll.Match(inputData);
            return m.Success;
        }

        #endregion 中文检测

        #region 邮件地址

        /// <summary>
        /// 是否是email
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsEmail(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion 邮件地址

        #region 日期格式判断

        /// <summary>
        /// 日期格式字符串判断
        /// </summary>
        /// <param name="str">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsDateTime(string str)
        {
            return null != XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(str);
        }

        #endregion 日期格式判断

        #region URL

        /// <summary>
        /// 是否为URL地址
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegURL.Match(inputData);
            return m.Success;
        }

        #endregion URL

        #region 身份证

        /// <summary>
        /// 是否为国内居民身份证号码
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsIDCard(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegIDCard.Match(inputData);
            return m.Success;
        }

        #endregion 身份证

        #region 编码格式

        /// <summary>
        /// 是否为Base64编码
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsBase64(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegBase64.Match(inputData);
            return m.Success;
        }

        #endregion 编码格式

        #region 电话号码

        /// <summary>
        /// 是否为国内座机号码
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsPhone(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegPhone.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否为国内手机号码
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsMobile(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegMobile.Match(inputData);
            return m.Success;
        }

        #endregion 电话号码

        #region 其它

        /// <summary>
        /// 是否为国内邮政编码
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsPostcode(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegPostcode.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否为用户名
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsUserName(string inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData))
            {
                return false;
            }
            Match m = XCLNetTools.Common.Consts.RegUserName.Match(inputData);
            return m.Success;
        }

        #endregion 其它
    }
}