/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

namespace XCLNetTools.Control.Pagination
{
    /// <summary>
    /// 分页抽象类
    /// </summary>
    public abstract class BasePagination<T>
    {
        private BasePagination()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pager">分页控件对象</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页最多显示的记录数</param>
        /// <param name="recordCount">记录总数</param>
        public BasePagination(T pager, int pageIndex, int pageSize, int recordCount)
        {
            this.Pager = pager;
            this.PagerInfo = new Entity.PagerInfo(pageIndex, pageSize, recordCount);
            this.InitPager();
        }

        /// <summary>
        /// 当前分页控件
        /// </summary>
        public T Pager { get; set; }

        /// <summary>
        /// 分页信息
        /// </summary>
        public XCLNetTools.Entity.PagerInfo PagerInfo { get; set; }

        /// <summary>
        /// 分页初始化
        /// </summary>
        public virtual void InitPager()
        {
        }
    }
}