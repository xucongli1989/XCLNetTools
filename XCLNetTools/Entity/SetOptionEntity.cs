/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using static XCLNetTools.Enum.CommonEnum;

namespace XCLNetTools.Entity
{
    /// <summary>
    /// 生成select的option时的选项
    /// </summary>
    [Serializable]
    public class SetOptionEntity
    {
        private bool _isNeedPleaseSelect = true;
        private SelectOptionFieldEnum _textFieldEnum = SelectOptionFieldEnum.None;
        private SelectOptionFieldEnum _valueFieldEnum = SelectOptionFieldEnum.None;

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

        /// <summary>
        /// Text字段类型，默认为：None
        /// </summary>
        public SelectOptionFieldEnum TextFieldEnum
        {
            get { return this._textFieldEnum; }
            set { this._textFieldEnum = value; }
        }

        /// <summary>
        /// Value字段类型，默认为：None
        /// </summary>
        public SelectOptionFieldEnum ValueFieldEnum
        {
            get { return this._valueFieldEnum; }
            set { this._valueFieldEnum = value; }
        }
    }
}