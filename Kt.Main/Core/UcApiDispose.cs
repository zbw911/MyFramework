using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
using Kt.Framework.Repository.Data;
using System.Web.SessionState;

namespace Kt.Main.Areas.GameWeiBo.Core
{
    public class UcApiDispose : DS.Web.UCenter.Api.UcApiBase, IRequiresSessionState
    {
        //private IRepository<Kt.GameWeiBo.Data.Members> MembersRepository;
        //private IRepository<Kt.GameWeiBo.Data.MemberValidate> _MemberValidate;
        public UcApiDispose()
        {
            //this.MembersRepository = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IRepository<Kt.GameWeiBo.Data.Members>>();
            //this._MemberValidate = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IRepository<Kt.GameWeiBo.Data.MemberValidate>>();
        }

        public override DS.Web.UCenter.Api.ApiReturn DeleteUser(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn GetCredit(decimal uid, int credit)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.UcCreditSettingReturns GetCreditSettings()
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.UcTagReturns GetTag(string tagName)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn RenameUser(decimal uid, string oldUserName, string newUserName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 同步登录逻辑
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public override DS.Web.UCenter.Api.ApiReturn SynLogin(decimal uid)
        {

            //var useraccount = this.MembersRepository.First(t => t.uid == uid);
            ////清除其它用户的状态，防止冲突
            //Kt.Framework.User.UserState.AllLoginOut();
            //if (useraccount == null)
            //{

            //    DS.Web.UCenter.Client.UcClient uc = new DS.Web.UCenter.Client.UcClient();

            //    var info = uc.UserInfo(uid);
            //    UserCookies.setActiveCookies(info.Uid, info.UserName, info.UserName, info.Mail);
            //}
            //else
            //{


            //    decimal userid = useraccount.uid;
            //    string logidId = useraccount.username;
            //    string hassafepass = useraccount.password;
            //    int expire = 0;// UserCookies.COOKIE_EXPIRETIME; 调用接口登录就使用浏览器进程
            //    var nickname = useraccount.nickname;
            //    var email = useraccount.email;

            //    //这里没有进行邮箱验证
            //    var act = _MemberValidate.First(t => t.email == useraccount.email);
            //    var registWay = useraccount.extcredits4;

            //    if ((registWay == null || registWay == 0) && ((act == null) || (act != null && act.status == 0)))
            //    {
            //        UserCookies.setActiveCookies(userid, logidId, logidId, email);
            //    }
            //    else
            //    {
            //        UserCookies.setAuthCookies(userid, logidId, email, expire, nickname);
            //        Kt.Framework.User.User.Set_MEMBER_ID(useraccount.uid);
            //        // Kt.Framework.User.User.NICKNAME = useraccount.username;
            //    }
            //}

            return DS.Web.UCenter.Api.ApiReturn.Success;
        }



        /// <summary>
        /// 同步退出逻辑
        /// </summary>
        /// <returns></returns>
        public override DS.Web.UCenter.Api.ApiReturn SynLogout()
        {
            //UserState.AllLoginOut();
            return DS.Web.UCenter.Api.ApiReturn.Success;
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateApps(DS.Web.UCenter.UcApps apps)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateBadWords(DS.Web.UCenter.UcBadWords badWords)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateClient(DS.Web.UCenter.UcClientSetting client)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateCredit(decimal uid, int credit, int amount)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateCreditSettings(DS.Web.UCenter.UcCreditSettings creditSettings)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdateHosts(DS.Web.UCenter.UcHosts hosts)
        {
            throw new NotImplementedException();
        }

        public override DS.Web.UCenter.Api.ApiReturn UpdatePw(string userName, string passWord)
        {
            throw new NotImplementedException();
        }
    }
}
