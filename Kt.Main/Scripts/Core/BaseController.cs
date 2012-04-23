using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kt.Framework.Filter;

namespace Kt.Main.Core
{
    [ErrorFilter]
    public class BaseController : Controller
    {
        //protected override void Execute(RequestContext requestContext)
        //{
        //    if (Kt.Framework.User.UserState.getIsNeedActived())
        //    {
        //        this.HttpContext.Response.Redirect(Kt.Framework.Common.HttpServerInfo.BaseUrl + "/Account/ActivateAccount/");
        //        //this.Redirect();
        //        return;
        //    }
        //    base.Execute(requestContext);
        //}
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
            if (Kt.Framework.User.UserState.getIsNeedActived())
            {
                RouteValueDictionary routeData = new RouteValueDictionary(new { controller = "Account", Action = "Activateaccount", Area = "" });
                return RedirectToRoute(routeData);
            }
            if (!Kt.Framework.User.User.IS_LOGIN)
            {
                RouteValueDictionary routeData = new RouteValueDictionary(new { controller = "Account", Action = "logOn", Area = "" });
                return RedirectToRoute(routeData);
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
