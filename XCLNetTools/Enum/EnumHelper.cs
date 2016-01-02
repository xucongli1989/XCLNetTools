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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace XCLNetTools.Enum
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public class EnumHelper
    {
        /// <summary>
        /// 将枚举转为List(包含自定义属性description)
        /// </summary>
        public static List<XCLNetTools.Enum.EnumFieldModel> GetEnumFieldModelList(Type emType)
        {
            if (!emType.IsEnum)
            {
                throw new InvalidOperationException();
            }
            List<XCLNetTools.Enum.EnumFieldModel> list = new List<XCLNetTools.Enum.EnumFieldModel>();
            System.Reflection.FieldInfo[] fields = emType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    XCLNetTools.Enum.EnumFieldModel model = new XCLNetTools.Enum.EnumFieldModel();
                    model.Value = ((int)emType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    model.Text = field.Name;
                    Object[] customObjs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (null != customObjs && customObjs.Length > 0)
                    {
                        model.Description = ((DescriptionAttribute)customObjs[0]).Description;
                    }
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取枚举的description注解
        /// </summary>
        public static string GetEnumDesc<T>(T enumtype)
        {
            string str = string.Empty;
            if (enumtype == null) throw new ArgumentNullException("Enumtype");
            if (!enumtype.GetType().IsEnum) throw new Exception("参数类型不正确");

            FieldInfo[] fieldinfo = enumtype.GetType().GetFields();
            foreach (FieldInfo item in fieldinfo)
            {
                if (string.Equals(Convert.ToString(item.GetValue(enumtype)), enumtype.ToString()))
                {
                    Object[] obj = item.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (obj != null && obj.Length != 0)
                    {
                        DescriptionAttribute des = (DescriptionAttribute)obj[0];
                        str = des.Description;
                    }
                    break;
                }
            }
            return str;
        }

        /// <summary>
        /// 根据枚举text,获取枚举description
        /// </summary>
        public static string GetEnumDescriptionByText(Type T, string text)
        {
            string str = string.Empty;
            var lst = EnumHelper.GetEnumFieldModelList(T);
            if (null != lst && lst.Count > 0)
            {
                str = lst.Where(k => string.Equals(k.Text, text, StringComparison.OrdinalIgnoreCase)).First().Description;
            }
            return str;
        }

        /// <summary>
        /// 将枚举转为list的形式
        /// </summary>
        /// <param name="type">枚举的typeof</param>
        /// <returns>枚举的list形式</returns>
        public static List<XCLNetTools.Entity.TextValue> GetList(Type type)
        {
            if (!type.IsEnum)
            {    //不是枚举的要报错
                throw new InvalidOperationException();
            }
            //建立列表
            List<XCLNetTools.Entity.TextValue> list = new List<XCLNetTools.Entity.TextValue>();
            //获得枚举的字段信息（因为枚举的值实际上是一个static的字段的值）
            System.Reflection.FieldInfo[] fields = type.GetFields();
            //检索所有字段
            foreach (FieldInfo field in fields)
            {
                //过滤掉一个不是枚举值的，记录的是枚举的源类型
                if (field.FieldType.IsEnum)
                {
                    XCLNetTools.Entity.TextValue obj = new XCLNetTools.Entity.TextValue();
                    // 通过字段的名字得到枚举的值
                    // 枚举的值如果是long的话，ToChar会有问题，但这个不在本文的讨论范围之内
                    obj.Value = ((int)type.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    obj.Text = field.Name;
                    list.Add(obj);
                }
            }
            return list;
        }

        /// <summary>
        /// 判断数字是否属于该枚举
        /// <param name="v">枚举的value，就是数字</param>
        /// <param name="type">枚举的typeof</param>
        /// <returns>true:v属于该枚举，反之则不属于</returns>
        /// </summary>
        public static bool IsExistEnumValue(int v, Type type)
        {
            bool flag = false;
            List<XCLNetTools.Entity.TextValue> lst = GetList(type);
            foreach (var m in lst)
            {
                if (m.Value == v.ToString())
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        /// <summary>
        /// 根据名获取值（若未找到，则返回-1）
        /// </summary>
        /// <param name="lst">枚举的list形式</param>
        /// <param name="text">枚举项的名称</param>
        /// <returns>该枚举的值</returns>
        public static int GetValueByText(List<XCLNetTools.Entity.TextValue> lst, string text)
        {
            int i = -1;
            if (null != lst && lst.Count > 0)
            {
                var m = lst.Find(k => k.Text == text);
                if (null != m)
                {
                    i = XCLNetTools.Common.DataTypeConvert.ToInt(m.Value, -1);
                }
            }
            return i;
        }

        /// <summary>
        /// 将多个枚举项进行（按位或）操作，返回int型，若失败，则返回null
        /// </summary>
        public static int? GetBitORValue<T>(List<T> em)
        {
            int? val = null;
            if (!typeof(T).IsEnum)
            {
                return val;
            }
            if (null != em && em.Count > 0)
            {
                val = Convert.ToInt32(em[0]);
                for (int i = 1; i < em.Count; i++)
                {
                    val = (val | Convert.ToInt32(em[i]));
                }
            }
            return val;
        }

        /// <summary>
        /// 根据多个枚举项（按位或）之后的int值，返回枚举list
        /// </summary>
        public static List<T> GetEnumListByBitValue<T>(int val)
        {
            var tp = typeof(T);
            if (!tp.IsEnum || val < 0) return null;
            List<T> lst = new List<T>();
            var values = System.Enum.GetValues(typeof(T));
            if (null != values && values.Length > 0)
            {
                T temp = default(T);
                foreach (var m in values)
                {
                    temp = (T)System.Enum.Parse(tp, Convert.ToString(m));
                    if ((val & Convert.ToInt32(temp)) == Convert.ToInt32(temp))
                    {
                        lst.Add(temp);
                        continue;
                    }
                }
            }
            return lst;
        }
    }
}