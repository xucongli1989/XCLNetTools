/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace XCLNetTools.DataBase
{
    /// <summary>
    /// sql处理类
    /// </summary>
    public static class SQLLibrary
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

        /// <summary>
        /// 生成分页查询的sql语句
        /// 注：sql中的out参数：TotalCount（总记录数）
        /// </summary>
        /// <param name="condition">分页参数</param>
        /// <returns>分页的sql查询语句</returns>
        public static string CreatePagerQuerySqlString(XCLNetTools.Entity.SqlPagerConditionEntity condition)
        {
            string strSql = string.Empty;

            if (null == condition)
            {
                return strSql;
            }

            if (string.IsNullOrWhiteSpace(condition.OrderBy))
            {
                throw new ArgumentException("请指定排序参数！");
            }

            if (condition.DatabaseType == Enum.CommonEnum.DatabaseTypeEnum.MSSQL)
            {
                strSql = string.Format(@"
                                                        DECLARE @_pageIndex INT={0}
                                                        DECLARE @_pageSize INT={1}
                                                        DECLARE @_maxPageIndex INT=0
                                                        DECLARE @Start INT=0
                                                        DECLARE @End INT=0

                                                        --获取总记录数
                                                        SELECT @TotalCount=COUNT(1) FROM {2} WITH(NOLOCK)  {4}

                                                        IF(@_pageIndex<=0) SET @_pageIndex=1
                                                        IF(@_pageSize<=0) SET @_pageSize=10

                                                        --获取最大页数
                                                        SET @_maxPageIndex=CEILING((1.0*@TotalCount)/@_pageSize)
                                                        IF(@_pageIndex>@_maxPageIndex) SET @_pageIndex=@_maxPageIndex

                                                        SET @Start=(@_pageIndex-1)*@_pageSize+1
                                                        SET @End=@Start+@_pageSize-1

                                                        --分页
                                                        ;WITH Data AS
                                                        (
                                                            SELECT
                                                            {5},
                                                            ROW_NUMBER() OVER (ORDER BY {3}) AS RowNumber
                                                            FROM {2} WITH(NOLOCK)
                                                            {4}
                                                        )
                                                        SELECT
                                                        {5}
                                                        FROM Data
                                                        WHERE RowNumber >= @Start AND RowNumber <= @End",
                                                        condition.PageIndex,//{0}
                                                        condition.PageSize,//{1}
                                                        condition.TableName,//{2}
                                                        condition.OrderBy,//{3}
                                                        string.IsNullOrWhiteSpace(condition.Where) ? string.Empty : (" where " + condition.Where),//{4}
                                                        null == condition.FieldNameList || condition.FieldNameList.Count == 0 ? " * " : string.Join(",", condition.FieldNameList.ToArray())//{5}
                                    );
            }

            return strSql;
        }
    }
}