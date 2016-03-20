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