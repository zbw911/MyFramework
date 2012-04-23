using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.Tool
{
    /// <summary>
    /// 跟踪效果分析
    /// 
    /// 2011-7-18 再一次修正了跟踪方法的实现，使用httpcontext作为每一个request 请求使用一个数据，张保维
    /// </summary>
    public class TrackEffect
    {
        private List<string> _trackinfos = new List<string>();
        //记录全部的信息

        private static List<string> trackinfos
        {
            get
            {
                if (System.Web.HttpContext.Current.Items["_trackinfos"] as List<string> == null)
                    System.Web.HttpContext.Current.Items["_trackinfos"] = new List<string>();

                var list =  System.Web.HttpContext.Current.Items["_trackinfos"] as List<string>;

                return list;
            }
        }

        /// <summary>
        /// 刷新，使用当前数据生效
        /// </summary>
        public void Flush()
        {
            trackinfos.AddRange(_trackinfos);
        }

        /// <summary>
        /// 全部的跟跟踪信息
        /// </summary>
        public List<string> Trackinfos
        {
            get { return trackinfos; }
            //set { trackinfos = value; }
        }

        public static  string TrackinfosString
        {
            get { return string.Join("", trackinfos); }
            //set { trackinfos = value; }
        }

        CodeRunTime Cr = null;

        /// <summary>
        /// 开始一段跟踪
        /// </summary>
        /// <param name="discript"></param>
        public void Begin(string discript)
        {
            Cr = new CodeRunTime(discript);
        }
        /// <summary>
        /// 开始一段跟踪
        /// </summary>
        public void End()
        {
            if (Cr == null)
                throw new Exception("错误的调用，未使用BEGIN（）");
            _trackinfos.Add(Cr.End());

            Cr = null;
        }
    }
}
