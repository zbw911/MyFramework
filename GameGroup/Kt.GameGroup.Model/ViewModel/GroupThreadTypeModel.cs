using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kt.GameGroup.Data;

namespace Kt.GameGroup.Model.ViewModel
{
    /// <summary>
    /// 分类显示模型 
    /// </summary>
    public class GroupThreadTypeModel
    {
        /// <summary>
        /// 是否启用分类
        /// </summary>
        public int jointype { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public int prefix { get; set; }
        /// <summary>
        /// 必须使用分类
        /// </summary>
        public int required { get; set; }

        /// <summary>
        /// 分类数据列表
        /// </summary>
        public IEnumerable<group_bbsSort> sortList { get; set; }
    }



}
