using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Model;
using Kt.GameGroup.Model.Enums;
namespace Kt.GameGroup.Model.ViewModel
{
    public class GroupMemberData
    {
        /// <summary>
        /// 用户属性
        /// </summary>
        public IEnumerable<ViewGroupMemberData> ViewGroupMemberData { get; set; }
        
        /// <summary>
        ///成员列表总数 
        /// </summary>
       public int count { get; set; }

       /// <summary>
       /// 页码编号
       /// </summary>
       public int PageNo { get; set; }


       /// <summary>
       /// 页面大小
       /// </summary>
       public int PageSize { get; set; }

       /// <summary>
       /// 当前登录用户类型
       /// </summary>
       public UserType UType { get; set; }

    }
}
