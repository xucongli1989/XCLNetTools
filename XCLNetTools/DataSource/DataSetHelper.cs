using System.Collections.Generic;
using System.Data;

namespace XCLNetTools.DataSource
{
    /// <summary>
    /// dataset相关
    /// </summary>
    public static class DataSetHelper
    {
        /// <summary>
        /// 将dataset的第一个datatable转为list
        /// </summary>
        /// <param name="ds">要转换的数据</param>
        /// <returns>list</returns>
        public static IList<T> DataSetToIList<T>(DataSet ds) where T : new()
        {
            return XCLNetTools.Generic.ListHelper.DataSetToList<T>(ds);
        }

        /// <summary>
        /// 将dataTable转为list
        /// </summary>
        /// <param name="dt">要转换的数据</param>
        /// <returns>list</returns>
        public static IList<T> DataTableToIList<T>(DataTable dt) where T : new()
        {
            return XCLNetTools.Generic.ListHelper.DataTableToList<T>(dt);
        }

        /// <summary>
        /// 将dataset的第一个datatable转为list
        /// </summary>
        /// <param name="ds">要转换的数据</param>
        /// <returns>list</returns>
        public static List<T> DataSetToList<T>(DataSet ds) where T : new()
        {
            return XCLNetTools.Generic.ListHelper.DataSetToList<T>(ds) as List<T>;
        }

        /// <summary>
        /// 将dataTable转为list
        /// </summary>
        /// <param name="dt">要转换的数据</param>
        /// <returns>list</returns>
        public static List<T> DataTableToList<T>(DataTable dt) where T : new()
        {
            return XCLNetTools.Generic.ListHelper.DataTableToList<T>(dt) as List<T>;
        }
    }
}