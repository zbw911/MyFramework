using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Data;

namespace Kt.GameGroup.Model.ViewModel
{
    public class ViewThreadModel
    {

        public group_bbs BBSInfo { get; set; }
        /// <summary>
        /// 回复列表
        /// </summary>
        public IEnumerable<Kt.GameGroup.Data.group_bbsReply> ReplyList { get; set; }

        /// <summary>
        /// 总个数
        /// </summary>
        public int count { get; set; }

        /// <summary>
        /// 页码编号
        /// </summary>
        public int PageNo { get; set; }


        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
