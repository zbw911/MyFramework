using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    public class ViewThreadInfo
    {
        /// <summary>
        /// 列表
        /// </summary>
        public IEnumerable<Kt.GameGroup.Data.group_bbs> List { get; set; }

        /// <summary>
        /// 总个数
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
    }
}
