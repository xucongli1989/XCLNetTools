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



using LitJson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace XCLNetTools.Serialize
{
    /// <summary>
    /// LitJson帮助类
    /// </summary>
    public class LitJson
    {
        /// <summary>
        /// 对象转为json
        /// </summary>
        public static string ConvertObjectToJson(object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        #region 处理DataTable

        /// <summary>
        /// DataTable转为js的数组形式，若为空，则返回[]
        /// </summary>
        public static string ConvertDataTableToArray(System.Data.DataTable dt)
        {
            string str = string.Empty;
            if (null != dt && dt.Rows.Count > 0)
            {
                List<string> strTemp = new List<string>();
                string columnName, dataType, value;//列名，字段类型，值
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    StringBuilder strBuild = new StringBuilder();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        columnName = dt.Columns[j].ColumnName;
                        dataType = dt.Columns[j].DataType.Name;
                        value = Convert.ToString(dt.Rows[i][j]);
                        JsonWriter jw = new JsonWriter();
                        jw.WriteObjectStart();
                        jw.WritePropertyName(columnName);
                        WriteFormat(jw, value, dataType);
                        jw.WriteObjectEnd();
                        strBuild.Append(jw.ToString());
                    }
                    strTemp.Add(strBuild.ToString().Replace("}{", ","));
                }
                str = String.Join(",", strTemp.ToArray());
            }
            return string.Format("[{0}]", str);
        }

        /// <summary>
        /// DataTable转为json
        /// </summary>
        public static string ConvertDataTableToJson(DataTable dt, string jsonName)
        {
            return string.Format(@"{{""{0}"":{1}}}", jsonName, LitJson.ConvertDataTableToArray(dt));
        }

        #endregion 处理DataTable

        #region 处理DataSet

        /// <summary>
        /// DataSet转为js的数组形式，若为空，则返回[]
        /// </summary>
        public static string ConvertDataSetToArray(System.Data.DataSet ds)
        {
            string str = string.Empty;
            if (null != ds && ds.Tables.Count > 0)
            {
                List<string> strTemp = new List<string>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    strTemp.Add(LitJson.ConvertDataTableToArray(ds.Tables[i]));
                }
                str = String.Join(",", strTemp.ToArray());
            }
            return string.Format("[{0}]", str);
        }

        /// <summary>
        /// DataSet转为json
        /// </summary>
        public static string ConvertDataSetToJson(DataSet ds, string jsonName)
        {
            return string.Format(@"{{""{0}"":{1}}}", jsonName, LitJson.ConvertDataSetToArray(ds));
        }

        #endregion 处理DataSet

        #region 处理List

        /// <summary>
        /// List转为json
        /// </summary>
        public static string ConvertListToJson<T>(IList<T> lst, string jsonName)
        {
            return string.Format(@"{{""{0}"":{1}}}", jsonName, LitJson.ConvertListToArray<T>(lst));
        }

        /// <summary>
        /// List转为数组，若为空，则直接返回[]
        /// </summary>
        public static string ConvertListToArray<T>(IList<T> lst)
        {
            string str = string.Empty;
            if (null != lst && lst.Count > 0)
            {
                List<string> strTemp = new List<string>();
                foreach (T m in lst)
                {
                    strTemp.Add(LitJson.ConvertObjectToJson(m));
                }
                str = String.Join(",", strTemp.ToArray());
            }
            return string.Format("[{0}]", str);
        }

        #endregion 处理List

        /// <summary>
        /// 输出指定格式的数据
        /// <param name="dataType">数据类型</param>
        /// <param name="jw">JsonWriter对象</param>
        /// <param name="value">要转换的值</param>
        /// </summary>
        private static void WriteFormat(JsonWriter jw, string value, string dataType)
        {
            switch (dataType)
            {
                case "Int32":
                    Int32 int32Value;
                    Int32.TryParse(value, out int32Value);
                    jw.Write(int32Value);
                    break;

                case "Boolean":
                    Boolean booleanValue;
                    Boolean.TryParse(value, out booleanValue);
                    jw.Write(booleanValue);
                    break;

                case "Decimal":
                    Decimal decimalValue;
                    Decimal.TryParse(value, out decimalValue);
                    jw.Write(decimalValue);
                    break;

                case "Double":
                    Double doubleValue;
                    Double.TryParse(value, out doubleValue);
                    jw.Write(doubleValue);
                    break;

                default:
                    jw.Write(value);
                    break;
            }
        }
    }
}