/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.XML
{
    /// <summary>
    /// Web.config 操作类
    /// </summary>
    public static class ConfigClass
    {
        /// <summary>
        /// 获取统一的 AppSettings 前缀环境变量值
        /// </summary>
        public static string GetEnvXCLNetToolsAppSettingsPrefix()
        {
            return Environment.GetEnvironmentVariable("XCLNetToolsAppSettingsPrefix") ?? "".Trim();
        }

        /// <summary>
        /// 获取 appSettings 配置文件中的字符串值。注意：如果环境变量中包含 XCLNetToolsAppSettingsPrefix ，则默认会在 key 的前面自动加上这个变量值。用于在不修改代码的情况下，根据不同环境获取不同的 key 值。
        /// </summary>
        public static string GetConfigString(string key)
        {
            key = $"{GetEnvXCLNetToolsAppSettingsPrefix()}{key}";
            var cfgName = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection("appSettings");
            return cfgName[key];
        }

        /// <summary>
        /// 取得配置文件中 默认节点的 浮点数型
        /// </summary>
        public static decimal GetConfigDecimal(string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(GetConfigString(key));
        }

        /// <summary>
        /// 得到配置文件中的默认节点配置int信息
        /// </summary>
        public static int GetConfigInt(string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(GetConfigString(key));
        }
    }
}