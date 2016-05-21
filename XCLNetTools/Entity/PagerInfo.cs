/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Runtime.Serialization;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 分页信息model
    /// </summary>
    [Serializable]
    [DataContract]
    public class PagerInfo
    {
        private int _pageIndex = 0;
        private int _pageSize = 0;
        private int _recordCount = 0;

        private PagerInfo()
        {
        }

        /// <summary>
        /// 分页信息 构造函数
        /// </summary>
        public PagerInfo(int pageIndex, int pageSize, int recordCount)
        {
            this._pageIndex = pageIndex <= 0 ? 1 : pageIndex;
            this._pageSize = pageSize <= 0 ? 10 : pageSize;
            this._recordCount = recordCount <= 0 ? 0 : recordCount;
            this.Refresh();
        }

        /// <summary>
        /// 刷新其它参数的值
        /// </summary>
        private void Refresh()
        {
            if (this.RecordCount > 0)
            {
                this.PageCount = (int)Math.Ceiling((1.0 * this.RecordCount) / this.PageSize);
                this._pageIndex = this.PageIndex > this.PageCount ? this.PageCount : this.PageIndex;
                this.CurrentPageRecordCount = this.PageIndex == this.PageCount && this.RecordCount % this.PageSize > 0 ? this.RecordCount % this.PageSize : this.PageSize;
                this.StartIndex = (this.PageIndex - 1) * this.PageSize + 1;
                this.EndIndex = this.StartIndex + this.CurrentPageRecordCount - 1;
            }
            else
            {
                this.PageCount = 0;
                this.CurrentPageRecordCount = 0;
                this.StartIndex = 0;
                this.EndIndex = 0;
            }
        }

        /// <summary>
        /// 当前页码（第一页为1），默认为1
        /// 注：若重新设置此值，则将一同更新其它相关联的属性值
        /// </summary>
        [DataMember]
        public int PageIndex
        {
            get { return this._pageIndex; }
            set
            {
                this._pageIndex = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 每页最多显示的记录数，默认为10
        /// 注：若重新设置此值，则将一同更新其它相关联的属性值
        /// </summary>
        [DataMember]
        public int PageSize
        {
            get { return this._pageSize; }
            set
            {
                this._pageSize = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 当前记录总数
        /// 注：若重新设置此值，则将一同更新其它相关联的属性值
        /// </summary>
        [DataMember]
        public int RecordCount
        {
            get { return this._recordCount; }
            set
            {
                this._recordCount = value;
                this.Refresh();
            }
        }

        /// <summary>
        /// 当前总页数
        /// </summary>
        [DataMember]
        public int PageCount { get; private set; }

        /// <summary>
        /// 当前页的实际记录条数
        /// </summary>
        [DataMember]
        public int CurrentPageRecordCount { get; private set; }

        /// <summary>
        /// 当前页的第一条记录的序号
        /// </summary>
        [DataMember]
        public int StartIndex { get; private set; }

        /// <summary>
        /// 当前页的最后一条记录的序号
        /// </summary>
        [DataMember]
        public int EndIndex { get; private set; }

        /// <summary>
        /// controller
        /// </summary>
        [DataMember]
        public string ControllerName { get; set; }

        /// <summary>
        /// action
        /// </summary>
        [DataMember]
        public string ActionName { get; set; }

        /// <summary>
        /// 转为PagerInfoSimple对象
        /// </summary>
        /// <returns>PagerInfoSimple对象</returns>
        public PagerInfoSimple ToPagerInfoSimple()
        {
            return new PagerInfoSimple(this.PageIndex, this.PageSize, this.RecordCount);
        }
    }

    /// <summary>
    /// 分页信息简易model
    /// </summary>
    [Serializable]
    [DataContract]
    public class PagerInfoSimple
    {
        /// <summary>
        /// 默认构造
        /// </summary>
        public PagerInfoSimple()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="pageIndex">当前为第几页</param>
        /// <param name="pageSize">每页最多显示多少条记录</param>
        /// <param name="recordCount">记录总数</param>
        public PagerInfoSimple(int pageIndex, int pageSize, int recordCount)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.RecordCount = recordCount;
        }

        /// <summary>
        /// 当前为第几页
        /// </summary>
        [DataMember]
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页最多显示多少条记录
        /// </summary>
        [DataMember]
        public int PageSize { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        [DataMember]
        public int RecordCount { get; set; }

        /// <summary>
        /// 转为PagerInfo对象
        /// </summary>
        /// <returns>PagerInfo对象</returns>
        public PagerInfo ToPagerInfo()
        {
            return new PagerInfo(this.PageIndex, this.PageSize, this.RecordCount);
        }
    }
}