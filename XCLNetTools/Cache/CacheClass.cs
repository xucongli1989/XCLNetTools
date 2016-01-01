using System;
using System.Collections;
using System.Web;

namespace XCLNetTools.Cache
{
    /// <summary>
    /// 缓存相关的操作类
    /// </summary>
    public class CacheClass
    {
        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">缓存key名</param>
        /// <returns>该缓存的值</returns>
        public static object GetCache(string CacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">缓存key名</param>
        /// <param name="objObject">缓存key值</param>
        public static void SetCache(string CacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">缓存key名</param>
        /// <param name="objObject">缓存key值</param>
        /// <param name="absoluteExpiration">所插入对象将到期并被从缓存中移除的时间</param>
        /// <param name="slidingExpiration">最后一次访问所插入对象时与该对象到期时之间的时间间隔</param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 删除指定缓存
        /// </summary>
        /// <param name="key">缓存key名</param>
        public static void Clear(string key)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Remove(key);
        }

        /// <summary>
        /// 指定缓存是否存在
        /// </summary>
        /// <param name="key">缓存名</param>
        /// <returns>true:存在</returns>
        public static bool Exists(string key)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[key] != null;
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache _cache = HttpRuntime.Cache;
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            while (CacheEnum.MoveNext())
            {
                _cache.Remove(CacheEnum.Key.ToString());
            }
        }
    }
}