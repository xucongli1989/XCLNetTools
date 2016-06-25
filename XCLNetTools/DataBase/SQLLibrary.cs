/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Collections.Generic;
using System.Linq;

namespace XCLNetTools.DataBase
{
    /// <summary>
    /// sql处理类
    /// </summary>
    public class SQLLibrary
    {
        /// <summary>
        /// 使用'and'合并sql
        /// </summary>
        /// <param name="whereList">where的各个条件，如：a='abc',b='xxx'</param>
        /// <returns>and合并后的新的sql</returns>
        public static string JoinWithAnd(List<string> whereList)
        {
            if (null != whereList && whereList.Count > 0)
            {
                whereList = whereList.Where(k => !string.IsNullOrWhiteSpace(k)).ToList();
            }
            if (null == whereList || whereList.Count == 0)
            {
                return string.Empty;
            }
            return string.Join(" and ", whereList.ToArray());
        }
    }
}