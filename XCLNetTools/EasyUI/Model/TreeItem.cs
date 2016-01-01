using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/**********************************************/

//引用 Newtonsoft.Json 后生成报错问题
namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}

/**********************************************/

namespace XCLNetTools.EasyUI.Model
{
    /// <summary>
    /// tree的每项(注意大小写，此js插件中是小写)
    /// </summary>
    [Serializable]
    public class TreeItem
    {
        /// <summary>
        /// 标识
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 显示文本
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 状态（open/closed）
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// 是否选中该节点
        /// </summary>
        [JsonProperty("checked")]
        public bool Checked { get; set; }

        /// <summary>
        /// 自定义属性
        /// </summary>
        [JsonProperty("attributes")]
        public string Attributes { get; set; }

        /// <summary>
        /// 子项数组
        /// </summary>
        [JsonProperty("children")]
        public List<TreeItem> Children { get; set; }
    }
}