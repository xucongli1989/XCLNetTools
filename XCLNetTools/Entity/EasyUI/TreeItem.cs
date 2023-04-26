/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace XCLNetTools.Entity.EasyUI
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