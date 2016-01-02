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



using com.mxgraph;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Xml;

namespace XCLNetTools.Control.MxGraph
{
    /// <summary>
    /// MxGraph操作类
    /// </summary>
    public class Lib
    {
        /// <summary>
        /// 根据view形式的xml生成图片
        /// </summary>
        /// <param name="xml">view形式的xml</param>
        /// <returns>image对象</returns>
        public static Image GetImage(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new Exception("要生成图片的xml信息不能为空！");
            }
            XmlTextReader xmlReader = new XmlTextReader(new StringReader(xml));
            mxGraphViewImageReader viewReader = new mxGraphViewImageReader(xmlReader, Color.White, 4, true, true);
            return mxGraphViewImageReader.Convert(viewReader);
        }

        /// <summary>
        /// 导出mxGraph为图片
        /// </summary>
        /// <param name="xml">mxGraph的model的view形式的xml</param>
        /// <param name="filename">导出后的文件名（包含扩展名）</param>
        public static void ExportImage(string xml, string filename)
        {
            using (Image image = GetImage(xml))
            {
                if (null == image)
                {
                    throw new Exception(string.Format("XML：【{0}】所生成的图片为null，生成失败！", xml));
                }
                HttpContext context = HttpContext.Current;
                context.Response.ContentType = "image/jpeg";
                context.Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}", filename));
                MemoryStream memStream = new MemoryStream();
                image.Save(memStream, ImageFormat.Jpeg);
                memStream.WriteTo(context.Response.OutputStream);
            }
        }
    }
}