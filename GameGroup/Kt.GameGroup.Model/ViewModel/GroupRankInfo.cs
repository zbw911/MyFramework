using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    public class GroupRankInfo
    {
        /// <summary>
        /// 游戏团信息
        /// </summary>
        public Kt.GameGroup.Data.group_Infor Group_Infor { get; set; }

        /// <summary>
        /// 排名
        /// </summary>
        public int OrderId { get; set; }
    }
}
