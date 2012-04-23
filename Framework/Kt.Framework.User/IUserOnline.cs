using System;
namespace Kt.Framework.User
{
    /// <summary>
    /// 用户在线接口
    /// </summary>
    public interface IUserOnline
    {
        void AddUser(UserInfo userinfo);
        System.Collections.Generic.IEnumerable<UserInfo> GetOnlineList(int pagesize = 12, int page = 1);
        bool IsOnLine(decimal uid);
        void RemoveUser(decimal uid);
    }
}
  