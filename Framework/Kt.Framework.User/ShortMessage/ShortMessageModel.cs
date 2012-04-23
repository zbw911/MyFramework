using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.ShortMessage
{
    public class ShortMessageModel
    {
        public string Mobile { get; set; }

        public DateTime LastTime { get; set; }

        public int SendTimes { get; set; }
    }
}
