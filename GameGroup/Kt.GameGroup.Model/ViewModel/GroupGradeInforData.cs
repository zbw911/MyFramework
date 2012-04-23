using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.GameGroup.Model.ViewModel
{
    /// <summary>
    /// 游戏团级别信息
    /// </summary>
    public class GroupGradeInforData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int AutoId { get; set; }
        /// <summary>
        /// 团号
        /// </summary>
        public int Gid { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int GradeId { get; set; }
        /// <summary>
        /// 级别名称
        /// </summary>
        public string  GradeName { get; set; }
        /// <summary>
        /// 人数
        /// </summary>
        public int GradeNum { get; set; }
    }
}
