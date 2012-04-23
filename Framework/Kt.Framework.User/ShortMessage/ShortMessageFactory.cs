using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.ShortMessage
{
    public class ShortMessageFactory
    {
        public static IShortMessage FactoryMethod()
        {
            return new MemShortMessage();
        }
    }
}
