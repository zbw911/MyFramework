using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.Models
{
    public class Messager
    {
        public List<string> MessageList { get; set; }

        public string redirectto { get; set; }

        public string to_title { get; set; }

        public int time { get; set; }

        public string return_msg { get; set; }

        public string tips { get; set; }
    }
}
