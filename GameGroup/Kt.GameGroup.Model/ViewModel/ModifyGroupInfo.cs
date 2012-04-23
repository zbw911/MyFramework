using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Data;
namespace Kt.GameGroup.Model.ViewModel
{
    /// <summary>
    /// 修改时的组信息
    /// </summary>
    public class ModifyGroupInfo
    {
        //public group_Infor groupInfo{get;set;}

        public int gId { get; set;}
        public string gName { get; set; }

        public string account { get; set; }
        public string notice { get; set; }
        public string gPic { get; set; }

        public int gameId { get; set; }
        public int platFormId { get; set; }
        public int gameserverid { get; set; }

        public int joinPerm { get; set; }
        public bool viewPerm { get; set; }
    }
}
