using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    public class GroupMemberInfo
    {
        /// <summary>
        /// 团长udi
        /// </summary>
        public decimal? Uid { get; set; }

        /// <summary>
        /// 团长昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 团长头像（大）
        /// </summary>
        public string face_Big { get; set; }

        /// <summary>
        /// 团长头像（中）
        /// </summary>
        public string face_Middle { get; set; }

        /// <summary>
        /// 头像（小）
        /// </summary>
        public string face_Small { get; set; }

        /// <summary>
        /// 关系（1-我，2-好友,3-陌生人,4-黑名单中的人）
        /// </summary>
        public int relation { get; set; }

        public string GroupUserName { get; set; }

        /// <summary>
        /// 副团长
        /// </summary>
        public Dictionary<decimal, ViewGroupMember> DeputyGroupMembers { get; set; }

        /// <summary>
        /// 最新成员
        /// </summary>
        public Dictionary<decimal, ViewGroupMember> NewGroupMembers { get; set; }

        /// <summary>
        /// 活跃成员
        /// </summary>
        public Dictionary<decimal, ViewGroupMember> ActiveGroupMembers { get; set; }
    }
}
