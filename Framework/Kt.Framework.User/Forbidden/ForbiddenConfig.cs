using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.User.Forbidden
{
    /// <summary>
    /// 配置项
    /// </summary>
    public static class ForbiddenConfig
    {
        /// <summary>
        /// 最大错误次数
        /// </summary>
        public const int MAXERROR = 5;

        /// <summary>
        /// 在禁用列表中保存时间
        /// </summary>
        public const double KEEPTIME = 15;

        /// <summary>
        /// 最小累计时间间隔
        /// </summary>
        public const double TIMEDENSITY = 10;
    }
}
