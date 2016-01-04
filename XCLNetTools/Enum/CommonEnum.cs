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



using System;
using System.ComponentModel;

namespace XCLNetTools.Enum
{
    /// <summary>
    /// 常用枚举常量
    /// </summary>
    [Serializable]
    public class CommonEnum
    {
        /// <summary>
        /// 是否
        /// </summary>
        public enum 是否
        {
            /// <summary>
            /// 是
            /// </summary>
            是 = 1,

            /// <summary>
            /// 否
            /// </summary>
            否 = 0
        }

        /// <summary>
        /// 静态资源类型
        /// </summary>
        public enum StaticResourceTypeEnum
        {
            /// <summary>
            /// js文件
            /// </summary>
            JS,

            /// <summary>
            /// css文件
            /// </summary>
            CSS,

            /// <summary>
            /// icon
            /// </summary>
            ICON
        }


        #region 日期时间相关
        /// <summary>
        /// 周枚举
        /// </summary>
        public enum Weeks
        {
            /// <summary>
            /// 周一
            /// </summary>
            周一 = 1,

            /// <summary>
            /// 周二
            /// </summary>
            周二 = 2,

            /// <summary>
            /// 周三
            /// </summary>
            周三 = 3,

            /// <summary>
            /// 周四
            /// </summary>
            周四 = 4,

            /// <summary>
            /// 周五
            /// </summary>
            周五 = 5,

            /// <summary>
            /// 周六
            /// </summary>
            周六 = 6,

            /// <summary>
            /// 周日
            /// </summary>
            周日 = 0
        }

        /// <summary>
        /// 以前的时间类别
        /// </summary>
        public enum BeforeDateTypeEnum
        {
            /// <summary>
            /// 七天前
            /// </summary>
            [Description("七天前")]
            SevenDay,

            /// <summary>
            /// 一个月前
            /// </summary>
            [Description("一个月前")]
            OneMonth,

            /// <summary>
            /// 三个月前
            /// </summary>
            [Description("三个月前")]
            ThreeMonth,

            /// <summary>
            /// 半年前
            /// </summary>
            [Description("半年前")]
            HalfYear,

            /// <summary>
            /// 一年前
            /// </summary>
            [Description("一年前")]
            OneYear,

            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            All
        }

        /// <summary>
        /// 关于返回值形式的枚举
        /// </summary>
        public enum DiffResultFormat
        {
            /// <summary>
            /// 年数和月数
            /// </summary>
            yymm,

            /// <summary>
            /// 年数
            /// </summary>
            yy,

            /// <summary>
            /// 月数
            /// </summary>
            mm,

            /// <summary>
            /// 天数
            /// </summary>
            dd
        }
        #endregion
    }
}