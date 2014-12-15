using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JIAOFENG.Practices.Library.Utility
{
    public class DateHelper
    {
        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetLastDayOfMonth(int year, int month)
        {
            DateTime lastDay = new DateTime(year, month, new System.Globalization.GregorianCalendar().GetDaysInMonth(year, month));
            int day = lastDay.Day;
            return day;

            //if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            //{
            //    return 31;
            //}
            //else if (month == 4 || month == 6 || month == 9 || month == 11)
            //{
            //    return 30;
            //}
            //else
            //{
            //    if ((year % 400 == 0) || (year % 4 == 0 && year % 100 > 0))
            //    {
            //        return 29;
            //    }
            //    else
            //    {
            //        return 28;
            //    }
            //}
        }

        /// <summary>   
        /// 判断两个日期是否在同一周   
        /// </summary>   
        /// <param name="dtStart">开始日期</param>   
        /// <param name="dtEnd">结束日期</param>  
        /// <returns></returns>   
        public static bool IsInSameWeek(DateTime dtStart, DateTime dtEnd)
        {
            TimeSpan ts = dtEnd - dtStart;
            double dbl = ts.TotalDays;
            int intDow = Convert.ToInt32(dtEnd.DayOfWeek);
            //把星期日的0转换为7
            if (intDow == 0) intDow = 7;

            //时间间隔大于7（一周时间）,或者时间间隔大于后者的星期数
            if (dbl >= 7 || dbl >= intDow)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region 返回时间差
        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            try
            {
                //TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
                //TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
                //TimeSpan ts = ts1.Subtract(ts2).Duration();
                TimeSpan ts = DateTime2 - DateTime1;
                if (ts.Days >= 1)
                {
                    dateDiff = DateTime1.Month.ToString() + "月" + DateTime1.Day.ToString() + "日";
                }
                else
                {
                    if (ts.Hours > 1)
                    {
                        dateDiff = ts.Hours.ToString() + "小时前";
                    }
                    else
                    {
                        dateDiff = ts.Minutes.ToString() + "分钟前";
                    }
                }
            }
            catch
            { }
            return dateDiff;
        }
        #endregion


        /// <summary>

        /// <summary>
        /// 得到dt是星期几
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="enOrch">0返回中文，1返回英文</param>
        /// <param name="daysInWeek">得到一周中的第几天</param>
        /// <returns></returns>
        public static string GetWeek(DateTime date, int enOrch, ref int daysInWeek)
        {
            if (enOrch > 1)
            {
                enOrch = 0;
            }
            //Sunday Monday Tuesday Wednesday Thursday Friday Saturday
            List<string> listWeek = new List<string>();
            listWeek.Add("星期一,Monday");
            listWeek.Add("星期二,Tuesday");
            listWeek.Add("星期三,Wednesday");
            listWeek.Add("星期四,Thursday");
            listWeek.Add("星期五,Friday");
            listWeek.Add("星期六,Saturday");
            listWeek.Add("星期日,Sunday");
            string strWeek = string.Empty;
            string w_1 = string.Empty;
            w_1 = date.DayOfWeek.ToString();
            for (int i = 0; i < listWeek.Count; i++)
            {
                if (w_1 == listWeek[i].Split(',')[1])
                {
                    strWeek = listWeek[i].Split(',')[enOrch];
                    daysInWeek = i + 1;
                }
            }
            if (strWeek == string.Empty)
            {
                strWeek = w_1;
            }
            return strWeek;
        }
    }
}
