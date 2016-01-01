namespace XCLNetTools.Encode
{
    /// <summary>
    /// 其它相关
    /// </summary>
    public class Lib
    {
        /// <summary>
        /// 对js的escape进行解码
        /// </summary>
        /// <param name="str">解码字符串</param>
        public static string Unescape(string str)
        {
            return Microsoft.JScript.GlobalObject.unescape(str);
        }
    }
}