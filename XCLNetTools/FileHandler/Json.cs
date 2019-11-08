using Newtonsoft.Json.Linq;
using System;
using System.Xml;

namespace XCLNetTools.FileHandler
{
    /// <summary>
    /// json文件操作
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// 将json字符串转换为freemind字符串
        /// </summary>
        public static string ToFreeMindFromPath(string path)
        {
            var json = System.IO.File.ReadAllText(path);
            return ToFreeMind(json);
        }

        /// <summary>
        /// 将json字符串转换为freemind字符串
        /// </summary>
        public static string ToFreeMind(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return string.Empty;
            }
            XmlDocument doc = new XmlDocument();
            JToken data = JToken.Parse(json);

            //最外层的map节点
            var root = doc.CreateElement("map");
            root.SetAttribute("version", "1.0.1");
            var node = CreateFreeMindNode(doc, "JSON");
            root.AppendChild(node);

            //遍历json
            Action<JToken, XmlElement> act = null;
            act = new Action<JToken, XmlElement>((JToken token, XmlElement ele) =>
            {
                //json最终的值
                if (token is JValue)
                {
                    var child = CreateFreeMindNode(doc, token.Value<string>());
                    ele.AppendChild(child);
                    return;
                }

                //节点的value为{...}
                if (token.Type == JTokenType.Object)
                {
                    foreach (JProperty pro in token)
                    {
                        var child = CreateFreeMindNode(doc, pro.Name);
                        act.Invoke(pro.Value, child);
                        ele.AppendChild(child);
                    }
                    return;
                }

                //节点的value为[...]
                if (token.Type == JTokenType.Array)
                {
                    var arr = (JArray)token;
                    for (var i = 0; i < arr.Count; i++)
                    {
                        var child = CreateFreeMindNode(doc, i);
                        act.Invoke(arr[i], child);
                        ele.AppendChild(child);
                    }
                }
            });
            act.Invoke(data, node);
            doc.AppendChild(root);
            return doc.OuterXml;
        }

        /// <summary>
        /// 创建节点
        /// </summary>
        private static XmlElement CreateFreeMindNode(XmlDocument doc, object text)
        {
            var node = doc.CreateElement("node");
            node.SetAttribute("CREATED", "0");
            node.SetAttribute("MODIFIED", "0");
            node.SetAttribute("ID", Guid.NewGuid().ToString("N"));
            node.SetAttribute("TEXT", Convert.ToString(text));
            return node;
        }
    }
}