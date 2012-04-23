using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kt.Framework.Tool
{
    /// <summary>
    /// 这是一个简单的代码分析类，会生成一个报告 
    /// added by zbw911 
    /// 
    /// example: 
    /// 
    ///    var t = new CodeRunTime("没什么说的");
    ///    var report = t.End();
    ///    
    /// </summary>
    public class CodeRunTime
    {

        DateTime start;
        DateTime end;
        private string discript;
        private double timelong;

        private string reporttmp = "{0},{3}" + System.Environment.NewLine;

        /// <summary>
        /// 设置输入模板
        /// </summary>
        public string Reporttmp
        {
            set { reporttmp = value; }
        }
        /// <summary>
        /// 构造并生成
        /// </summary>
        /// <param name="discript"></param>
        public CodeRunTime(string discript)
        {
            this.discript = discript;
            start = System.DateTime.Now;
        }

        /// <summary>
        /// 完成
        /// </summary>
        /// <returns></returns>
        public string End()
        {
            end = System.DateTime.Now;

            timelong = (this.end - this.start).TotalMilliseconds;

            return this.format();
        }

        private string format()
        {
            return string.Format(this.reporttmp, this.discript, this.start, this.end, this.timelong);
        }
    }
}
