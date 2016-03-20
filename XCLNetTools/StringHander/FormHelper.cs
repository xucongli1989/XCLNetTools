/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

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
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
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
        /// <param name="name">参数名</param>
        /// <returns>参数值数组</returns>
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
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static string GetString(string name)
        {
            return FormHelper.GetQueryOrFormString(name);
        }

        /// <summary>
        /// 获取数组参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static string[] GetStringArray(string name)
        {
            return FormHelper.GetQueryOrFormStringArray(name);
        }

        #endregion get string

        #region get byte

        /// <summary>
        /// 获取byte?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static byte? GetByteNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToByteNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取byte?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static byte? GetByteNull(string name, byte? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToByteNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取byte参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static byte GetByte(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToByte(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取byte参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static byte GetByte(string name, byte defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToByte(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取byte参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<byte> GetByteList(string name)
        {
            List<byte> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToByte(k));
            }
            return lst;
        }

        #endregion get byte

        #region get int

        /// <summary>
        /// 获取int?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static int? GetIntNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToIntNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取int?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static int? GetIntNull(string name, int? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToIntNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取int参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static int GetInt(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取int参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static int GetInt(string name, int defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取int参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<int> GetIntList(string name)
        {
            List<int> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToInt(k));
            }
            return lst;
        }

        #endregion get int

        #region get short

        /// <summary>
        /// 获取short?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static short? GetShortNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToShortNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取short?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static short? GetShortNull(string name, short? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToShortNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取short参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static short GetShort(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToShort(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取short参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static short GetShort(string name, short defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToShort(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取short参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<short> GetShortList(string name)
        {
            List<short> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToShort(k));
            }
            return lst;
        }

        #endregion get short

        #region get long

        /// <summary>
        /// 获取Long?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static long? GetLongNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLongNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取Long?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static long? GetLongNull(string name, long? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLongNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取long参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static long GetLong(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLong(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取long参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static long GetLong(string name, long defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToLong(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取long参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
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

        #region get float

        /// <summary>
        /// 获取float?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static float? GetFloatNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToFloatNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取float?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static float? GetFloatNull(string name, float? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToFloatNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取float参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static float GetFloat(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToFloat(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取float参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static float GetFloat(string name, float defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToFloat(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取float参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<float> GetFloatList(string name)
        {
            List<float> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToFloat(k));
            }
            return lst;
        }

        #endregion get float

        #region get double

        /// <summary>
        /// 获取double?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static double? GetDoubleNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDoubleNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取double?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static double? GetDoubleNull(string name, double? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDoubleNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取double参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static double GetDouble(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDouble(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取double参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static double GetDouble(string name, double defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDouble(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取double参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<double> GetDoubleList(string name)
        {
            List<double> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToDouble(k));
            }
            return lst;
        }

        #endregion get double

        #region get bool

        /// <summary>
        /// 获取bool?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static bool? GetBoolNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToBoolNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取bool?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static bool? GetBoolNull(string name, bool? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToBoolNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取bool参数，默认false
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static bool GetBool(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToBool(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取bool参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static bool GetBool(string name, bool defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToBool(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取bool参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<bool> GetBoolList(string name)
        {
            List<bool> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToBool(k));
            }
            return lst;
        }

        #endregion get bool

        #region get decimal

        /// <summary>
        /// 获取decimal?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static decimal? GetDecimalNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimalNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取decimal?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static decimal? GetDecimalNull(string name, decimal? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimalNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取decimal参数，默认0
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static decimal GetDecimal(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取decimal参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static decimal GetDecimal(string name, decimal defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取decimal参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
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

        #region get datetime

        /// <summary>
        /// 获取DateTime?参数
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static DateTime? GetDateTimeNull(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取DateTime?参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static DateTime? GetDateTimeNull(string name, DateTime? defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取DateTime参数，默认'0001/1/1 0:00:00'
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static DateTime GetDateTime(string name)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDateTime(FormHelper.GetString(name));
        }

        /// <summary>
        /// 获取DateTime参数，默认defaultValue
        /// </summary>
        /// <param name="name">参数名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        public static DateTime GetDateTime(string name, DateTime defaultValue)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDateTime(FormHelper.GetString(name), defaultValue);
        }

        /// <summary>
        /// 获取DateTime参数数组
        /// </summary>
        /// <param name="name">参数名</param>
        /// <returns>参数值</returns>
        public static List<DateTime> GetDateTimeList(string name)
        {
            List<DateTime> lst = null;
            string[] strArr = FormHelper.GetQueryOrFormStringArray(name);
            if (null != strArr && strArr.Length > 0)
            {
                lst = strArr.ToList().ConvertAll(k => XCLNetTools.Common.DataTypeConvert.ToDateTime(k));
            }
            return lst;
        }

        #endregion get datetime

        #region 其它

        /// <summary>
        /// 根据参数的name前缀，获取它的value数组
        /// </summary>
        /// <param name="preName">参数name值的前缀</param>
        /// <returns>参数值</returns>
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
        /// <returns>hidden字符串</returns>
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
        /// <param name="name">hidden名</param>
        /// <param name="value">hidden值</param>
        /// <returns>hidden字符串</returns>
        public static string CreateHiddenHtml(string name, string value)
        {
            return string.Format("<input type='hidden' name='{0}' value='{1}'/>", name, value);
        }

        /// <summary>
        /// 获取QueryString的参数序列化字符串（也就是a=b&amp;c=d的形式）
        /// </summary>
        /// <returns>参数序列化的结值</returns>
        public static string GetQuerySerializeString()
        {
            var req = HttpContext.Current.Request;
            if (null == req.QueryString || req.QueryString.Count == 0)
            {
                return string.Empty;
            }
            List<string> lst = new List<string>();
            string[] vals = new string[] { };
            for (int i = 0; i < req.QueryString.Count; i++)
            {
                vals = req.QueryString.GetValues(i);
                for (int j = 0; j < vals.Length; j++)
                {
                    lst.Add(string.Format("{0}={1}", req.QueryString.GetKey(i), vals[j]));
                }
            }
            return string.Join("&", lst.ToArray());
        }

        /// <summary>
        /// 获取Form的参数序列化字符串（也就是a=b&amp;c=d的形式）
        /// </summary>
        /// <returns>参数序列化的结值</returns>
        public static string GetFromSerializeString()
        {
            var req = HttpContext.Current.Request;
            if (null == req.Form || req.Form.Count == 0)
            {
                return string.Empty;
            }
            List<string> lst = new List<string>();
            string[] vals = new string[] { };
            for (int i = 0; i < req.Form.Count; i++)
            {
                vals = req.Form.GetValues(i);
                for (int j = 0; j < vals.Length; j++)
                {
                    lst.Add(string.Format("{0}={1}", req.Form.GetKey(i), vals[j]));
                }
            }
            return string.Join("&", lst.ToArray());
        }

        /// <summary>
        /// 获取QueryString和Form的参数序列化字符串（也就是a=b&amp;c=d的形式）
        /// </summary>
        /// <returns>参数序列化的结值</returns>
        public static string GetQueryFromSerializeString()
        {
            string q = FormHelper.GetQuerySerializeString();
            string f = FormHelper.GetFromSerializeString();
            if (string.IsNullOrEmpty(q))
            {
                return f;
            }
            if (string.IsNullOrEmpty(f))
            {
                return q;
            }
            return string.Join("&", new string[] { f, q });
        }

        #endregion 其它
    }
}