using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Model.Enums;

namespace Kt.GameGroup.Model.ViewModel
{
    public class ViewGroupMember
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
        /// 等级
        /// </summary>
        public string GradeName { get; set; }

        /// <summary>
        /// 积分/团豆
        /// </summary>
        public int credits { get; set; }

        /// <summary>
        /// 经验百分比
        /// </summary>
        public string ExpPercentage { get; set; }

        /// <summary>
        /// 头像（大）
        /// </summary>
        public string face_Big { get; set; }

        /// <summary>
        /// 头像（中）
        /// </summary>
        public string face_Middle { get; set; }

        /// <summary>
        /// 头像（小）
        /// </summary>
        public string face_Small { get; set; }

        /// <summary>
        /// 用户的类型
        /// </summary>
        public UserType  UserType {get;set;}

        /// <summary>
        /// 团名片-名称
        /// </summary>
        public string GroupUserName { get; set; }

        /// <summary>
        /// 团名片-头像
        /// </summary>
        public string GroupUserImg {get;set; }

        /// <summary>
        /// 加入游戏团时间
        /// </summary>
        public DateTime revTime { get; set; }

        /// <summary>
        /// 游戏团成员级别名称
        /// </summary>
        public string UserGroupGradeName { get; set; }
    }
}
