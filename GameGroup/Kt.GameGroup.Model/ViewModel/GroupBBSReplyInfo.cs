using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    public class GroupBBSReplyInfo
    {
        /// <summary>
        /// 回复编号
        /// </summary>
        public int replyId { get; set; }

        /// <summary>
        /// 帖子编号
        /// </summary>
        public int bbsId { get; set; }

        /// <summary>
        /// 回复人编号
        /// </summary>
        public decimal uId { get; set; }

        /// <summary>
        /// 回复人昵称
        /// </summary>
        public string  nickName { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 回复时间
        /// </summary>
        public DateTime repTime { get; set; }
    }
}
