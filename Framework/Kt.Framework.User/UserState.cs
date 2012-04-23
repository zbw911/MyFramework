using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User
{
    /// <summary>
    /// 用户状态方法
    /// </summary>
    public class UserState
    {/**
	 * 是否已经完善安全资料
	 */
        public static string getIsSafeComplate()
        {
            return UserCookies.getAuthCookie(UserCookies.AUTHCOOKIE_NAME_ISSAFLECOMPLATE);
        }

        /**
         * 是否已经激活
         */
        public static bool getIsNeedActived()
        {
            return !string.IsNullOrEmpty(UserCookies.getActiveCookies(UserCookies.ACTIVECOOKIE_NAME_USERID));
        }

        /**
         * 是否已经登录UC
         */
        public static bool getIsUcLogin()
        {
            return UserCookies.getAuthCookie(UserCookies.AUTHCOOKIE_NAME_USERID) != null;
        }

        /**
         * 判断是否已经安全平台登录，这个XXX最重要的判断，与SESSION结合 
         */
        public static bool getIsWebLogin()
        {
            return User.IS_LOGIN;
        }

        public static void AllLoginOut()
        {

            //清除SESSION
            User.LoginOut();

            //清除cookies
            UserCookies.clearCookies(UserCookies.ACTIVECOOKIESNAME);
            UserCookies.clearCookies(UserCookies.AUTHCOOKIENAME);
        }
    }
}
