/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System.Net;

namespace XCLNetTools.Entity.Http
{
    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResult
    {
        private string _Cookie;

        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }

        private CookieCollection _CookieCollection;

        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return _CookieCollection; }
            set { _CookieCollection = value; }
        }

        private string _Html;

        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get { return _Html; }
            set { _Html = value; }
        }

        private byte[] _ResultByte;

        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {
            get { return _ResultByte; }
            set { _ResultByte = value; }
        }

        private WebHeaderCollection _Header;

        /// <summary>
        /// header对象
        /// </summary>
        public WebHeaderCollection Header
        {
            get { return _Header; }
            set { _Header = value; }
        }

        private string _StatusDescription;

        /// <summary>
        /// 返回状态说明
        /// </summary>
        public string StatusDescription
        {
            get { return _StatusDescription; }
            set { _StatusDescription = value; }
        }

        private HttpStatusCode _StatusCode;

        /// <summary>
        /// 返回状态码,默认为OK
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _StatusCode; }
            set { _StatusCode = value; }
        }
    }
}