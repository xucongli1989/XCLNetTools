using System;
using System.Collections.Generic;
using System.Data;

namespace XCLNetTools.DataSource
{
    /// <summary>
    /// datatable相关
    /// </summary>
    public static class DataTableHelper
    {
        /// <summary>
        /// 根据dt和指定行号和列名，返回该列的列号.若找不到该列，则返回-1
        /// </summary>
        /// <param name="dt">dataTable</param>
        /// <param name="rowIndex">行号</param>
        /// <param name="colName">列名</param>
        /// <returns>列号</returns>
        public static int GetColIndex(DataTable dt, int rowIndex, string colName)
        {
            int s = -1;
            if (null != dt && dt.Rows.Count >= rowIndex)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (string.Equals(dt.Rows[rowIndex][i].ToString(), colName))
                    {
                        s = i;
                        break;
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// 将lst转为单列的datatable，如果lst没有数据，则返回一个空的datatable
        /// </summary>
        /// <typeparam name="source">要转换的列类型</typeparam>
        /// <typeparam name="target">转换后的列类型</typeparam>
        /// <param name="lst">要转换的数据源</param>
        /// <param name="columnName">datatable列名，默认为“ID”</param>
        /// <returns>转换后的datatable</returns>
        public static DataTable ToSingleColumnDataTable<source, target>(List<source> lst, string columnName = "ID")
        {
            DataTable tb = new DataTable();
            tb.Columns.Add(columnName, typeof(target));
            if (null == lst || lst.Count == 0)
            {
                return tb;
            }
            lst.ForEach(k =>
            {
                var row = tb.NewRow();
                row[columnName] = k;
                tb.Rows.Add(row);
            });
            return tb;
        }

        /// <summary>
        /// 将List转换成DataTable
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <returns>datatable</returns>
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            Type nullableType;
            for (int i = 0; i < properties.Count; i++)
            {
                var property = properties[i];
                nullableType = Nullable.GetUnderlyingType(property.PropertyType);
                dt.Columns.Add(property.Name, null == nullableType ? property.PropertyType : nullableType);
            }
            object[] values = new object[properties.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = properties[i].GetValue(item);
                }
                dt.Rows.Add(values);
            }
            return dt;
        }
    }
}