using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.GetPassWordEmail
{
    public class GPEmialFactory
    {
        public static IGPEmail FactoryMethod()
        {
            return new MemGPEmail();
        }
    }
}
