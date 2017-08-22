/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace XCLNetTools.Generic
{
    /// <summary>
    /// List操作类
    /// </summary>
    public static class ListHelper
    {
        /// <summary>
        /// 根据步长，将一个总List拆分为多个子List
        /// </summary>
        /// <param name="step">每个子list最多的项数</param>
        /// <param name="lst">主list</param>
        /// <returns>分拆后的结果list</returns>
        public static List<List<T>> SplitListByStep<T>(int step, List<T> lst)
        {
            List<List<T>> newList = null;
            if (null != lst && lst.Count > 0)
            {
                newList = new List<List<T>>();
                int max = lst.Count;
                int times = (int)Math.Ceiling(max * 1.00 / step);
                for (int i = 1; i <= times; i++)
                {
                    int c = step;
                    if (i == times && ((max % step) != 0))
                    {
                        c = max % step;
                    }
                    newList.Add(lst.GetRange(step * (i - 1), c));
                }
            }
            return newList;
        }

        /// <summary>
        /// 将指定字符串用指定分隔符分开存到list中
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔字符</param>
        /// <returns>list</returns>
        public static List<string> GetStrSplitList(string str, char speater)
        {
            List<string> result = null;
            if (!string.IsNullOrEmpty(str))
            {
                result = str.Split(speater).ToList();
            }
            return result;
        }

        /// <summary>
        /// 将list中的项拼接字符串
        /// </summary>
        /// <param name="lst">要操作的list</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns>字符串结果值</returns>
        public static string GetStringByList<T>(List<T> lst, string splitChar)
        {
            string str = string.Empty;
            if (null != lst && lst.Count > 0)
            {
                str = string.Join(splitChar, lst.ConvertAll(k => k.ToString()).ToArray());
            }
            return str;
        }

        /// <summary>
        /// 将dataset的第一个datatable转为list
        /// </summary>
        /// <param name="ds">要转换的数据</param>
        /// <returns>list</returns>
        public static IList<T> DataSetToList<T>(DataSet ds) where T : new()
        {
            if (null == ds || null == ds.Tables || ds.Tables.Count == 0)
            {
                return null;
            }
            return XCLNetTools.Generic.ListHelper.DataTableToList<T>(ds.Tables[0]);
        }

        /// <summary>
        /// 将dataTable转为list
        /// </summary>
        /// <param name="dt">要转换的数据</param>
        /// <returns>list</returns>
        public static IList<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            if (null == dt || null == dt.Rows || dt.Rows.Count == 0)
            {
                return null;
            }
            IList<T> ts = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (!dt.Columns.Contains(pi.Name) || !pi.CanWrite)
                    {
                        continue;
                    }
                    var value = dr[pi.Name];
                    if (value != DBNull.Value)
                    {
                        pi.SetValue(t, value, null);
                    }
                }
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 将DataRow转为Model
        /// </summary>
        /// <typeparam name="T">要转换的类型</typeparam>
        /// <param name="dr">DataRow对象</param>
        /// <returns>实体</returns>
        public static T DataRowToModel<T>(DataRow dr) where T : new()
        {
            if (null == dr)
            {
                return default(T);
            }
            var dt = dr.Table.Clone();
            dt.Rows.Add(dr.ItemArray);
            var lst = DataTableToList<T>(dt);
            if (lst.IsNotNullOrEmpty())
            {
                return lst[0];
            }
            return default(T);
        }
    }
}