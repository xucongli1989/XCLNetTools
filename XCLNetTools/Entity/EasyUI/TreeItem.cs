/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.1
更新时间：2016-01-01

四：更新内容：
1：将原先基础数据转换方法转移到Common/DataTypeConvert下面
2：其它逻辑优化，如表单参数获取等
3：首次开放所有源代码
 */



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