/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

二：贡献者：
1：xucongli1989（https://github.com/xucongli1989）电子邮件：80213876@qq.com

三：更新：
当前版本：v2.2
更新时间：2016-02

四：更新内容：
1：更新表单获取的参数类型
2：更改Message/JsonMsg类的目录
3：删除多余的方法
4：修复一处未dispose问题
5：整理部分代码
6：添加 MethodResult.cs
7：获取枚举list时可以使用byte/short等
 */


using System;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 生成select的option时的选项
    /// </summary>
    [Serializable]
    public class SetOptionEntity
    {
        private bool _isNeedPleaseSelect = true;

        /// <summary>
        /// 是否需要生成"请选择"的option
        /// </summary>
        public bool IsNeedPleaseSelect
        {
            get
            {
                return this._isNeedPleaseSelect;
            }
            set
            {
                this._isNeedPleaseSelect = value;
            }
        }

        /// <summary>
        /// 默认选中的项
        /// </summary>
        public string DefaultValue { get; set; }
    }
}