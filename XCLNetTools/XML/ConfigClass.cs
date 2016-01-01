using System;

namespace XCLNetTools.XML
{
    /// <summary>
    /// Web.config 操作类
    /// </summary>
    public sealed class ConfigClass
    {
        /// <summary>
        /// 取得置文件路径和名称
        /// </summary>
        private static string getConfigFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

        /// <summary>
        /// 读入配置文件
        /// </summary>
        private static System.Xml.XmlDocument LoadConfigDocument
        {
            get
            {
                System.Xml.XmlDocument doc = null;
                try
                {
                    doc = new System.Xml.XmlDocument();
                    doc.Load(getConfigFilePath);
                    return doc;
                }
                catch (System.IO.FileNotFoundException e)
                {
                    throw new System.Exception("No configuration file found.", e);
                }
            }
        }

        /// <summary>
        /// 返回配置节
        /// </summary>
        /// <param name="sectionName">节点名，如【appSettings】</param>
        /// <returns>该配置节点对象</returns>
        public static object GetConfigurationSecion(string sectionName)
        {
            return System.Configuration.ConfigurationManager.GetSection(sectionName);
        }

        /// <summary>
        /// 取得配置文件中的字符串KEY
        /// </summary>
        /// <param name="sectionName">节点名称（为空时，默认为"appSettings"）</param>
        /// <param name="key">KEY名</param>
        /// <returns>返回KEY值</returns>
        public static string GetConfigString(string sectionName, string key)
        {
            sectionName = string.IsNullOrEmpty(sectionName) ? "appSettings" : sectionName;
            System.Collections.Specialized.NameValueCollection cfgName = (System.Collections.Specialized.NameValueCollection)System.Configuration.ConfigurationManager.GetSection(sectionName);
            return cfgName[key];
        }

        /// <summary>
        /// 取得appSettings中的配置节
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>value</returns>
        public static string GetConfigString(string key)
        {
            return GetConfigString(null, key);
        }

        /// <summary>
        /// 得到配置文件中的配置decimal信息
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="key">KEY名称</param>
        /// <returns>返回浮点数</returns>
        public static decimal GetConfigDecimal(string sectionName, string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(GetConfigString(sectionName, key));
        }

        /// <summary>
        /// 取得配置文件中 默认节点的 浮点数型
        /// </summary>
        /// <param name="key">key名</param>
        /// <returns>value值</returns>
        public static decimal GetConfigDecimal(string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDecimal(GetConfigString(key));
        }

        /// <summary>
        /// 得到配置文件中的配置int信息
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="key">KEY名</param>
        /// <returns>返回整数</returns>
        public static int GetConfigInt(string sectionName, string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(GetConfigString(sectionName, key));
        }

        /// <summary>
        /// 得到配置文件中的默认节点配置int信息
        /// </summary>
        /// <param name="key">KEY名</param>
        /// <returns>返回整数</returns>
        public static int GetConfigInt(string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToInt(GetConfigString(key));
        }

        /// <summary>
        /// 写入,更新配置文件节点
        /// </summary>
        /// <param name="sectionName">节点名称</param>
        /// <param name="key">键名</param>
        /// <param name="keyvalue">键值</param>
        public static void SetConfigKeyValue(string sectionName, string key, string keyvalue)
        {
            //导入配置文件
            System.Xml.XmlDocument doc = LoadConfigDocument;
            //重新取得 节点名
            System.Xml.XmlNode node = doc.SelectSingleNode("//" + sectionName);
            if (node == null)
            {
                throw new InvalidOperationException(sectionName + " section not found in config file.");
            }
            try
            {
                // 用 'add'元件 格式化是否包含键名
                // select the 'add' element that contains the key
                System.Xml.XmlElement elem = (System.Xml.XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));
                if (elem != null)
                {
                    //修改或添加键值
                    elem.SetAttribute("value", keyvalue);
                }
                else
                {
                    //如果没有发现键名则进行添加设置键名和键值
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", keyvalue);
                    node.AppendChild(elem);
                }
                doc.Save(getConfigFilePath);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 写入,更新配置文件默认节点
        /// </summary>
        /// <parma name="key">键名</parma>
        /// <parma name="keyvalue">键值</parma>
        public static void SetConfigKeyValue(string key, string keyvalue)
        {
            //导入配置文件
            string SectionName = "appSettings";
            System.Xml.XmlDocument doc = LoadConfigDocument;
            //重新取得 节点名
            System.Xml.XmlNode node = doc.SelectSingleNode("//" + SectionName);
            if (node == null)
            {
                throw new InvalidOperationException(SectionName + " section not found in config file.");
            }
            try
            {
                // 用 'add'元件 格式化是否包含键名
                // select the 'add' element that contains the key
                System.Xml.XmlElement elem = (System.Xml.XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

                if (elem != null)
                {
                    //修改或添加键值
                    elem.SetAttribute("value", keyvalue);
                }
                else
                {
                    //如果没有发现键名则进行添加设置键名和键值
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", keyvalue);
                    node.AppendChild(elem);
                }
                doc.Save(getConfigFilePath);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 删除配置文件节点
        /// </summary>
        /// <param name="SectionName">节名称</param>
        /// <param name="key">要删除的键</param>
        public static void RemoveSectionKey(string SectionName, string key)
        {
            //导入配置文件
            System.Xml.XmlDocument doc = LoadConfigDocument;
            //重新取得 节点名
            System.Xml.XmlNode node = doc.SelectSingleNode("//" + SectionName);
            try
            {
                if (node == null)
                {
                    throw new InvalidOperationException(SectionName + " section not found in config file.");
                }
                else
                {
                    // 用 'add' 方法格式 key和value
                    node.RemoveChild(node.SelectSingleNode(string.Format("//add[@key='{0}']", key)));
                    doc.Save(getConfigFilePath);
                }
            }
            catch (NullReferenceException e)
            {
                throw new System.Exception(string.Format("The key {0} does not exist.", key), e);
            }
        }

        /// <summary>
        /// 删除默认节点
        /// </summary>
        /// <param name="key"></param>
        public static void RemoveSectionKey(string key)
        {
            string SectionName = "appSettings";
            //导入配置文件
            System.Xml.XmlDocument doc = LoadConfigDocument;
            //重新取得 节点名
            System.Xml.XmlNode node = doc.SelectSingleNode("//" + SectionName);
            try
            {
                if (node == null)
                    throw new InvalidOperationException(SectionName + " section not found in config file.");
                else
                {
                    // 用 'add' 方法格式 key和value
                    node.RemoveChild(node.SelectSingleNode(string.Format("//add[@key='{0}']", key)));
                    doc.Save(getConfigFilePath);
                }
            }
            catch (NullReferenceException e)
            {
                throw new System.Exception(string.Format("The key {0} does not exist.", key), e);
            }
        }
    }
}