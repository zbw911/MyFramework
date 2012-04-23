using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kt.Framework.Common
{
    public class HttpServerInfo
    {
        /// <summary>
        /// 当前系统网点地址
        /// </summary>
        /// <returns></returns>
        public static string GetHost()
        {
            //HttpContext.Current.Request.
            return HttpContext.Current.Request.Url.Host.ToString();
        }
        /// <summary>
        /// 全地址包括？后的
        /// </summary>
        /// <returns></returns>
        public static string FullRequestPath()
        {
            return HttpContext.Current.Request.Url.PathAndQuery;
        }

        /// <summary>
        /// 请求端口
        /// </summary>
        /// <returns></returns>
        public static int GetPort()
        {
            return HttpContext.Current.Request.Url.Port;
        }

        /// <summary>
        /// 请求类型
        /// </summary>
        /// <returns></returns>
        public static string GetRequestType()
        {
            return HttpContext.Current.Request.Url.Scheme;
        }
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string BaseUrl
        {
            get { return GetRequestType() + "://" + GetHost() + (GetPort() == 80 ? "" : ":" + GetPort()) + "/"; }
        }

        /// <summary>
        /// Agent
        /// </summary>
        /// <returns></returns>
        public static string GetRequestAgent()
        {
            return HttpContext.Current.Request.UserAgent;
        }

        /// <summary>
        /// 返回上一个页面的地址
        /// </summary>
        /// <returns>上一个页面的地址</returns>
        public static string GetUrlReferrer()
        {
            string retVal = null;

            try
            {
                retVal = HttpContext.Current.Request.UrlReferrer.ToString();
            }
            catch { }

            if (retVal == null)
                return "";

            return retVal;
        }
        /// <summary>
        /// 系统根路径
        /// </summary>

        public static string RELATIVE_ROOT_PATH = HttpContext.Current.Server.MapPath("~");
    }
}
