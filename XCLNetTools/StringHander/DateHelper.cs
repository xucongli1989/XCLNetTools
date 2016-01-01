using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace XCLNetTools.StringHander
{
    /// <summary>
    /// 日期时间处理相关
    /// </summary>
    public static class DateHelper
    {
        #region 剩余天数相关

        /// <summary>
        /// 返回指定日期到该日期所在月结束的剩余天数
        /// </summary>
        public static int DaysLeftInMonth(this DateTime Date)
        {
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInMonth(Date.Year, Date.Month) - Date.Day;
        }

        /// <summary>
        /// 返回指定日期到该日期所在年结束的剩余天数
        /// </summary>
        public static int DaysLeftInYear(this DateTime Date)
        {
            return Thread.CurrentThread.CurrentCulture.Calendar.GetDaysInYear(Date.Year) - Date.DayOfYear;
        }

        /// <summary>
        /// 返回指定日期到所在周结束的剩余天数
        /// </summary>
        public static int DaysLeftInWeek(this DateTime Date)
        {
            return 7 - ((int)Date.DayOfWeek + 1);
        }

        #endregion 剩余天数相关

        #region Unix与DateTime转换

        /// <summary>
        /// 将Unix（int）转为DateTime
        /// </summary>
        /// <param name="Date">Unix</param>
        /// <returns>DateTime</returns>
        public static DateTime FromUnixTime(int Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        ///  将Unix（long）转为DateTime
        /// </summary>
        public static DateTime FromUnixTime(long Date)
        {
            return new DateTime((Date * TimeSpan.TicksPerSecond) + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks, DateTimeKind.Utc);
        }

        /// <summary>
        /// 将DateTime转为Unix
        /// </summary>
        public static int ToUnix(DateTime Date)
        {
            return (int)((Date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks / TimeSpan.TicksPerSecond);
        }

        #endregion Unix与DateTime转换

        #region 时间间隔相关

        /// <summary>
        /// 获取两个时间的时间间隔，如：“小时:分钟:秒”
        /// </summary>
        /// <returns></returns>
        public static string GetTimeString(DateTime dtStart, DateTime dtEnd)
        {
            int h, m, s;
            double sec = dtEnd.Subtract(dtStart).TotalSeconds;
            h = (int)(sec / 3600);
            m = (int)((sec % 3600) / 60);
            s = (int)((sec % 3600) % 60);
            return string.Format("{0}：{1}：{2}", h, m, s); ;
        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期字符串</param>
        /// <param name="d2">要参与计算的另一个日期字符串</param>
        /// <returns>一个表示日期间隔的TimeSpan类型</returns>
        public static TimeSpan GetTimeInterval(string d1, string d2)
        {
            try
            {
                DateTime date1 = DateTime.Parse(d1);
                DateTime date2 = DateTime.Parse(d2);
                return GetTimeInterval(date1, date2);
            }
            catch
            {
                throw new Exception("字符串参数不正确!");
            }
        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期</param>
        /// <param name="d2">要参与计算的另一个日期</param>
        /// <returns>一个表示日期间隔的TimeSpan类型</returns>
        public static TimeSpan GetTimeInterval(DateTime d1, DateTime d2)
        {
            TimeSpan ts;
            if (d1 > d2)
            {
                ts = d1 - d2;
            }
            else
            {
                ts = d2 - d1;
            }
            return ts;
        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期字符串</param>
        /// <param name="d2">要参与计算的另一个日期字符串</param>
        /// <param name="drf">决定返回值形式的枚举</param>
        /// <returns>一个代表年月日的int数组，具体数组长度与枚举参数drf有关</returns>
        public static int[] GetTimeInterval(string d1, string d2, diffResultFormat drf)
        {
            try
            {
                DateTime date1 = DateTime.Parse(d1);
                DateTime date2 = DateTime.Parse(d2);
                return GetTimeInterval(date1, date2, drf);
            }
            catch
            {
                throw new Exception("字符串参数不正确!");
            }
        }

        /// <summary>
        /// 计算日期间隔
        /// </summary>
        /// <param name="d1">要参与计算的其中一个日期</param>
        /// <param name="d2">要参与计算的另一个日期</param>
        /// <param name="drf">决定返回值形式的枚举</param>
        /// <returns>一个代表年月日的int数组，具体数组长度与枚举参数drf有关</returns>
        public static int[] GetTimeInterval(DateTime d1, DateTime d2, diffResultFormat drf)
        {
            #region 数据初始化

            DateTime max;
            DateTime min;
            int year;
            int month;
            int tempYear, tempMonth;
            if (d1 > d2)
            {
                max = d1;
                min = d2;
            }
            else
            {
                max = d2;
                min = d1;
            }
            tempYear = max.Year;
            tempMonth = max.Month;
            if (max.Month < min.Month)
            {
                tempYear--;
                tempMonth = tempMonth + 12;
            }
            year = tempYear - min.Year;
            month = tempMonth - min.Month;

            #endregion 数据初始化

            #region 按条件计算

            if (drf == diffResultFormat.dd)
            {
                TimeSpan ts = max - min;
                return new int[] { ts.Days };
            }
            if (drf == diffResultFormat.mm)
            {
                return new int[] { month + year * 12 };
            }
            if (drf == diffResultFormat.yy)
            {
                return new int[] { year };
            }
            return new int[] { year, month };

            #endregion 按条件计算
        }

        /// <summary>
        /// 关于返回值形式的枚举
        /// </summary>
        public enum diffResultFormat
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
            dd,
        }

        #endregion 时间间隔相关

        #region 时间差

        /// <summary>
        /// 返回指定时间段内不包括某一时间段的时间差（以小时为单位）
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="workStart">排除的时间段（开始）如:"08:00"</param>
        /// <param name="workEnd">排除的时间段（结束）如:"18:00"</param>
        /// <returns>decimal（小时）</returns>
        public static decimal GetTimeSub(DateTime? start, DateTime? end, string workStart, string workEnd)
        {
            decimal sum = 0m;
            DateTime dtNow = DateTime.Now;
            DateTime dtStart = XCLNetTools.Common.DataTypeConvert.ToDateTime(Convert.ToString(start), dtNow);
            DateTime dtEnd = XCLNetTools.Common.DataTypeConvert.ToDateTime(Convert.ToString(end), dtNow);
            if (dtStart > dtEnd)
            {
                return 0;
            }
            DateTime oldStartDay = Convert.ToDateTime(dtStart.ToShortDateString());
            DateTime oldEndDay = Convert.ToDateTime(dtEnd.ToShortDateString());
            String oldStartStr = dtStart.ToShortTimeString();
            String oldEndStr = dtEnd.ToShortTimeString();
            String newStartStr = workStart;//如："08:00" 工作时间（始）
            String newEndStr = workEnd;//如："18:00" 工作时间（终）
            String[] oldStart = oldStartStr.Split(':');
            String[] oldEnd = oldEndStr.Split(':');
            String[] newStart = newStartStr.Split(':');
            String[] newEnd = newEndStr.Split(':');
            int oldStartSec = XCLNetTools.Common.DataTypeConvert.ToInt(oldStart[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(oldStart[1]) * 60;
            int oldEndSec = XCLNetTools.Common.DataTypeConvert.ToInt(oldEnd[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(oldEnd[1]) * 60;
            int newStartSec = XCLNetTools.Common.DataTypeConvert.ToInt(newStart[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(newStart[1]) * 60;
            int newEndSec = XCLNetTools.Common.DataTypeConvert.ToInt(newEnd[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(newEnd[1]) * 60;
            if (String.Equals(oldStartDay, oldEndDay))
            { //开始日期和结束日期是同一天
                String strStart = (oldStartSec > newStartSec) ? oldStartStr : newStartStr;
                String strEnd = (oldEndSec > newEndSec) ? newEndStr : oldEndStr;
                DateTime dtSamDayStartTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", oldStartDay), strStart));
                DateTime dtSamDayEndTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", oldEndDay), strEnd));
                if (dtSamDayStartTime <= dtSamDayEndTime)
                {
                    sum = Convert.ToDecimal(Math.Round(dtSamDayEndTime.Subtract(dtSamDayStartTime).TotalSeconds / 3600.0, 1));
                }
            }
            else
            {  //开始日期和结束日期不是同一天，且结束日期大于开始日期（数据验证中有验证）
                int span = oldEndDay.Subtract(oldStartDay).Days;
                DateTime dt = oldStartDay;
                String strStart = (oldStartSec > newStartSec) ? oldStartStr : newStartStr;
                String strEnd = (oldEndSec > newEndSec) ? newEndStr : oldEndStr;
                String st = String.Empty;
                String se = String.Empty;
                for (int i = 0; i <= span; i++)
                {
                    st = i == 0 ? strStart : newStartStr;
                    se = i == span ? strEnd : newEndStr;
                    DateTime dtDiffStartTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", dt), st));
                    DateTime dtDiffEndTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", dt), se));
                    if (dtDiffStartTime <= dtDiffEndTime)
                    {
                        sum += Convert.ToDecimal(Math.Round(dtDiffEndTime.Subtract(dtDiffStartTime).TotalSeconds / (3600.0), 1));
                    }
                }
            }
            return sum;
        }

        /// <summary>
        /// 返回指定时间段内拆分后的的时间差（以小时为单位）的list
        /// </summary>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <param name="workStart">排除的时间段（开始）如:"08:00"</param>
        /// <param name="workEnd">排除的时间段（结束）如:"18:00"</param>
        /// <returns>decimal（小时）</returns>
        public static List<XCLNetTools.Entity.SubTime> GetTimeSubList(DateTime? start, DateTime? end, string workStart, string workEnd)
        {
            List<Entity.SubTime> lst = new List<Entity.SubTime>();
            Entity.SubTime subTimeModel = null;
            decimal sum = 0m;
            DateTime dtNow = DateTime.Now;
            DateTime dtStart = XCLNetTools.Common.DataTypeConvert.ToDateTime(Convert.ToString(start), dtNow);
            DateTime dtEnd = XCLNetTools.Common.DataTypeConvert.ToDateTime(Convert.ToString(end), dtNow);
            if (dtStart > dtEnd)
            {
                return null;
            }
            DateTime oldStartDay = Convert.ToDateTime(dtStart.ToShortDateString());
            DateTime oldEndDay = Convert.ToDateTime(dtEnd.ToShortDateString());
            String oldStartStr = dtStart.ToShortTimeString();
            String oldEndStr = dtEnd.ToShortTimeString();
            String newStartStr = workStart;//如："08:00" 工作时间（始）
            String newEndStr = workEnd;//如："18:00" 工作时间（终）
            String[] oldStart = oldStartStr.Split(':');
            String[] oldEnd = oldEndStr.Split(':');
            String[] newStart = newStartStr.Split(':');
            String[] newEnd = newEndStr.Split(':');
            int oldStartSec = XCLNetTools.Common.DataTypeConvert.ToInt(oldStart[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(oldStart[1]) * 60;
            int oldEndSec = XCLNetTools.Common.DataTypeConvert.ToInt(oldEnd[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(oldEnd[1]) * 60;
            int newStartSec = XCLNetTools.Common.DataTypeConvert.ToInt(newStart[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(newStart[1]) * 60;
            int newEndSec = XCLNetTools.Common.DataTypeConvert.ToInt(newEnd[0]) * 3600 + XCLNetTools.Common.DataTypeConvert.ToInt(newEnd[1]) * 60;
            if (String.Equals(oldStartDay, oldEndDay))
            { //开始日期和结束日期是同一天
                String strStart = (oldStartSec > newStartSec) ? oldStartStr : newStartStr;
                String strEnd = (oldEndSec > newEndSec) ? newEndStr : oldEndStr;
                DateTime dtSamDayStartTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", oldStartDay), strStart));
                DateTime dtSamDayEndTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", oldEndDay), strEnd));
                if (dtSamDayStartTime <= dtSamDayEndTime)
                {
                    sum = Convert.ToDecimal(Math.Round(dtSamDayEndTime.Subtract(dtSamDayStartTime).TotalSeconds / 3600.0, 1));

                    subTimeModel = new Entity.SubTime();
                    subTimeModel.StartTime = dtSamDayStartTime;
                    subTimeModel.EndTime = dtSamDayEndTime;
                    subTimeModel.SumTime = sum;
                    lst.Add(subTimeModel);
                }
            }
            else
            {  //开始日期和结束日期不是同一天，且结束日期大于开始日期（数据验证中有验证）
                int span = oldEndDay.Subtract(oldStartDay).Days;
                DateTime dt = oldStartDay;
                String strStart = (oldStartSec > newStartSec) ? oldStartStr : newStartStr;
                String strEnd = (oldEndSec > newEndSec) ? newEndStr : oldEndStr;
                String st = String.Empty;
                String se = String.Empty;
                for (int i = 0; i <= span; i++)
                {
                    st = i == 0 ? strStart : newStartStr;
                    se = i == span ? strEnd : newEndStr;
                    DateTime dtDiffStartTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", dt), st));
                    DateTime dtDiffEndTime = XCLNetTools.Common.DataTypeConvert.ToDateTime(String.Format("{0} {1}", String.Format("{0:yyyy-MM-dd}", dt), se));
                    if (dtDiffStartTime <= dtDiffEndTime)
                    {
                        sum = Convert.ToDecimal(Math.Round(dtDiffEndTime.Subtract(dtDiffStartTime).TotalSeconds / (3600.0), 1));

                        subTimeModel = new Entity.SubTime();
                        subTimeModel.StartTime = dtDiffStartTime.AddDays(i);
                        subTimeModel.EndTime = dtDiffEndTime.AddDays(i);
                        subTimeModel.SumTime = sum;
                        lst.Add(subTimeModel);
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 返回汉字表示的时间（如1年3个月5天3小时）
        /// </summary>
        /// <param name="hour">小时</param>
        /// <param name="oneDayHour">一天几小时（灵活，如上班时间为一天7小时）</param>
        /// <returns></returns>
        public static string GetTimeStr(decimal hour, int oneDayHour)
        {
            int year = (int)hour / (365 * oneDayHour);
            hour = hour - year * 365 * oneDayHour;
            int month = (int)hour / (30 * oneDayHour);
            hour = hour - month * 30 * oneDayHour;
            int day = (int)hour / oneDayHour;
            hour = hour - day * oneDayHour;
            StringBuilder str = new StringBuilder();
            if (year > 0)
            {
                str.AppendFormat("{0}年", year);
            }
            if (month > 0)
            {
                str.AppendFormat("{0}月", month);
            }
            if (day > 0)
            {
                str.AppendFormat("{0}天", day);
            }
            if (hour > 0)
            {
                str.AppendFormat("{0}时", hour);
            }
            return str.ToString();
        }

        #endregion 时间差

        #region 周相关

        /// <summary>
        /// 返回星期几，如："星期一"
        /// </summary>
        public static string GetWeek(DateTime date)
        {
            string str = string.Empty;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    str = "一";
                    break;

                case DayOfWeek.Tuesday:
                    str = "二";
                    break;

                case DayOfWeek.Wednesday:
                    str = "三";
                    break;

                case DayOfWeek.Thursday:
                    str = "四";
                    break;

                case DayOfWeek.Friday:
                    str = "五";
                    break;

                case DayOfWeek.Saturday:
                    str = "六";
                    break;

                default:
                    str = "日";
                    break;
            }
            return str;
        }

        /// <summary>
        /// 获取某一日期是该年中的第几周
        /// </summary>
        /// <param name="dt">日期</param>
        /// <returns> 该日期在该年中的周数 </returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }

        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份 </param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            DateTime end = new DateTime(year, 12, 31);   // 该年最后一天 
            System.Globalization.GregorianCalendar gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday);   // 该年星期数 
        }

        #endregion 周相关

        #region 转中文日期

        /// <summary>
        /// 将数字日期格式转为中文日期格式
        /// 如：2013-01-01=》二〇一三年一月一日
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToCNString(this DateTime dt)
        {
            string CNdate = dt.ToLongDateString();//转成年月日格式
            CNdate = Regex.Replace(CNdate, @"\d+[^\d]", Rep_date);
            return CNdate;
        }

        private static string Rep_date(Match mc)
        {
            const string cnd = "〇一二三四五六七八九";
            string val = mc.Value;
            string digit = val.Substring(0, val.Length - 1);
            char c = val[val.Length - 1];
            val = "";
            switch (c)
            {
                case '年':
                    foreach (char s in digit) val += cnd[int.Parse(s.ToString())];
                    break;

                case '月':
                case '日':
                    if (digit.Length == 1)
                        val += cnd[int.Parse(digit)];
                    else
                    {
                        val += cnd[int.Parse(digit[0].ToString())] + "十";
                        val = val.TrimStart('一');
                        val += cnd[int.Parse(digit[1].ToString())].ToString().Trim('〇');
                    }
                    break;

                default:
                    return "格式错误";
            }
            return val + c.ToString();
        }

        #endregion 转中文日期

        #region 枚举项

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
        /// 获取BeforeDateTypeEnum所对应的时间
        /// （如果枚举为All,则返回DateTime.MaxValue）
        /// </summary>
        /// <param name="em">BeforeDateTypeEnum枚举</param>
        /// <returns>枚举对应的时间</returns>
        public static DateTime GetBeforeDateTypeDateTime(BeforeDateTypeEnum em)
        {
            DateTime dtNow = DateTime.Now;
            DateTime dt = DateTime.MinValue;
            switch (em)
            {
                case BeforeDateTypeEnum.All:
                    dt = DateTime.MaxValue;
                    break;

                case BeforeDateTypeEnum.HalfYear:
                    dt = dtNow.AddMonths(-6).Date;
                    break;

                case BeforeDateTypeEnum.OneMonth:
                    dt = dtNow.AddMonths(-1).Date;
                    break;

                case BeforeDateTypeEnum.OneYear:
                    dt = dtNow.AddYears(-1).Date;
                    break;

                case BeforeDateTypeEnum.SevenDay:
                    dt = dtNow.AddDays(-7).Date;
                    break;

                case BeforeDateTypeEnum.ThreeMonth:
                    dt = dtNow.AddMonths(-3).Date;
                    break;
            }
            return dt;
        }

        #endregion 枚举项

        #region 其它

        /// <summary>
        /// 返回指定日期所在月的第一天
        /// </summary>
        public static DateTime FirstDayOfMonth(this DateTime Date)
        {
            return new DateTime(Date.Year, Date.Month, 1);
        }

        /// <summary>
        /// 返回指定日期所在月的最后一天(包含时间部分)（如：2012-01-02 23:59:59）
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime dt)
        {
            return dt.AddMonths(1).FirstDayOfMonth().AddMilliseconds(-1);
        }

        /// <summary>
        /// 根据指定年月和向前推移的月数，返回起始时间
        /// 如：传入("2012-09",2)，则返回{2012-08-01,2012-09-01}
        /// </summary>
        /// <param name="yearMonth">如：2012-09</param>
        /// <param name="monthCount">如：2</param>
        /// <returns>如：{2012-08-01,2012-09-01}</returns>
        public static DateTime[] GetStartTimeEndTimeByYearMonth(string yearMonth, int monthCount)
        {
            DateTime[] dt = null;
            DateTime? dtEndTime = XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(string.Format("{0}-01", yearMonth));
            if (null != dtEndTime)
            {
                DateTime dtStartTime = Convert.ToDateTime(dtEndTime).AddMonths(-1 * monthCount + 1);
                dt = new DateTime[] { dtStartTime, XCLNetTools.Common.DataTypeConvert.ToDateTime(Convert.ToString(dtEndTime)) };
            }
            return dt;
        }

        /// <summary>
        /// 返回指定条件名和起止时间的最终条件字符串（包含等号）
        /// 如：("aaa",'2012-01-01 00:10:00',null)="aaa>='2012-01-01 00:10:00'"(无小于等于，若end不为null，则有小于等于)
        /// </summary>
        /// <param name="fieldName">条件名</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static string GetDateTimeWhereByStartEndTime(string fieldName, DateTime? start, DateTime? end)
        {
            string str = string.Empty;
            if (!string.IsNullOrEmpty(fieldName))
            {
                if (start == null && end != null)
                {
                    str = string.Format("{0}<='{1}'", fieldName, end);
                }
                else if (start != null && end == null)
                {
                    str = string.Format("{0}>='{1}'", fieldName, start);
                }
                else if (start != null && end != null)
                {
                    str = string.Format("{0}>='{1}' and {0}<='{2}'", fieldName, start, end);
                }
            }
            return str;
        }

        /// <summary>
        /// 获取日期区间
        /// </summary>
        public static String GetDateSpan(DateTime date)
        {
            DateTime dt = (DateTime)date;
            TimeSpan span = DateTime.Now - dt;
            if (span.TotalDays > 60)
            {
                return dt.ToShortDateString();
            }
            else if (span.TotalDays > 30)
            {
                return "1个月前";
            }
            else if (span.TotalDays > 14)
            {
                return "2周前";
            }
            else if (span.TotalDays > 7)
            {
                return "1周前";
            }
            else if (span.TotalDays > 1)
            {
                return string.Format("{0}天前", (int)Math.Floor(span.TotalDays));
            }
            else if (span.TotalHours > 1)
            {
                return string.Format("{0}小时前", (int)Math.Floor(span.TotalHours));
            }
            else if (span.TotalMinutes > 1)
            {
                return string.Format("{0}分钟前", (int)Math.Floor(span.TotalMinutes));
            }
            else if (span.TotalSeconds >= 1)
            {
                return string.Format("{0}秒前", (int)Math.Floor(span.TotalSeconds));
            }
            else
            {
                return "1秒前";
            }
        }

        /// <summary>
        /// 将Json序列化的时间由/Date(1294499956278)/转为DateTime? .
        /// </summary>
        public static DateTime? ConvertJsonDateToDateTime(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            Match mt = new Regex(@"\/Date\((\d+)\)\/").Match(str.Trim());
            if (mt.Success)
            {
                return new DateTime(1970, 1, 1).AddMilliseconds(long.Parse(mt.Groups[1].Value));
            }
            return null;
        }

        /// <summary>
        /// 将字符串转换为可空的日期类型，如果字符串不是有效的日期格式，则返回null
        /// </summary>
        /// <param name="s">进行转换的字符串</param>
        /// <param name="type">"start"：将此时间设置为yyyy-MM-dd 00:00:00；"end"：yyyy-MM-dd 23:59:59</param>
        /// <returns></returns>
        public static DateTime? GetStartEndDateTimeNullable(string s, string type)
        {
            DateTime? dt = XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(s, null);
            switch (type)
            {
                case "start":
                    dt = XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(string.Format("{0:yyyy-MM-dd 00:00:00}", dt));
                    break;

                case "end":
                    dt = XCLNetTools.Common.DataTypeConvert.ToDateTimeNull(string.Format("{0:yyyy-MM-dd 23:59:59}", dt));
                    break;
            }
            return dt;
        }

        /// <summary>
        /// 转换为DateTime,若为null或转换失败，则取DateTime.MinValue
        /// </summary>
        public static DateTime GetDateTimeWithMinValue(string key)
        {
            return XCLNetTools.Common.DataTypeConvert.ToDateTime(key, DateTime.MinValue);
        }

        #endregion 其它
    }
}