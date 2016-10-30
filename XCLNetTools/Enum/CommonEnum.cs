/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

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
        #region 逻辑

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

        #endregion 逻辑

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

        #endregion 日期时间相关

        #region 图片操作相关

        /// <summary>
        /// 图片指定宽高生成缩略图的模式 枚举
        /// </summary>
        public enum ThumbImageModeEnum
        {
            /// <summary>
            /// 指定宽高缩放（可能变形）
            /// </summary>
            WH,

            /// <summary>
            /// 指定宽，高按比例
            /// </summary>
            W,

            /// <summary>
            /// 指定高，宽按比例
            /// </summary>
            H,

            /// <summary>
            /// 指定宽高裁减（不变形）
            /// </summary>
            EqualRatioWH
        }

        #endregion 图片操作相关

        #region 其它

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

        /// <summary>
        /// XCLNetTools.Message.MessageModel 跳转方式
        /// </summary>
        public enum RedirectTargetEnum
        {
            /// <summary>
            /// 在当前页打开
            /// </summary>
            None = 0,

            /// <summary>
            /// 新窗口打开
            /// </summary>
            NewBlank = 1
        }

        /// <summary>
        /// select options 字段类型枚举
        /// </summary>
        public enum SelectOptionFieldEnum
        {
            /// <summary>
            /// 未指定
            /// </summary>
            None,

            /// <summary>
            /// 文本
            /// </summary>
            Text,

            /// <summary>
            /// value值
            /// </summary>
            Value,

            /// <summary>
            /// 描述
            /// </summary>
            Description
        }

        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        public enum DatabaseTypeEnum
        {
            /// <summary>
            /// sql server
            /// </summary>
            MSSQL
        }

        /// <summary>
        /// 登录退出类型枚举
        /// </summary>
        public enum LoginTypeEnum
        {
            /// <summary>
            /// 登录
            /// </summary>
            ON,

            /// <summary>
            /// 退出
            /// </summary>
            OFF
        }

        /// <summary>
        /// 系统所属环境枚举
        /// </summary>
        public enum SysEnvironmentEnum
        {
            /// <summary>
            /// 开发环境
            /// </summary>
            DEV,

            /// <summary>
            /// 测试环境
            /// </summary>
            FAT,

            /// <summary>
            /// UAT环境
            /// </summary>
            UAT,

            /// <summary>
            /// 生产环境
            /// </summary>
            PRD
        }

        /// <summary>
        /// 页面操作类型枚举
        /// </summary>
        public enum HandleTypeEnum
        {
            /// <summary>
            /// 添加
            /// </summary>
            ADD,

            /// <summary>
            /// 删除
            /// </summary>
            DEL,

            /// <summary>
            /// 更新
            /// </summary>
            UPDATE,

            /// <summary>
            /// 导入
            /// </summary>
            INPUT,

            /// <summary>
            /// 导出
            /// </summary>
            OUTPUT,

            /// <summary>
            /// 其它
            /// </summary>
            OTHER
        }

        #endregion 其它
    }
}