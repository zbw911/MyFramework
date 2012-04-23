using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kt.Framework.Filter;
//using Kt.Framework.User;

namespace Kt.Main.Core
{
    [ErrorFilter]
    public class BaseController : Controller
    {
       

        /// <summary>
        /// 返回javascript , 虽然有个 JavascriptAction 但好像不太好用，这个反而比那个好用。
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        protected ActionResult JavascriptContent(string script)
        {
            return this.Content("<script>" + script + "</script>");
        }

        protected ActionResult JavascriptMessage(string messge, int delaySecond = 3)
        {
            return this.JavascriptContent("show_message('" + messge + "', " + delaySecond + ")");
        }

        protected ActionResult JavascriptMessageBox(string script)
        {
            return this.JavascriptContent("window.alert('" + script + "')");
        }
        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="messagelist"></param>
        /// <param name="redirectto"></param>
        /// <param name="to_title"></param>
        /// <param name="time"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        protected ActionResult Message(List<string> messagelist, string redirectto = "", string to_title = "跳转", int time = 3, string return_msg = "")
        {
            if (redirectto == "") redirectto = Kt.Framework.Common.HttpServerInfo.GetUrlReferrer();
            var model = new Kt.Framework.Models.Messager
            {
                MessageList = messagelist,
                redirectto = redirectto,
                to_title = to_title,
                time = time,
                return_msg = return_msg

            };
            return View("_Messager", model);
        }

        /// <summary>
        /// 注册导航的信息提示
        /// </summary>
        /// <param name="messagelist"></param>
        /// <param name="redirectto"></param>
        /// <param name="to_title"></param>
        /// <param name="time"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        protected ActionResult Message_Nav(List<string> messagelist, string redirectto = "", string tips = "", string to_title = "跳转", int time = 3, string return_msg = "")
        {
            if (redirectto == "") redirectto = Kt.Framework.Common.HttpServerInfo.GetUrlReferrer();
            var model = new Kt.Framework.Models.Messager
            {
                MessageList = messagelist,
                redirectto = redirectto,
                to_title = to_title,
                time = time,
                return_msg = return_msg,
                tips = tips
            };
            return View("_Messager_Nav", model);
        }


        /// <summary>
        /// 提示消息
        /// </summary>
        /// <param name="strmessage"></param>
        /// <param name="redirectto"></param>
        /// <param name="to_title"></param>
        /// <param name="time"></param>
        /// <param name="return_msg"></param>
        /// <returns></returns>
        protected ActionResult Message(string strmessage, string redirectto = "", string to_title = "跳转", int time = 3, string return_msg = "")
        {
            return this.Message(new List<string> { strmessage }, redirectto, to_title, time, return_msg);
        }



        protected RedirectToRouteResult RedirectToSelf(object routeValues)
        {
            RouteValueDictionary routeData = new RouteValueDictionary(routeValues);
            string controller = (string)this.ControllerContext.RouteData.Values["controller"];
            string action = (string)this.ControllerContext.RouteData.Values["action"];
            routeData.Add("controller", controller);
            routeData.Add("action", action);

            return RedirectToRoute(routeData);
        }

        protected RedirectToRouteResult RedirectToSelf()
        {
            return RedirectToSelf(null);
        }



        /// <summary>
        /// 按需要跳转
        /// </summary>
        /// <returns></returns>
        protected RedirectToRouteResult UserStatusRedirect()
        {
            //需要激活 
            //if (Kt.Framework.User.UserState.getIsNeedActived())
            //{
            //    RouteValueDictionary routeData = new RouteValueDictionary(new { controller = "RegNav", Action = "Activateaccount", Area = "" });
            //    return RedirectToRoute(routeData);
            //}
            //在这里调用            
            if (!Kt.Framework.User.User.IS_LOGIN)
            {
                //RouteValueDictionary routeData = new RouteValueDictionary(new { controller = "RegNav", Action = "logOn", Area = "" });
                //return RedirectToRoute(routeData);
                
                //在这里检验Cookie["ktweb_auth"]是否存在，是否没有过期，如果是，则更新用户登录状态，否则            
                if (Request.Cookies[UserCookies.SECURELOGIN] != null && Request.Cookies[UserCookies.SECURELOGIN].Expires < DateTime.Now)
                //if (Response.Cookies[UserCookies.SECURELOGIN] != null && Response.Cookies[UserCookies.SECURELOGIN].Expires < DateTime.Now)
                {
                    //在这里让用户进行登录，调用Ucenter的方法将Cookies["ktweb_auth"].Value解码，然后拆出来uid，存入到记录用户登录状态的cookie中，以达到用户“免登录”的效果
                    //var cookievalue = Response.Cookies[UserCookies.SECURELOGIN].Value;
                    var cookievalue = Request.Cookies[UserCookies.SECURELOGIN].Value;
                    if (!string.IsNullOrEmpty(cookievalue))
                    {
                        string userid = UserCookies.getSecureLoginCookies(UserCookies.AUTHCOOKIE_NAME_USERID);
                        string logidId = UserCookies.getSecureLoginCookies(UserCookies.AUTHCOOKIE_NAME_USERNAME);
                        string email = UserCookies.getSecureLoginCookies(UserCookies.AUTHCOOKIE_NAME_EMAIL);
                        string nickname = UserCookies.getSecureLoginCookies(UserCookies.AUTHCOOKIE_NAME_NICKNAME);
                        int expire = UserCookies.COOKIE_EXPIRETIME * 14;

                        var uid = decimal.Parse(userid);

                        //在这里设置最后登录状态
                        var oline = new UserOnline();

                        oline.AddUser(new UserInfo { LASTActive = DateTime.Now, Uid = uid });
                        UserCookies.setAuthCookies(uid, logidId, email, expire, nickname);

                        //设置在线状态
                        Kt.Framework.User.User.Set_MEMBER_ID(uid);
                        RouteValueDictionary routeData = new RouteValueDictionary(new
                        {
                            controller = "Index",
                            Action = "Index",
                            Area = "UserHome"
                        });
                        return RedirectToRoute(routeData);
                    }
                }
                else
                {
                    RouteValueDictionary routeData = new RouteValueDictionary(new { controller = "RegNav", Action = "logOn", Area = "" });
                    return RedirectToRoute(routeData);
                }

            }
            return null;
        }

        [FlagsAttribute]
        protected enum UserStatLevel : short
        {
            激活 = 1,
            登录 = 2,
            无 = 4
        }

        /// <summary>
        /// 用户状态
        /// </summary>
        /// <param name="userStatLevel"></param>
        /// <returns></returns>
        protected bool checkUserStatus(UserStatLevel userStatLevel = ( UserStatLevel.登录 | UserStatLevel.激活))
        {

            if ((UserStatLevel.激活 & userStatLevel) == UserStatLevel.激活 && Kt.Framework.User.UserState.getIsNeedActived())
            {
                return true;
            }
            if ((UserStatLevel.登录 & userStatLevel) == UserStatLevel.登录 && !Kt.Framework.User.User.IS_LOGIN)
            {
                return true;
            }
            return false;
        }

    }
}
