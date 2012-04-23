using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.TransModel
{
    /// <summary>
    /// the add model
    /// </summary>
    public class CreateBbsSortModel
    {
        /// <summary>
        /// 团编号
        /// </summary>
        public int gId { get; set; }
        /// <summary>
        /// 显示次序
        /// </summary>
        public int showXH { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string sortName { get; set; }
    }
}
