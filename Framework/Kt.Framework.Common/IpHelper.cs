using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kt.Framework.Common
{
    public class IpHelper
    {
        #region 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        public static string getRealIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  //得到穿过代理服务器的ip地址
            {

                UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return UserIP;
        }
        #endregion
        #region 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        /// <summary>
        /// 穿过代理服务器获得Ip地址,如果有多个IP，则第一个是用户的真实IP，其余全是代理的IP，用逗号隔开
        /// </summary>
        /// <returns></returns>
        public static string getRealIps()
        {
            string UserIP;
            if (HttpContext.Current.Request.Headers["Cdn-Src-Ip"] != null)
            {
                UserIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))  //得到穿过代理服务器的ip地址
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    UserIP = Common.StringUtil.GetFirstIp(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                else
                    UserIP = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            }
            else
            {
                UserIP = HttpContext.Current.Request.UserHostAddress;
                //UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return UserIP;
        }

        /// <summary>
        /// 获取全部IP
        /// </summary>
        /// <returns></returns>
        public static string getAllIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.Headers["Cdn-Src-Ip"] != null)
            {
                UserIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            }
            else if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  //得到穿过代理服务器的ip地址
            {

                UserIP = string.Format("{0},{1}",
                    HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            else
            {
                UserIP = HttpContext.Current.Request.UserHostAddress;
            }
            return UserIP;
        }
        #endregion
    }
}
