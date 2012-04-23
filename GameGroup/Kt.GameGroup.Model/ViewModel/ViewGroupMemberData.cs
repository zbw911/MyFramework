using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Model.Enums;

namespace Kt.GameGroup.Model.ViewModel
{
    public class ViewGroupMemberData
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public decimal uid { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }


        /// <summary>
        /// 头像（小）
        /// </summary>
        public string face_Small { get; set; }

        /// <summary>
        /// 用户的类型
        /// </summary>
        public UserType UserType { get; set; }

        /// <summary>
        /// 用户在线状态
        /// </summary>
        public bool IsOnLine { get; set; }

        /// <summary>
        /// 用户间关系
        /// </summary>
        public int relation { get; set; }

        /// <summary>
        /// 加入游戏团时间
        /// </summary>
        public DateTime revTime { get; set; }

        /// <summary>
        /// 团名片-名称
        /// </summary>
        public string GroupUserName { get; set; }

        /// <summary>
        /// 团名片-头像
        /// </summary>
        public string GroupUserImg { get; set; }

        /// <summary>
        /// 游戏团成员级别名称
        /// </summary>
        public string UserGroupGradeName { get; set; }
    }
}
