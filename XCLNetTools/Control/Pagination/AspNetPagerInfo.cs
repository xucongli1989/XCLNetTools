namespace XCLNetTools.Control.Pagination
{
    /// <summary>
    /// AspNetPager分页
    /// 分页控件来源：http://www.webdiyer.com/aspnetpager/
    /// </summary>
    public class AspNetPagerInfo : BasePagination<Wuqi.Webdiyer.AspNetPager>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pager">分页控件对象</param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页最多显示的记录数</param>
        /// <param name="recordCount">记录总数</param>
        public AspNetPagerInfo(Wuqi.Webdiyer.AspNetPager pager, int pageIndex, int pageSize, int recordCount)
            : base(pager, pageIndex, pageSize, recordCount)
        {
        }

        /// <summary>
        /// 分页控件初始化
        /// </summary>
        public override void InitPager()
        {
            base.InitPager();

            this.Pager.PageSize = this.PagerInfo.PageSize;
            this.Pager.RecordCount = this.PagerInfo.RecordCount;
            this.Pager.CurrentPageIndex = this.PagerInfo.PageIndex;
            this.Pager.CustomInfoHTML = @"第<font style=""color:#f00;font-weight:bolder;"">%CurrentPageIndex%</font>/%PageCount%页 每页最多%PageSize%条，共<font style=""font-weight:bolder;"">%RecordCount%</font>条";
            this.Pager.CustomInfoClass = "pagerInfoClass";
            this.Pager.NumericButtonCount = 3;
            this.Pager.SubmitButtonClass = "pagerSubmitBtnClass";
            this.Pager.ShowCustomInfoSection = Wuqi.Webdiyer.ShowCustomInfoSection.Left;
            this.Pager.FirstPageText = "首页";
            this.Pager.PrevPageText = "上一页";
            this.Pager.NextPageText = "下一页";
            this.Pager.LastPageText = "尾页";
            this.Pager.UrlPaging = true;
            this.Pager.SubmitButtonText = "跳转>>";
            this.Pager.AlwaysShow = true;
            this.Pager.ShowPageIndexBox = Wuqi.Webdiyer.ShowPageIndexBox.Always;
            this.Pager.LayoutType = Wuqi.Webdiyer.LayoutType.Div;
            this.Pager.PagingButtonsClass = "pagerBtn";
            this.Pager.CurrentPageButtonClass = "pagerCurrentBtn";
            this.Pager.PageIndexBoxClass = "pagerInputBox";
        }
    }
}