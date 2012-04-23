using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kt.Framework.Common
{
    public class CookieFun
    {
        /// <summary>
        /// 设置 cookies
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <param name="domain"></param>
        public static void setcookie(string cookieName, string value, TimeSpan? timespan = null, string domain = "")
        {
            HttpCookie cookies = new HttpCookie(cookieName, value);

            if (!string.IsNullOrEmpty(domain))
                cookies.Domain = domain;

            if (timespan != null)
                cookies.Expires = System.DateTime.Now.Add((TimeSpan)timespan);

            HttpContext.Current.Response.AppendCookie(cookies); //.a.Cookies.Add(cookies);
        }


        public static string getcookie(string cookieName)
        {
            if (!IsExistCookies(cookieName)) return null;

            return HttpContext.Current.Request.Cookies[cookieName].Value;
        }

        ///**
        // * 是否存在
        // * @param unknown_type $cookieName
        // */
        public static bool IsExistCookies(string cookieName)
        {
            return HttpContext.Current.Request.Cookies[cookieName] != null;
        }
    }
}
