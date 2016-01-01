using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 静态文件配置
    /// </summary>
    [Serializable]
    public class StaticResourceConfig
    {
        /// <summary>
        /// 静态文件列表
        /// </summary>
        public List<StaticResource> StaticResourceList { get; set; }

        /// <summary>
        /// 深度克隆
        /// </summary>
        public StaticResourceConfig DeepClone()
        {
            return XCLNetTools.Serialize.Lib.DeepClone<StaticResourceConfig>(this);
        }
    }

    /// <summary>
    /// 静态资源model
    /// </summary>
    [Serializable]
    public class StaticResource
    {
        /// <summary>
        /// 静态文件类型
        /// </summary>
        [XmlAttribute]
        public XCLNetTools.Enum.CommonEnum.StaticResourceTypeEnum Type { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        [XmlAttribute]
        public string Src { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        [XmlAttribute]
        public string Version { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        [XmlAttribute]
        public string Attr { get; set; }

        /// <summary>
        /// 转字符串
        /// </summary>
        public override string ToString()
        {
            string fmt = string.Empty;
            switch (this.Type)
            {
                case Enum.CommonEnum.StaticResourceTypeEnum.JS:
                    fmt = @"<script src=""{0}"" type=""text/javascript""  {1}></script>";
                    break;

                case Enum.CommonEnum.StaticResourceTypeEnum.CSS:
                    fmt = @"<link href=""{0}"" rel=""stylesheet"" type=""text/css""  {1}/>";
                    break;

                case Enum.CommonEnum.StaticResourceTypeEnum.ICON:
                    fmt = @"<link rel=""icon"" href=""{0}"" {1} /> ";
                    break;
            }
            fmt += Environment.NewLine;
            string ver = string.Format("{0}v={1}", this.Src.Trim().TrimEnd('?').Contains("?") ? "&" : "?", this.Version);
            return string.Format(fmt, this.Src + ver, this.Attr);
        }
    }
}