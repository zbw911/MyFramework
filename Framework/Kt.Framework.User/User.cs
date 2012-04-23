using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kt.Framework.User
{
    public class User
    {

        /// <summary>
        /// 退出
        /// </summary>
        internal static void LoginOut()
        {
            var oline = new UserOnline();

            oline.RemoveUser(MEMBER_ID);

            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }



        public static void Set_MEMBER_ID(decimal UID)
        {
            var oline = new UserOnline();

            oline.AddUser(new UserInfo { Uid = UID, LASTActive = System.DateTime.Now });
            //HttpContext.Current.Session[SessionKeys.UserID] = UID;
        }
        /// <summary>
        /// 用户编号 UID
        /// </summary>
        public static decimal MEMBER_ID
        {
            get
            {
                return GetUserIdFromCookie();
                // return 147484075;
                //if (HttpContext.Current.Session[SessionKeys.UserID] == null || HttpContext.Current.Session[SessionKeys.UserID].ToString() == "")
                //{
                //    if (0 != GetUserIdFromCookie())
                //    {

                //    }

                //    return 0;
                //}
                //else
                //{
                //    decimal uid;
                //    decimal.TryParse(HttpContext.Current.Session[SessionKeys.UserID].ToString(), out uid);
                //    return uid;
                //}

            }

        }

        /// <summary>
        /// 当前登录用户昵称
        /// </summary>
        public static string NICKNAME
        {
            get
            {
                return UserCookies.getAuthCookie(UserCookies.AUTHCOOKIE_NAME_NICKNAME);
            }
        }


        private static decimal GetUserIdFromCookie()
        {
            decimal uid;

            if (UserState.getIsNeedActived())
            {
                var strUid = UserCookies.getActiveCookies(UserCookies.ACTIVECOOKIE_NAME_USERID);
                decimal.TryParse(strUid, out uid);
                return uid;
            }

            var authuid = UserCookies.getAuthCookie(UserCookies.AUTHCOOKIE_NAME_USERID);
            decimal.TryParse(authuid, out uid);
            return uid;
        }

        /// <summary>
        /// 当前用户登录名
        /// </summary>
        public static string MEMBER_NAME
        {
            get
            {
                //if (HttpContext.Current.Session[SessionKeys.UserName] == null || HttpContext.Current.Session[SessionKeys.UserName].ToString() == "")
                //{
                //    return "";
                //}
                //else
                //{
                //    return HttpContext.Current.Session[SessionKeys.UserName].ToString();
                //}

                if (UserState.getIsNeedActived())
                {
                    return UserCookies.getActiveCookies(UserCookies.ACTIVECOOKIE_NAME_USERNAME);
                }
                else
                {
                    return UserCookies.getAuthCookie(UserCookies.AUTHCOOKIE_NAME_USERNAME);
                }
            }
            //set
            //{
            //    HttpContext.Current.Session[SessionKeys.UserName] = value;
            //}
        }

        /// <summary>
        /// 最后一次活动
        /// </summary>
        public static DateTime LASTActive
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.LASTActive] == null || HttpContext.Current.Session[SessionKeys.LASTActive].ToString() == "")
                {
                    return System.DateTime.MinValue;
                }
                else
                {
                    return DateTime.Parse(HttpContext.Current.Session[SessionKeys.LASTActive].ToString());
                }
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.LASTActive] = value;
            }
        }

        /// <summary>
        /// 判断是否登录,如果未激话，状态为未登录
        /// </summary>
        public static bool IS_LOGIN
        {
            get
            {
                if (User.MEMBER_ID > 0)
                {
                    var oline = new UserOnline();
                    //oline.FreshUser(User.MEMBER_ID);
                }


                return !Kt.Framework.User.UserState.getIsNeedActived() && User.MEMBER_ID > 0;
            }
        }

        /// <summary>
        /// 未登录用户
        /// </summary>
        public static bool IS_GUEST
        {
            get
            {
                return User.MEMBER_ID == 0;
            }
        }

        public static DateTime LASTPOST
        {
            get
            {
                if (HttpContext.Current.Session[SessionKeys.LASTPOST] == null || HttpContext.Current.Session[SessionKeys.LASTPOST].ToString() == "")
                {
                    return System.DateTime.MinValue;
                }
                else
                {
                    return DateTime.Parse(HttpContext.Current.Session[SessionKeys.LASTPOST].ToString());
                }
            }
            set
            {
                HttpContext.Current.Session[SessionKeys.LASTPOST] = value;
            }
        }





        public static string MEMBER_ROLE_TYPE { get; set; }
    }


    internal class SessionKeys
    {
        public static readonly string IPAddress = "IPAddress";
        public static readonly string UserID = "UserID";
        public static readonly string UserName = "UserName";
        public static readonly string LastActivityUpdate = "LastActivityUpdate";
        public static readonly string CaptchaSessionPrefix = "mb_captcha_";
        public static readonly string LASTPOST = "LASTPOST";

        public static readonly string LASTActive = "LASTActive";
    }
}
