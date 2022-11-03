using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XCLNetTools.Generic;

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
        /// 将List转换成DataTable，如果数据为空，则返回一个没有内容的DataTable
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <returns>datatable</returns>
        public static DataTable ToDataTable<T>(IList<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties = System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();
            if (null == data)
            {
                return dt;
            }
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

        /// <summary>
        /// 将List转换成DataSet
        /// </summary>
        /// <param name="data">要转换的数据</param>
        /// <returns>dataset</returns>
        public static DataSet ToDataSet<T>(IList<T> data)
        {
            var dt = DataTableHelper.ToDataTable(data);
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 检查指定的列名是否在 DataTable 的 ColumnName 属性中存在
        /// </summary>
        public static XCLNetTools.Entity.MethodResult<bool> CheckColumnNames(DataTable dt, List<string> mustFindColumnNames)
        {
            var msg = new XCLNetTools.Entity.MethodResult<bool>();
            msg.IsSuccess = false;

            if (null == dt || null == dt.Rows)
            {
                msg.IsSuccess = false;
                msg.Message = "请指定有效的数据源！";
                return msg;
            }
            if (mustFindColumnNames.IsNullOrEmpty())
            {
                msg.IsSuccess = false;
                msg.Message = "请指定需要检查的列名！";
                return msg;
            }
            var dtColumnNames = new List<string>();
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                dtColumnNames.Add(dt.Columns[i].ColumnName);
            }
            var notFoundNames = mustFindColumnNames.Where(k => !dtColumnNames.Contains(k)).ToList();
            if (notFoundNames.IsNotNullOrEmpty())
            {
                msg.IsSuccess = false;
                msg.Message = "在数据源中未找到这些列名：" + string.Join("、", notFoundNames);
                return msg;
            }

            msg.IsSuccess = true;
            return msg;
        }

        /// <summary>
        /// 合并 DataTable 中的标题行（Excel 转为 DataTable 时，如果此列是合并的单元格，则只有最左边的列有值，其它列是空值。这个方法会将最后一行作为唯一的标题行，标题的名称为父标题合并后的值）。
        /// 示例：前三行为标题行，第一行为 a，第二行为 b，第三行为 c，则在处理后的第三行的内容为 a_b_c
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="titleRowCount">前几行是标题行</param>
        public static void MergeTitleRows(DataTable dt, int titleRowCount)
        {
            var rowCount = dt.Rows.Count;
            var columnCount = dt.Columns.Count;

            if (rowCount < titleRowCount)
            {
                return;
            }

            //填充前面合并列的单元格为前一个单元格的内容
            for (var i = 0; i < titleRowCount - 1; i++)
            {
                for (var c = 1; c < columnCount; c++)
                {
                    var cellValue = dt.Rows[i][c].ToString();
                    if (string.IsNullOrWhiteSpace(cellValue))
                    {
                        dt.Rows[i][c] = dt.Rows[i][c - 1].ToString();
                    }
                }
            }

            //更新最后一行中的单元格内容
            var titleRow = dt.Rows[titleRowCount - 1];
            for (var c = 0; c < columnCount; c++)
            {
                var cellValues = new List<string>();
                for (var i = 0; i < titleRowCount; i++)
                {
                    var val = dt.Rows[i][c].ToString()?.Trim();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        cellValues.Add(val);
                    }
                }
                titleRow[c] = string.Join("_", cellValues).Trim('_');
            }
        }

        /// <summary>
        /// 使用行中的数据作为当前 DataTable 的所有列名
        /// </summary>
        public static void UpdateColumnNameFromRow(DataTable dt, int rowIndex)
        {
            var rowCount = dt.Rows.Count;
            if (rowIndex <= -1 || rowIndex >= rowCount)
            {
                return;
            }
            var row = dt.Rows[rowIndex];
            for (var i = 0; i < dt.Columns.Count; i++)
            {
                var val = row[i].ToString()?.Trim();
                if (!string.IsNullOrWhiteSpace(val))
                {
                    dt.Columns[i].ColumnName = val;
                }
            }
        }

        /// <summary>
        /// 将字典列表转换为 DataTable（字典中的每一个 Key 是字段名，Value 是该字段的值）
        /// </summary>
        public static DataTable ConvertDicListToDataTable(List<IDictionary<string, object>> lst)
        {
            var dt = new DataTable();
            if (lst.IsNullOrEmpty())
            {
                return dt;
            }
            lst.ForEach(dic =>
            {
                var dr = dt.NewRow();
                if (null == dic)
                {
                    return;
                }
                dic.Keys.ToList().ForEach(key =>
                {
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        return;
                    }
                    if (!dt.Columns.Contains(key))
                    {
                        dt.Columns.Add(key, typeof(string));
                    }
                    dr[key] = Convert.ToString(dic[key]);
                });
                dt.Rows.Add(dr);
            });
            return dt;
        }
    }
}