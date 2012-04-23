using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.Forbidden
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// <![CDATA[
    /// var forbidden = ForbiddenFactory.FactoryMethod();
    /// forbidden.is.....
    /// forbidden.En....
    /// forbidden.De....
    /// ]]>
    /// </example>
    public class ForbiddenFactory
    {
        public static IForbidden FactoryMethod()
        {
            return new MemForbidden();
        }
    }
}
