/*
一：基本信息：
开源协议：https://github.com/xucongli1989/XCLNetTools/blob/master/LICENSE
项目地址：https://github.com/xucongli1989/XCLNetTools
Create By: XCL @ 2012

 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XCLNetTools.Enum
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// 将枚举转为List(包含自定义属性description)（value为int型的string）
        /// 已按枚举的value升序排列
        /// </summary>
        /// <param name="emType">枚举type</param>
        /// <returns>枚举的List</returns>
        public static List<XCLNetTools.Entity.Enum.EnumFieldModel> GetEnumFieldModelList(Type emType)
        {
            var lst = GetEnumFieldTModelList<int>(emType);
            if (null == lst || lst.Count == 0)
            {
                return null;
            }
            List<XCLNetTools.Entity.Enum.EnumFieldModel> result = new List<Entity.Enum.EnumFieldModel>();
            lst.ForEach(k =>
            {
                result.Add(new Entity.Enum.EnumFieldModel()
                {
                    Description = k.Description,
                    Text = k.Text,
                    Value = k.Value.ToString()
                });
            });
            return result;
        }

        /// <summary>
        /// 将枚举转为List(包含自定义属性description)
        /// 已按枚举的value升序排列
        /// </summary>
        /// <param name="emType">枚举type</param>
        /// <typeparam name="T">枚举value的类型（（可为byte、sbyte、short、ushort、int、uint、long 或 ulong。））</typeparam>
        /// <returns>枚举的List</returns>
        public static List<XCLNetTools.Entity.Enum.EnumFieldTModel<T>> GetEnumFieldTModelList<T>(Type emType)
        {
            if (!emType.IsEnum)
            {
                throw new ArgumentException("emType必须为枚举类型！");
            }
            object objVal = null;
            var list = new List<XCLNetTools.Entity.Enum.EnumFieldTModel<T>>();
            System.Reflection.FieldInfo[] fields = emType.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    var model = new XCLNetTools.Entity.Enum.EnumFieldTModel<T>();
                    objVal = emType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                    model.Value = (T)Convert.ChangeType(objVal, typeof(T));
                    model.Text = field.Name;
                    Object[] customObjs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (null != customObjs && customObjs.Length > 0)
                    {
                        model.Description = ((DescriptionAttribute)customObjs[0]).Description;
                    }
                    list.Add(model);
                }
            }

            if (list.Count > 0)
            {
                list = list.OrderBy(k => k.Value).ToList();
            }

            return list;
        }

        /// <summary>
        /// 获取枚举的description注解
        /// </summary>
        /// <returns>枚举的描述</returns>
        public static string GetEnumDesc<T>(T enumtype)
        {
            string str = string.Empty;
            if (enumtype == null)
            {
                throw new ArgumentNullException("enumtype");
            }
            if (!enumtype.GetType().IsEnum)
            {
                throw new ArgumentException("参数类型应该为枚举类型", "enumtype");
            }
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
        /// <returns>枚举的描述</returns>
        public static string GetEnumDescriptionByText(Type T, string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }
            string str = string.Empty;
            var lst = EnumHelper.GetEnumFieldModelList(T);
            if (null != lst && lst.Count > 0)
            {
                var m = lst.FirstOrDefault(k => string.Equals(k.Text, text, StringComparison.OrdinalIgnoreCase));
                if (null != m)
                {
                    str = m.Description;
                }
            }
            return str;
        }

        /// <summary>
        /// 根据枚举description,获取枚举text
        /// </summary>
        public static string GetEnumTextByDescription(Type T, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return string.Empty;
            }
            var str = string.Empty;
            var lst = EnumHelper.GetEnumFieldModelList(T);
            if (null != lst && lst.Count > 0)
            {
                var m = lst.Find(k => string.Equals(k.Description, description, StringComparison.OrdinalIgnoreCase));
                if (null != m)
                {
                    str = m.Text;
                }
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
            {
                throw new InvalidOperationException();
            }
            List<XCLNetTools.Entity.TextValue> list = new List<XCLNetTools.Entity.TextValue>();
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    XCLNetTools.Entity.TextValue obj = new XCLNetTools.Entity.TextValue();
                    obj.Value = ((int)type.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    obj.Text = field.Name;
                    list.Add(obj);
                }
            }
            return list;
        }

        /// <summary>
        /// 判断数字是否属于该枚举
        /// </summary>
        /// <param name="v">枚举的value，就是数字</param>
        /// <param name="type">枚举的typeof</param>
        /// <returns>true:v属于该枚举，反之则不属于</returns>
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
        /// <returns>结果值</returns>
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
        /// <returns>枚举list</returns>
        public static List<T> GetEnumListByBitValue<T>(int val)
        {
            var tp = typeof(T);
            if (!tp.IsEnum || val < 0) return null;
            List<T> lst = new List<T>();
            var values = System.Enum.GetValues(typeof(T));
            if (null != values && values.Length > 0)
            {
                T temp;
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

        /// <summary>
        /// 将指定枚举的值求和
        /// </summary>
        /// <returns>求和后的结果值</returns>
        public static long GetEnumSumValue(Type em)
        {
            var lst = EnumHelper.GetList(em);
            if (null == lst || lst.Count == 0)
            {
                throw new ArgumentException("该枚举中没有任何项！");
            }
            return lst.Sum(k => Convert.ToInt64(k.Value));
        }

        /// <summary>
        /// 获取枚举的最小值
        /// </summary>
        /// <param name="em">指定枚举</param>
        /// <returns>枚举项中的最小值</returns>
        public static long GetMinValue(Type em)
        {
            var lst = EnumHelper.GetList(em);
            if (null == lst || lst.Count == 0)
            {
                throw new ArgumentException("该枚举中没有任何项！");
            }
            return lst.Min(k => Convert.ToInt64(k.Value));
        }

        /// <summary>
        /// 获取枚举的最大值
        /// </summary>
        /// <param name="em">指定枚举</param>
        /// <returns>枚举项中的最大值</returns>
        public static long GetMaxValue(Type em)
        {
            var lst = EnumHelper.GetList(em);
            if (null == lst || lst.Count == 0)
            {
                throw new ArgumentException("该枚举中没有任何项！");
            }
            return lst.Max(k => Convert.ToInt64(k.Value));
        }

        /// <summary>
        /// 判断指定值是否超出指定枚举的值范围
        /// </summary>
        /// <param name="em">枚举</param>
        /// <param name="val">要判断的值</param>
        /// <returns>true：在范围内；false：已超出范围</returns>
        public static bool IsInRange(Type em, long val)
        {
            var min = EnumHelper.GetMinValue(em);
            var max = EnumHelper.GetMaxValue(em);
            return val >= min && val <= max;
        }

        /// <summary>
        /// 将指定class中的所有枚举转为json字符串
        /// 示例：public class Test{public enum EE{a,b,c}}  ====》  {"EE":{"a":"","b":"","c":""}}
        /// </summary>
        public static string GetEnumJson(Type t)
        {
            StringBuilder str = new StringBuilder();
            var ms = t.GetNestedTypes();
            if (null == ms || !ms.Any())
            {
                return string.Empty;
            }
            var enumlist = ms.Where(k => k.IsEnum).ToList();
            if (null == enumlist || enumlist.Count == 0)
            {
                return string.Empty;
            }
            str.Append("{");
            for (int i = 0; i < enumlist.Count; i++)
            {
                var m = enumlist[i];
                str.AppendFormat(@"""{0}"":{{", m.Name);
                var fields = m.GetFields().Where(k => k.FieldType.IsEnum).ToList();
                for (int j = 0; j < fields.Count; j++)
                {
                    string val = fields[j].Name;
                    string des = "";

                    Object[] customObjs = fields[j].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (null != customObjs && customObjs.Length > 0)
                    {
                        des = ((DescriptionAttribute)customObjs[0]).Description;
                    }

                    str.AppendFormat(@"""{0}"":""{1}""", val, des);
                    if (j != fields.Count - 1)
                    {
                        str.Append(",");
                    }
                }
                str.Append("}");
                if (i != enumlist.Count - 1)
                {
                    str.Append(",");
                }
            }
            str.Append("}");

            return str.ToString();
        }

        /// <summary>
        /// 将字符串转换为枚举类型
        /// </summary>
        public static T? ConvertStringToEnum<T>(string value) where T : struct, System.Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            if (System.Enum.TryParse<T>(value, true, out T result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// 将字符串列表转换为枚举类型列表
        /// </summary>
        public static List<T> ConvertStringToEnum<T>(List<string> lst) where T : struct, System.Enum
        {
            var result = new List<T>();
            lst?.ForEach(k =>
            {
                var em = EnumHelper.ConvertStringToEnum<T>(k);
                if (null != em)
                {
                    result.Add(em.Value);
                }
            });
            return result;
        }
    }
}