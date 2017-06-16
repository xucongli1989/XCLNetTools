using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using XCLNetTools.Entity;

namespace XCLNetTools.Common
{
    /// <summary>
    /// 开关控制
    /// </summary>
    public class SwitchControl
    {
        #region 开关配置和开关的百分比控制

        /// <summary>
        /// 百分比对应的匹配正则
        /// ==============
        ///配置百分比     实际百分比
        ///1=========0.78125
        ///2=========1.953125
        ///3=========2.734375
        ///4=========3.90625
        ///5=========4.6875
        ///6=========5.859375
        ///7=========6.25
        ///8=========7.8125
        ///9=========9.375
        ///10=========9.765625
        ///11=========10.9375
        ///12=========11.71875
        ///13=========12.890625
        ///14=========13.671875
        ///15=========15.234375
        ///16=========15.625
        ///17=========16.40625
        ///18=========17.578125
        ///19=========18.75
        ///20=========19.53125
        ///21=========21.09375
        ///22=========21.875
        ///23=========21.875
        ///24=========23.4375
        ///25=========25
        ///26=========25.78125
        ///27=========27.34375
        ///28=========28.125
        ///29=========29.296875
        ///30=========30.078125
        ///31=========31.25
        ///32=========31.640625
        ///33=========32.8125
        ///34=========34.375
        ///35=========35.15625
        ///36=========35.546875
        ///37=========37.5
        ///38=========37.5
        ///39=========38.671875
        ///40=========39.0625
        ///41=========40.625
        ///42=========42.1875
        ///43=========42.96875
        ///44=========43.75
        ///45=========45.703125
        ///46=========45.703125
        ///47=========46.875
        ///48=========47.265625
        ///49=========49.21875
        ///50=========50
        ///51=========50.78125
        ///52=========51.5625
        ///53=========52.734375
        ///54=========54.6875
        ///55=========54.6875
        ///56=========55.859375
        ///57=========56.25
        ///58=========58.59375
        ///59=========58.59375
        ///60=========60.15625
        ///61=========60.9375
        ///62=========60.9375
        ///63=========62.5
        ///64=========64.453125
        ///65=========64.453125
        ///66=========65.625
        ///67=========66.015625
        ///68=========68.75
        ///69=========68.75
        ///70=========70.3125
        ///71=========71.09375
        ///72=========71.09375
        ///73=========71.09375
        ///74=========75
        ///75=========75
        ///76=========76.171875
        ///77=========76.5625
        ///78=========76.5625
        ///79=========76.5625
        ///80=========81.25
        ///81=========81.25
        ///82=========82.03125
        ///83=========82.03125
        ///84=========82.03125
        ///85=========82.03125
        ///86=========87.5
        ///87=========87.5
        ///88=========87.890625
        ///89=========87.890625
        ///90=========87.890625
        ///91=========87.890625
        ///92=========93.75
        ///93=========93.75
        ///94=========93.75
        ///95=========93.75
        ///96=========93.75
        ///97=========100
        ///98=========100
        ///99=========100
        ///100=========100
        /// </summary>
        private static Dictionary<int, string> percentDic = new Dictionary<int, string>(){
                                            {1,"^[0][01]$"},
                                            {2,"^[0][01234]$"},
                                            {3,"^[0][0123456]$"},
                                            {4,"^[01][01234]$"},
                                            {5,"^[012][0123]$"},
                                            {6,"^[012][01234]$"},
                                            {7,"^[0123][0123]$"},
                                            {8,"^[0123][01234]$"},
                                            {9,"^[0123][012345]$"},
                                            {10,"^[01234][01234]$"},
                                            {11,"^[0123][0123456]$"},
                                            {12,"^[01234][012345]$"},
                                            {13,"^[012][0123456789A]$"},
                                            {14,"^[01234][0123456]$"},
                                            {15,"^[012][0123456789ABC]$"},
                                            {16,"^[01234][01234567]$"},
                                            {17,"^[012345][0123456]$"},
                                            {18,"^[01234][012345678]$"},
                                            {19,"^[012345][01234567]$"},
                                            {20,"^[01234][0123456789]$"},
                                            {21,"^[012345][012345678]$"},
                                            {22,"^[0123456][01234567]$"},
                                            {23,"^[0123456][01234567]$"},
                                            {24,"^[012345][0123456789]$"},
                                            {25,"^[01234567][01234567]$"},
                                            {26,"^[012345][0123456789A]$"},
                                            {27,"^[0123456][0123456789]$"},
                                            {28,"^[01234567][012345678]$"},
                                            {29,"^[01234][0123456789ABCDE]$"},
                                            {30,"^[0123456][0123456789A]$"},
                                            {31,"^[01234567][0123456789]$"},
                                            {32,"^[012345678][012345678]$"},
                                            {33,"^[0123456][0123456789AB]$"},
                                            {34,"^[01234567][0123456789A]$"},
                                            {35,"^[012345678][0123456789]$"},
                                            {36,"^[0123456][0123456789ABC]$"},
                                            {37,"^[01234567][0123456789AB]$"},
                                            {38,"^[01234567][0123456789AB]$"},
                                            {39,"^[012345678][0123456789A]$"},
                                            {40,"^[0123456789][0123456789]$"},
                                            {41,"^[01234567][0123456789ABC]$"},
                                            {42,"^[012345678][0123456789AB]$"},
                                            {43,"^[0123456789][0123456789A]$"},
                                            {44,"^[01234567][0123456789ABCD]$"},
                                            {45,"^[012345678][0123456789ABC]$"},
                                            {46,"^[012345678][0123456789ABC]$"},
                                            {47,"^[0123456789][0123456789AB]$"},
                                            {48,"^[0123456789A][0123456789A]$"},
                                            {49,"^[012345678][0123456789ABCD]$"},
                                            {50,"^[01234567][0123456789ABCDEF]$"},
                                            {51,"^[0123456789][0123456789ABC]$"},
                                            {52,"^[0123456789A][0123456789AB]$"},
                                            {53,"^[012345678][0123456789ABCDE]$"},
                                            {54,"^[0123456789][0123456789ABCD]$"},
                                            {55,"^[0123456789][0123456789ABCD]$"},
                                            {56,"^[0123456789A][0123456789ABC]$"},
                                            {57,"^[0123456789AB][0123456789AB]$"},
                                            {58,"^[0123456789][0123456789ABCDE]$"},
                                            {59,"^[0123456789][0123456789ABCDE]$"},
                                            {60,"^[0123456789A][0123456789ABCD]$"},
                                            {61,"^[0123456789AB][0123456789ABC]$"},
                                            {62,"^[0123456789AB][0123456789ABC]$"},
                                            {63,"^[0123456789][0123456789ABCDEF]$"},
                                            {64,"^[0123456789A][0123456789ABCDE]$"},
                                            {65,"^[0123456789A][0123456789ABCDE]$"},
                                            {66,"^[0123456789AB][0123456789ABCD]$"},
                                            {67,"^[0123456789ABC][0123456789ABC]$"},
                                            {68,"^[0123456789A][0123456789ABCDEF]$"},
                                            {69,"^[0123456789A][0123456789ABCDEF]$"},
                                            {70,"^[0123456789AB][0123456789ABCDE]$"},
                                            {71,"^[0123456789ABC][0123456789ABCD]$"},
                                            {72,"^[0123456789ABC][0123456789ABCD]$"},
                                            {73,"^[0123456789ABC][0123456789ABCD]$"},
                                            {74,"^[0123456789AB][0123456789ABCDEF]$"},
                                            {75,"^[0123456789AB][0123456789ABCDEF]$"},
                                            {76,"^[0123456789ABC][0123456789ABCDE]$"},
                                            {77,"^[0123456789ABCD][0123456789ABCD]$"},
                                            {78,"^[0123456789ABCD][0123456789ABCD]$"},
                                            {79,"^[0123456789ABCD][0123456789ABCD]$"},
                                            {80,"^[0123456789ABC][0123456789ABCDEF]$"},
                                            {81,"^[0123456789ABC][0123456789ABCDEF]$"},
                                            {82,"^[0123456789ABCD][0123456789ABCDE]$"},
                                            {83,"^[0123456789ABCD][0123456789ABCDE]$"},
                                            {84,"^[0123456789ABCD][0123456789ABCDE]$"},
                                            {85,"^[0123456789ABCD][0123456789ABCDE]$"},
                                            {86,"^[0123456789ABCD][0123456789ABCDEF]$"},
                                            {87,"^[0123456789ABCD][0123456789ABCDEF]$"},
                                            {88,"^[0123456789ABCDE][0123456789ABCDE]$"},
                                            {89,"^[0123456789ABCDE][0123456789ABCDE]$"},
                                            {90,"^[0123456789ABCDE][0123456789ABCDE]$"},
                                            {91,"^[0123456789ABCDE][0123456789ABCDE]$"},
                                            {92,"^[0123456789ABCDE][0123456789ABCDEF]$"},
                                            {93,"^[0123456789ABCDE][0123456789ABCDEF]$"},
                                            {94,"^[0123456789ABCDE][0123456789ABCDEF]$"},
                                            {95,"^[0123456789ABCDE][0123456789ABCDEF]$"},
                                            {96,"^[0123456789ABCDE][0123456789ABCDEF]$"},
                                            {97,"^[0123456789ABCDEF][0123456789ABCDEF]$"},
                                            {98,"^[0123456789ABCDEF][0123456789ABCDEF]$"},
                                            {99,"^[0123456789ABCDEF][0123456789ABCDEF]$"},
                                            {100,"^[0123456789ABCDEF][0123456789ABCDEF]$"}
        };

        /// <summary>
        /// 配置key类型枚举
        /// </summary>
        private enum SwitchKeyTypeEnum
        {
            /// <summary>
            /// 白名单（true）
            /// </summary>
            T,

            /// <summary>
            /// 黑名单（false）
            /// </summary>
            F,

            /// <summary>
            /// 白名单正则（true）
            /// </summary>
            RT,

            /// <summary>
            /// 黑名单正则（false）
            /// </summary>
            RF,

            /// <summary>
            /// 既不是白名单也不在黑名单，最终的判断类型
            /// </summary>
            V
        }

        /// <summary>
        /// 开关是否打开（仅正则表达式区分大小写，其它参数均先转化为大写后再进行匹配）
        /// 如：IsOpen("T=admin,test&amp;RT=^user200.*$&amp;F=user1,user2&amp;RF=^user100.*$&amp;V=20","admin");
        /// </summary>
        /// <param name="str">
        /// 配置项的值，一般是从数据库的配置表中读取
        /// 格式："T=admin,test&amp;RT=^user200.*$&amp;F=user1,user2&amp;RF=^user100.*$&amp;V=20"
        /// 说明：
        /// 1、T后面的值为白名单，用英文,隔开，如果flag在此值中存在，则返回true
        /// 2、F后面的值为黑名单，用英文,隔开，如果flag在此值中存在，则返回false
        /// 3、RT后面的值为白名单正则，如果flag的值能匹配，则返回true
        /// 4、RF后面的值为黑名单正则，如果flag的值能匹配，则返回false
        /// 5、当均不在黑白名单时，则使用V后面的值 ，该值为字符T或F或0~100之间的数字，当为T时，返回true；当为F时，返回false；当为数字时，即为百分比，由系统根据一定算法计算flag，并返回true或false
        /// 6、TFV之间用&amp;隔开，类似url查询字符串
        /// 7、当整个配置值为T，则返回true；当整个配置值为空、F或不符合格式要求时，则返回false
        /// </param>
        /// <param name="flag">百分比控制时的标志字符串，比如用户名：admin，或用户ID：1001</param>
        /// <returns>.Result=true：开，.Result=false：关</returns>
        public static MethodResult<bool> IsOpen(string str, string flag = "")
        {
            int? intVal = null;
            var val = string.Empty;
            var result = new MethodResult<bool>();
            result.Result = false;

            str = (str ?? "").Trim().Replace("+", "%2b");
            flag = (flag ?? "").Trim().ToUpper();

            #region 配置格式校验

            //空字符串
            if (string.IsNullOrWhiteSpace(str))
            {
                result.Result = false;
                result.Message = "未指定配置项";
                return result;
            }

            //T
            if (str.Equals(SwitchKeyTypeEnum.T.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result.Result = true;
                result.Message = "配置项为T，全开";
                return result;
            }

            //F
            if (str.Equals(SwitchKeyTypeEnum.F.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                result.Result = false;
                result.Message = "配置项为F，全关";
                return result;
            }

            //符合基本格式
            var nv = System.Web.HttpUtility.ParseQueryString(str);
            if (null == nv || nv.Count == 0)
            {
                result.Message = "必须使用有效的格式，如：【T=admin,test&RT=^user200.*$&F=user1,user2&RF=^user100.*$&V=20】，用&符分开，再用=赋值";
                result.Result = false;
                return result;
            }

            //配置的key必须在枚举SwitchKeyTypeEnum中
            var enumKeys = XCLNetTools.Enum.EnumHelper.GetList(typeof(SwitchKeyTypeEnum)).Select(k => k.Text).ToList();
            var allKeys = nv.AllKeys.Select(k => k.ToUpper()).ToList();
            if (allKeys.Exists(k => !enumKeys.Contains(k)))
            {
                result.Message = "只允许存在T、RT、F、RF、V的赋值，如：【T=admin,test&RT=^user200.*$&F=user1,user2&RF=^user100.*$&V=20】！";
                result.Result = false;
                return result;
            }

            if (!allKeys.Contains(SwitchKeyTypeEnum.V.ToString()) && (allKeys.Contains(SwitchKeyTypeEnum.T.ToString()) || allKeys.Contains(SwitchKeyTypeEnum.F.ToString()) || allKeys.Contains(SwitchKeyTypeEnum.RT.ToString()) || allKeys.Contains(SwitchKeyTypeEnum.RF.ToString())))
            {
                result.Message = "配置了黑名单或白名单，必须配置V的值！";
                result.Result = false;
                return result;
            }

            val = (nv[SwitchKeyTypeEnum.V.ToString()] ?? "").Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(val))
            {
                result.Message = "配置V的值不能为空！";
                result.Result = false;
                return result;
            }

            intVal = XCLNetTools.Common.DataTypeConvert.ToIntNull(val);

            if ((!intVal.HasValue && val != SwitchKeyTypeEnum.T.ToString() && val != SwitchKeyTypeEnum.F.ToString()) || (intVal.HasValue && (intVal < 0 || intVal > 100)))
            {
                result.Message = "配置V的值只能为：T或F或0~100之间的数字！";
                result.Result = false;
                return result;
            }

            #endregion 配置格式校验

            if (!string.IsNullOrWhiteSpace(flag))
            {
                try
                {
                    //F
                    val = (nv[SwitchKeyTypeEnum.F.ToString()] ?? "").Trim().ToUpper();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (val.Split(',').Contains(flag))
                        {
                            result.Result = false;
                            result.Message = "命中黑名单";
                            return result;
                        }
                    }

                    //RF
                    val = (nv[SwitchKeyTypeEnum.RF.ToString()] ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (new Regex(val).IsMatch(flag))
                        {
                            result.Result = false;
                            result.Message = "命中黑名单正则";
                            return result;
                        }
                    }

                    //T
                    val = (nv[SwitchKeyTypeEnum.T.ToString()] ?? "").Trim().ToUpper();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (val.Split(',').Contains(flag))
                        {
                            result.Result = true;
                            result.Message = "命中白名单";
                            return result;
                        }
                    }

                    //RT
                    val = (nv[SwitchKeyTypeEnum.RT.ToString()] ?? "").Trim();
                    if (!string.IsNullOrWhiteSpace(val))
                    {
                        if (new Regex(val).IsMatch(flag))
                        {
                            result.Result = true;
                            result.Message = "命中白名单正则";
                            return result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Result = false;
                    result.Message = "发生了异常：" + ex.Message;
                    return result;
                }
            }

            //V
            val = (nv[SwitchKeyTypeEnum.V.ToString()] ?? "").Trim().ToUpper();
            if (string.IsNullOrWhiteSpace(val))
            {
                result.Result = false;
                result.Message = "V为空";
                return result;
            }

            if (val.Equals(SwitchKeyTypeEnum.T.ToString()))
            {
                result.Result = true;
                result.Message = "V为T";
                return result;
            }

            if (val.Equals(SwitchKeyTypeEnum.F.ToString()))
            {
                result.Result = false;
                result.Message = "V为F";
                return result;
            }

            if (intVal == 0)
            {
                result.Result = false;
                result.Message = "V为0";
                return result;
            }

            if (intVal == 100)
            {
                result.Result = true;
                result.Message = "V为100";
                return result;
            }

            if (string.IsNullOrWhiteSpace(flag))
            {
                result.Result = false;
                result.Message = "标识flag为空";
                return result;
            }

            string flagMd5 = XCLNetTools.Encrypt.MD5.EncodeMD5(flag);
            result.Result = new Regex(percentDic[intVal.Value]).IsMatch(flagMd5[0].ToString() + flagMd5[1].ToString());
            result.Message = "系统自动计算";

            return result;
        }

        #endregion 开关配置和开关的百分比控制

        /// <summary>
        /// 将T/F转为bool值
        /// </summary>
        /// <param name="val">T或F</param>
        /// <returns>T:true,F:false</returns>
        public static bool TFToBool(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return false;
            }
            val = val.ToUpper();
            if (val == "T")
            {
                return true;
            }
            if (val == "F")
            {
                return false;
            }
            throw new ArgumentOutOfRangeException("val", "val只能为：T或F");
        }
    }
}