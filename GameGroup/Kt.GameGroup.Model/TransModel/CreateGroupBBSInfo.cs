using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.TransModel
{
    public class CreateGroupBBSInfo
    {
        private int _publishid = 1;

        public int gid { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public int publishid { get { return this._publishid; } set { this._publishid = value; } }

        public int sortid { get; set; }
    }
}
