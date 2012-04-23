using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.Common
{
    /// <summary>
    /// 时间帮助方法  
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// 时间格式化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string my_date_format2(DateTime dateTime, string format = "M月d日 H时m分")
        {

            TimeSpan ts = System.DateTime.Now - dateTime;

            //一天以前显示 
            if (ts.Days >= 1)
            {
                return my_date_format(dateTime, format);
            }
            else if (ts.Days < 1 && ts.Hours >= 1)
            {
                return ts.Hours + "小时" + (ts.Minutes > 0 ? ts.Minutes + "分钟" : "") + "前";
            }
            else if (ts.Hours < 1 && ts.Minutes > 1)
            {
                return ts.Minutes + "分钟前";
            }
            else
            {
                return "刚刚";
            }
        }

        /// <summary>
        /// 常用时间格式化
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string my_date_format(System.DateTime timestamp, string format = "Y-M-d H:i:s")
        {
            return timestamp.ToString(format);
        }


    }
}
