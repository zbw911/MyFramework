using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.TransModel
{
    /// <summary>
    /// 搜索条件
    /// </summary>
    public class SearchConditionModel
    {
        public int gameid { get; set; }             //游戏ID
        public int terraceid {get; set;}            //平台ID
        public int serverid {get; set;}             //服务器ID

        public string searchname { get; set; }      //游戏团名字

        /// <summary>
        /// 1、人数；2、时间；其他、积分
        /// </summary>
        public int pxtype { get; set; }
    }
}
