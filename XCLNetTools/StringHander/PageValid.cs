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


using System.Text.RegularExpressions;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// 页面数据校验类
    /// </summary>
    public class PageValid
    {
        private static Regex RegPhone = new Regex("^[0-9]+[-]?[0-9]+[-]?[0-9]$");
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegURL = new Regex(@"^http[s]?:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\""\""])*$");

        /// <summary>
        /// 构造函数
        /// </summary>
        public PageValid()
        {
        }

        #region 数字字符串检查

        /// <summary>
        /// 是否为手机号
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsPhone(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegPhone.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否为纯数字（不带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsNumber(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否数字（可带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsNumberSign(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数（不带正负）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsDecimal(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }

        /// <summary>
        /// 是否是浮点数（可带正负号）
        /// </summary>
        /// <param name="inputData">待判断的值</param>
        /// <returns>判断结果</returns>
        public static bool IsDecimalSign(string inputData)
        {
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegDecimalSign.Match(inputData);
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
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegCHZN.Match(inputData);
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
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegEmail.Match(inputData);
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
            if (string.IsNullOrEmpty(inputData))
            {
                return false;
            }
            Match m = RegURL.Match(inputData);
            return m.Success;
        }

        #endregion URL
    }
}