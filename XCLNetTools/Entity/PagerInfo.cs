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
    /// 分页信息model
    /// </summary>
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
        public int PageCount { get; private set; }

        /// <summary>
        /// 当前页的实际记录条数
        /// </summary>
        public int CurrentPageRecordCount { get; private set; }

        /// <summary>
        /// 当前页的第一条记录的序号
        /// </summary>
        public int StartIndex { get; private set; }

        /// <summary>
        /// 当前页的最后一条记录的序号
        /// </summary>
        public int EndIndex { get; private set; }

        /// <summary>
        /// controller
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// action
        /// </summary>
        public string ActionName { get; set; }
    }
}