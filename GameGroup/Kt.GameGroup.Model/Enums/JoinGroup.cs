using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.Enums
{
    public enum JoinGroup
    {
        禁止加入,
        审核中,
        已加入,
        未登录,
        已退出
    }

    public enum GroupNum
    { 
        超过人数上线,
        操作成功,
        操作失败,
        该团已不存在

    }
}
