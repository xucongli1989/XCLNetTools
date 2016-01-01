using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// form表单相关
    /// </summary>
    public class FormHelper
    {
        #region 主方法

        /// <summary>
        /// 获取request参数，先取querystring,若没有，再取formstring，若没有，则为string.Empty
        /// </summary>
        private static string GetQueryOrFormString(string name)
        {
            string str = null;
            HttpRequest request = HttpContext.Current.Request;
            str = request.QueryString[name];
            if (null != str)
            {
                return str;
            }
            str = request.Form[name];
            if (null != str)
            {
                return str;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取request参数数组，先取querystring,若没有，再取formstring，若没有，则为null
        /// </summary>
        private static string[] GetQueryOrFormStringArray(string name)
        {
            string[] str = null;
            HttpRequest request = HttpContext.Current.Request;
            str = request.QueryString.GetValues(name);
            if (null != str)
            {
                return str;
            }
            str = request.Form.GetValues(name);
            if (null != str)
            {
                return str;
            }
            return null;
        }

        #endregion 主方法

        #region get string

        /// <summary>
        /// 获取string参数
        /// </summary>
        public static string GetString(string name)
        {
            return FormHelper.GetQueryOrFormString(name);
        }

        /// <summary>
        /// 获取数组参数
        /// </summary>
        public static string[] GetStringArray(string name)
        {
            return FormHelper.GetQueryOrFormStringArray(name);
        }

        #endregion get string

        #region get int

        /// <summary>
        /// 获取int?参数
        /// </summary>
        public static int? GetIntNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToIntNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取int?参数，默认defaultValue
        /// </summary>
        public static int? GetIntNull(string name, int? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToIntNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取int参数，默认0
        /// </summary>
        public static int GetInt(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取int参数，默认defaultValue
        /// </summary>
        public static int GetInt(string name, int defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(FormHelper.GetString(name), defaultValue);
        }

        #endregion get int

        #region get long

        /// <summary>
        /// 获取Long?参数
        /// </summary>
        public static long? GetLongNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLongNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取Long?参数，默认defaultValue
        /// </summary>
        public static long? GetLongNull(string name, long? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLongNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取long参数，默认0
        /// </summary>
        public static long GetLong(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLong(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取long参数，默认defaultValue
        /// </summary>
        public static long GetLong(string name, long defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLong(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取long参数数组
        /// </summary>
        public static List<long> GetLongList(string name)
        {
            List<long> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToLong(k));
            }
            return lst;
        }

        #endregion get long

        #region get decimal

        /// <summary>
        /// 获取decimal?参数
        /// </summary>
        public static decimal? GetDecimalNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimalNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取decimal?参数，默认defaultValue
        /// </summary>
        public static decimal? GetDecimalNull(string name, decimal? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimalNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取decimal参数，默认0
        /// </summary>
        public static decimal GetDecimal(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取decimal参数，默认defaultValue
        /// </summary>
        public static decimal GetDecimal(string name, decimal defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取decimal参数数组
        /// </summary>
        public static List<decimal> GetDecimalList(string name)
        {
            List<decimal> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToDecimal(k));
            }
            return lst;
        }

        #endregion get decimal

        #region 其它

        /// <summary>
        /// 根据表单控件的name前缀，获取它的value数组
        /// 注意：请保证已存在的name值之间没有包含关系（如：txtName和txtNameFirst），否则数据会紊乱！
        /// </summary>
        /// <param name="preName">name值的前缀</param>
        public static string[] GetFromParamsValudByPre(string preName)
        {
            HttpContext context = HttpContext.Current;
            IList<string> str = new List<string>();
            if (!string.IsNullOrEmpty(preName))
            {
                NameValueCollection ps = context.Request.Params;
                foreach (var m in ps.AllKeys)
                {
                    if (m.IndexOf(preName) == 0)
                    {
                        str.Add(ps[m]);
                    }
                }
            }
            return str.ToArray();
        }

        /// <summary>
        /// 把lst中的项生成input hidden标签
        /// </summary>
        /// <param name="lst">Key:hidden的name名字；Value:hidden的value</param>
        /// <returns></returns>
        public static string CreateHiddenHtml(List<XCLNetTools.Entity.TextValue> lst)
        {
            StringBuilder str = new StringBuilder();
            if (null != lst && lst.Count > 0)
            {
                foreach (var m in lst)
                {
                    str.AppendFormat(CreateHiddenHtml(m.Text, m.Value));
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 返回hidden
        /// </summary>
        public static string CreateHiddenHtml(string name, string value)
        {
            return string.Format("<input type='hidden' name='{0}' value='{1}'/>", name, value);
        }

        #endregion 其它
    }
}