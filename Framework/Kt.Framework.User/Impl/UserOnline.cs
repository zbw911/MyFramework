using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User
{
    public class UserOnline : Kt.Framework.User.IUserOnline
    {

        private static List<UserInfo> _onlineUser = new List<UserInfo>();
        private static object lockobj = new object();

        /// <summary>
        /// 更新在线状态
        /// </summary>
        /// <param name="uid"></param>
        internal void FreshUser(decimal uid)
        {
            var online = OnlineUser.FirstOrDefault(x => x.Uid == uid);

            if (online == null) { this.AddUser(new UserInfo { Uid = uid, LASTActive = System.DateTime.Now }); return; };

            online.LASTActive = System.DateTime.Now;
        }

        internal IEnumerable<UserInfo> OnlineUser
        {
            get
            {
                lock (lockobj)
                {
                    return _onlineUser;
                }
            }
        }

        /// <summary>
        /// 加入一个用户进入在线列表
        /// </summary>
        /// <param name="userinfo"></param>
        public void AddUser(UserInfo userinfo)
        {
            if (!(_onlineUser.Where(p=>p.Uid==userinfo.Uid).Count()>0))
            {
                _onlineUser.Add(userinfo);
                this.SortUser();
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private void SortUser()
        {
            if (OnlineUser.Count() == 0)
                return;
            _onlineUser = OnlineUser.OrderByDescending(x => x.LASTActive).ToList();
        }

        /// <summary>
        /// 取得一个人的在线状态
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool IsOnLine(decimal uid)
        {
            if (OnlineUser.Count() == 0)
                return false;

            var online = OnlineUser.FirstOrDefault(x => x.Uid == uid);

            if (online == null) return false;

            //if (online.Uid != uid) return false;

            //假设最后一次是活动在20分钟内
            if (online.LASTActive < System.DateTime.Now.AddMinutes(-20))
            {
                this.RemoveUser(uid);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 返回当前在线的用户列表
        /// </summary>
        /// <param name="pagesize"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IEnumerable<UserInfo> GetOnlineList(int pagesize = 12, int page = 1)
        {
            if (OnlineUser.Count() == 0)
                return null;
            return OnlineUser.Skip(pagesize * (page - 1)).Take(pagesize);
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="uid"></param>
        public void RemoveUser(decimal uid)
        {
            if (OnlineUser.Count() == 0)
                return;
            if (OnlineUser == null) return;

            var user = OnlineUser.FirstOrDefault(x => x.Uid == uid);
            if (user != null)
            {
                _onlineUser.Remove(user);
            }
        }
    }
}
