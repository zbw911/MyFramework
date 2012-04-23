using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.TransModel
{
    /// <summary>
    /// 创建群组的模型
    /// </summary>
    public class CreateGroupModel
    {
        /// <summary>
        /// 团编号
        /// </summary>
        public int gId { get; set; }

        /// <summary>
        /// 所属用户
        /// </summary>
        public decimal uId { get; set; }

        /// <summary>
        /// 游戏团名称
        /// </summary>
        public string gName { get; set; }

        /// <summary>
        /// 团人数
        /// </summary>
        public int memberNum { get; set; }

        /// <summary>
        /// 总发帖数量
        /// </summary>
        public int postNum { get; set; }

        /// <summary>
        /// 游戏团积分
        /// </summary>
        public int points { get; set; }

        /// <summary>
        /// 游戏团简介
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 团公告
        /// </summary>
        public string notice { get; set; }

        /// <summary>
        /// 团Logo图片
        /// </summary>
        public string gPic { get; set; }

        /// <summary>
        /// 游戏编号
        /// </summary>
        public int gameId { get; set; }

        /// <summary>
        /// 运营商编号
        /// </summary>
        public int platFormId { get; set; }

        /// <summary>
        /// 区服编号
        /// </summary>
        public int gameserverid { get; set; }

        /// <summary>
        /// 访问权限: 0所有人，1仅会员：默认0
        /// </summary>
        public bool viewPerm { get; set; }

        /// <summary>
        /// 加入方式: 0关闭，1自由加入，2审核加入：默认1
        /// </summary>
        public int joinPerm { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime createDate { get; set; }

        /// <summary>
        /// 访问地址
        /// </summary>
        public string vistUrl { get; set; }
    }
}
