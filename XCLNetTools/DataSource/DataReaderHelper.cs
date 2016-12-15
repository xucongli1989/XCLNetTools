/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace XCLNetTools.DataSource
{
    /// <summary>
    /// DataReader帮助类
    /// </summary>
    public class DataReaderHelper
    {
        /// <summary>
        /// 将DataReader转为list
        /// </summary>
        /// <param name="dr">要转换的数据</param>
        /// <returns>list</returns>
        public static IList<T> DataReaderToList<T>(IDataReader dr) where T : new()
        {
            if (null == dr)
            {
                return null;
            }
            IList<T> lst = new List<T>();
            var fields = new List<string>();
            using (dr)
            {
                for (var i = 0; i < dr.FieldCount; i++)
                {
                    fields.Add(dr.GetName(i));
                }
                while (dr.Read())
                {
                    var t = new T();
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        if (!fields.Contains(pi.Name) || !pi.CanWrite)
                        {
                            continue;
                        }
                        var value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, value, null);
                        }
                    }
                    lst.Add(t);
                }
            }
            return lst;
        }

        /// <summary>
        /// 将DataReader转为实体
        /// </summary>
        /// <param name="dr">要转换的数据</param>
        /// <returns>list</returns>
        public static T DataReaderToEntity<T>(IDataReader dr) where T : new()
        {
            var lst = DataReaderToList<T>(dr);
            if (null == lst || lst.Count == 0)
            {
                return default(T);
            }
            return lst[0];
        }
    }
}