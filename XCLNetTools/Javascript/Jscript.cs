/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Text;
using System.Web;

namespace XCLNetTools.Javascript
{
    ///<summary>
    /// 一些常用的Js调用
    /// </summary>
    public class Jscript
    {
        #region 输出JS代码

        /// <summary>
        /// 输出JS代码
        /// </summary>
        /// <param name="code">js代码</param>
        public static void WriteJS(string code)
        {
            string js = @"<script type='text/javascript'>" + code + "</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 输出JS代码

        #region 弹出Javascript小窗口

        /// <summary>
        /// 弹出Javascript小窗口
        /// </summary>
        /// <param name="message">alter中的消息</param>
        public static void Alert(string message)
        {
            string js = @"<script type='text/javascript'>alert('" + message.Replace("'", "\"") + "');</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 弹出Javascript小窗口

        #region 弹出消息框并且转向到新的URL

        /// <summary>
        /// 弹出消息框并且转向到新的URL
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="toURL">要跳转的地址</param>
        public static void AlertAndRedirect(string message, string toURL)
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, message, toURL));
        }

        #endregion 弹出消息框并且转向到新的URL

        #region 回到历史页面

        /// <summary>
        /// 回到历史页面
        /// </summary>
        /// <param name="value">回到历史第几个页面</param>
        public static void GoHistory(int value)
        {
            string js = @"<script type='text/javascript'>history.go({0});</script>";
            HttpContext.Current.Response.Write(string.Format(js, value));
        }

        #endregion 回到历史页面

        #region 关闭当前窗口

        /// <summary>
        /// 关闭当前窗口
        /// </summary>
        public static void CloseWindow()
        {
            string js = @"<script type='text/javascript'>parent.opener=null;window.close();</script>";
            HttpContext.Current.Response.Write(js);
            HttpContext.Current.Response.End();
        }

        #endregion 关闭当前窗口

        #region 刷新父窗口

        /// <summary>
        /// 刷新父窗口
        /// </summary>
        /// <param name="url">指定的url</param>
        public static void RefreshParent(string url)
        {
            string js = @"<script type='text/javascript'>window.opener.location.href='" + url + "';window.close();</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 刷新父窗口

        #region 刷新打开窗口

        /// <summary>
        /// 刷新打开窗口
        /// </summary>
        public static void RefreshOpener()
        {
            string js = @"<script type='text/javascript'>opener.location.reload();</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 刷新打开窗口

        #region 打开指定大小的新窗体

        /// <summary>
        /// 打开指定大小的新窗体
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="width">宽</param>
        /// <param name="heigth">高</param>
        /// <param name="top">头位置</param>
        /// <param name="left">左位置</param>
        public static void OpenWebFormSize(string url, int width, int heigth, int top, int left)
        {
            string js = @"<script type='text/javascript'>window.open('" + url + @"','','height=" + heigth + ",width=" + width + ",top=" + top + ",left=" + left + ",location=no,menubar=no,resizable=yes,scrollbars=yes,status=yes,titlebar=no,toolbar=no,directories=no');</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 打开指定大小的新窗体

        #region 打开指定大小的新窗体

        /// <summary>
        /// 打开指定url
        /// </summary>
        /// <param name="url">url地址</param>
        public static void OpenWebFormSize(string url)
        {
            string js = @"<script type='text/javascript'>window.open('" + url + @"');</script>";
            HttpContext.Current.Response.Write(js);
        }

        #endregion 打开指定大小的新窗体

        #region 打开指定大小位置的模式对话框

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">url地址</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <param name="top">距离上位置</param>
        /// <param name="left">距离左位置</param>
        public static void ShowModalDialogWindow(string webFormUrl, int width, int height, int top, int left)
        {
            string features = "dialogWidth:" + width.ToString() + "px"
                + ";dialogHeight:" + height.ToString() + "px"
                + ";dialogLeft:" + left.ToString() + "px"
                + ";dialogTop:" + top.ToString() + "px"
                + ";center:yes;help=no;resizable:no;status:no;scroll=yes";
            ShowModalDialogWindow(webFormUrl, features);
        }

        #endregion 打开指定大小位置的模式对话框

        #region 打开指定大小位置的模式对话框

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">url地址</param>
        /// <param name="features">参数</param>
        public static void ShowModalDialogWindow(string webFormUrl, string features)
        {
            string js = ShowModalDialogJavascript(webFormUrl, features);
            HttpContext.Current.Response.Write(js);
        }

        #endregion 打开指定大小位置的模式对话框

        #region 打开指定大小位置的模式对话框

        /// <summary>
        /// 打开指定大小位置的模式对话框
        /// </summary>
        /// <param name="webFormUrl">url地址</param>
        /// <param name="features">用来描述对话框的外观等信息，可以使用以下的一个或几个，用分号“;”隔开
        /// 1.   dialogHeight:   对话框高度，不小于100px
        /// 2.   dialogWidth:   对话框宽度。
        /// 3.   dialogLeft:    离屏幕左的距离。
        /// 4.   dialogTop:    离屏幕上的距离。
        /// 5.   center:         { yes | no | 1 | 0 } ：             是否居中，默认yes，但仍可以指定高度和宽度。
        /// 6.   help:            {yes | no | 1 | 0 }：               是否显示帮助按钮，默认yes。
        /// 7.   resizable:      {yes | no | 1 | 0 } [IE5+]：    是否可被改变大小。默认no。
        /// 8.   status:         {yes | no | 1 | 0 } [IE5+]：     是否显示状态栏。默认为yes[ Modeless]或no[Modal]。
        /// 9.   scroll:           { yes | no | 1 | 0 | on | off }：是否显示滚动条。默认为yes。
        ///    下面几个属性是用在HTA中的，在一般的网页中一般不使用。
        /// 10.  dialogHide:{ yes | no | 1 | 0 | on | off }：在打印或者打印预览时对话框是否隐藏。默认为no。
        /// 11.   edge:{ sunken | raised }：指明对话框的边框样式。默认为raised。
        /// 12.   unadorned:{ yes | no | 1 | 0 | on | off }：默认为no。
        /// </param>
        /// <returns>js代码</returns>
        public static string ShowModalDialogJavascript(string webFormUrl, string features)
        {
            string js = @"<script type='text/javascript'>showModalDialog('" + webFormUrl + "','','" + features + "');</script>";
            return js;
        }

        #endregion 打开指定大小位置的模式对话框

        #region 在body结尾输出js

        /// <summary>
        /// 在body结尾输出js(嵌入到ASP.NET页面的底部,恰好位于关闭元素 /form的前面)
        /// </summary>
        /// <param name="page">page对象</param>
        /// <param name="js">js代码</param>
        public static void AddBodyEnd(System.Web.UI.Page page, String js)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), @"<script type='text/javascript' >" + js + "</script>");
        }

        #endregion 在body结尾输出js

        #region 在body开始处输出js

        /// <summary>
        /// 在body开始处输出js(将 JavaScript 嵌入到页面中开始元素 form 的紧后面)
        /// </summary>
        /// <param name="page">page对象</param>
        /// <param name="js">js代码</param>
        public static void AddBodyStart(System.Web.UI.Page page, String js)
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), Guid.NewGuid().ToString(), @"<script type='text/javascript' >" + js + "</script>");
        }

        #endregion 在body开始处输出js

        #region 控件点击 消息确认提示框

        /// <summary>
        /// 控件点击 消息确认提示框
        /// </summary>
        /// <param name="control">webform服务器控件</param>
        /// <param name="msg">js confirm 提示语</param>
        public static void ShowConfirm(System.Web.UI.WebControls.WebControl control, string msg)
        {
            control.Attributes.Add("onclick", "return confirm('" + msg + "');");
        }

        #endregion 控件点击 消息确认提示框

        #region 显示消息提示对话框，并进行页面跳转

        /// <summary>
        /// 显示消息提示对话框，并进行页面跳转
        /// </summary>
        /// <param name="page">当前页面指针，一般为this</param>
        /// <param name="msg">提示信息</param>
        /// <param name="url">跳转的目标URL</param>
        public static void ShowAndRedirect(System.Web.UI.Page page, string msg, string url)
        {
            StringBuilder Builder = new StringBuilder();
            Builder.Append("<script type='text/javascript' defer>");
            Builder.AppendFormat("alert('{0}');", msg);
            Builder.AppendFormat("top.location.href='{0}'", url);
            Builder.Append("</script>");
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), Builder.ToString());
        }

        #endregion 显示消息提示对话框，并进行页面跳转

        #region 其它

        /// <summary>
        /// 将多行模式的字符串字符串转js常量
        /// </summary>
        public static string StringToConst(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            return str.Trim().Replace(@"\", @"\\").Replace(@"'", @"\'").Replace(@"""", @"\""").Replace("\r", "\\\r");
        }

        #endregion 其它
    }
}