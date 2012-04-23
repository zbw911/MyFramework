using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.ShortMessage
{
    public class DBShortMessage : IShortMessage
    {
        public void DeList(ShortMessageModel ShortMessageModel)
        {
            return;
        }

        public void EnList(ShortMessageModel ShortMessageModel)
        {
            return;
        }

        public void FreshList(ShortMessageModel ShortMessageModel)
        {
            return;
        }

        public bool IsSendToMany(string Mobile)
        {
            return true;
        }

        public int GetSendTimes(string Mobile)
        {
            return 0;
        }
    }
}
