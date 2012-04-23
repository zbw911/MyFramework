using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Model.Enums;

namespace Kt.GameGroup.Model.TransModel
{
    public class NavBarModel
    {
        public NavType NavType { get; set; }

        /// <summary>
        /// 用户类型， 团长， 副团，
        /// </summary>
        public UserType UserType { get; set; }
    }
}
