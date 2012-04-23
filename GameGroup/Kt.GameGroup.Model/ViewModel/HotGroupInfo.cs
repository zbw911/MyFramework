using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    /// <summary>
    /// 热门群组分类
    /// </summary>
    public class HotGameGroupInfo
    {
        /// <summary>
        /// 游戏名
        /// </summary>
        public Dictionary<int, string> GameList { get; set; }

        /// <summary>
        /// 群组数量
        /// </summary>
        public Dictionary<int, int?> GroupCount { get; set; }

        /// <summary>
        /// 群组名
        /// </summary>
        public Dictionary<int, IEnumerable<Kt.GameGroup.Data.group_Infor>> GroupList { get; set; }

    }
        /// <summary>
        /// 游戏的ID
        /// </summary>
        //public int GameId { get; set; }
        ///// <summary>
        ///// 游戏名
        ///// </summary>
        //public string GameName { get; set; }

        ///// <summary>
        ///// 游戏图片
        ///// </summary>
        //public string gamePic { get; set; }

        ///// <summary>
        ///// 此游戏下的组个数
        ///// </summary>
        //public int groupCount { get; set; }

        ///// <summary>
        /////  热门游戏下的热门群组 列表
        ///// </summary>
        //public IEnumerable<Kt.GameGroup.Data.group_Infor> Group_Infor { get; set; }
        //public IList<HotGroup> SubGroupList { get; set; }
   // }

    /// <summary>
    /// 热门游戏下的热门群组
    /// </summary>
    public class HotGroup
    {
        /// <summary>
        /// 组名
        /// </summary>
        public string groupname { get; set; }
        /// <summary>
        /// 组编号
        /// </summary>
        public string groupId { get; set; }
    }
}
