using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    public class GameInfoModel
    {
        /// <summary>
        /// 游戏的ID
        /// </summary>
        public int GameId { get; set; }
        /// <summary>
        /// 游戏名
        /// </summary>
        public string GameName { get; set; }

        /// <summary>
        /// 游戏图片
        /// </summary>
        public string gamePic { get; set; }
    }
}
