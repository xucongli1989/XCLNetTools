/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 文件信息实体
    /// </summary>
    [Serializable]
    public class FileInfoEntity
    {
        /// <summary>
        /// 标识ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public bool IsFolder { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 扩展名（不含小圆点）
        /// </summary>
        public string ExtName { get; set; }

        /// <summary>
        /// 根物理路径
        /// </summary>
        public string RootPath { get; set; }

        /// <summary>
        /// 该文件或文件夹的物理路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 该文件或文件夹的web路径
        /// </summary>
        public string WebPath { get; set; }

        /// <summary>
        /// 该文件或文件夹相对于RootPath的相对路径
        /// </summary>
        public string RelativePath { get; set; }

        /// <summary>
        /// 大小(byte)
        /// </summary>
        public long? Size { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}