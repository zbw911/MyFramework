using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kt.Framework.Common
{
    public class RegexHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        public static IList<KeyValuePair<int, string>> MatchesKeyIndex(string content, string pattern)
        {
            MatchCollection mc = Matches(content, pattern);
            IList<KeyValuePair<int, string>> kvs = new List<KeyValuePair<int, string>>();
            for (int i = 0; i < mc.Count; i++) //在输入字符串中找到所有匹配
            {
                var v = mc[i].Value; //将匹配的字符串添在字符串数组中
                var k = mc[i].Index; //记录匹配字符的位置

                kvs.Add(new KeyValuePair<int, string>(k, v));
            }
            return kvs;
        }

        public static MatchCollection Matches(string content, string pattern)
        {
            Regex r = new Regex(pattern);
            MatchCollection mc = r.Matches(content);
            return mc;
        }

        /// <summary>
        /// 正则替换
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Replace(string content, string pattern, string replacement)
        {
            Regex r = new Regex(pattern);
            return r.Replace(content, replacement);
        }

        public static string MatchesString(string content, string pattern, int matchindex, int groupindex)
        {

            var g = MatchesGroups(content, pattern, matchindex);
            if (g == null || groupindex >= g.Count)
            {
                return null;
            }

            return g[groupindex].Value;
        }

        /// <summary>
        /// 返回 所有 分组匹配项
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <param name="matchindex"></param>
        /// <returns></returns>
        public static string[] MatchesGroupsString(string content, string pattern, int GroupIndex)
        {
            var mc = Matches(content, pattern);
            if (mc == null)
            {
                return null;
            }
            string[] vals = new string[mc.Count];
            for (int i = 0; i < mc.Count; i++)// m in mc)
            {
                if (mc[i].Groups.Count > GroupIndex)
                {
                    var v = mc[i].Groups[GroupIndex].Value;
                    vals[i] = v;
                }
                else
                {
                    throw new Exception("组编号大于现在长度  mc[" + i + "].Groups.Count > " + GroupIndex);
                }
            }

            //var g = MatchesGroups(content, pattern, matchindex);
            //if (g == null || 0 >= g.Count)
            //{
            //    return null;
            //}
            //string[] vals = new string[g.Count];
            //for (int i = 0; i < g.Count; i++)
            //{
            //    vals[i] = g[i].Value;
            //}
            return vals;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <param name="matchindex"></param>
        /// <returns></returns>
        public static GroupCollection MatchesGroups(string content, string pattern, int matchindex)
        {
            var mc = Matches(content, pattern);

            if (mc == null || mc.Count <= matchindex)
            {
                return null;
            }

            var m = mc[matchindex];

            if (m == null)
                return null;

            var g = m.Groups;

            return g;
        }


        public static string Match(string content, string pattern)
        {
            Regex r = new Regex(pattern, RegexOptions.ExplicitCapture);
            Match mc = r.Match(content);
            if (mc.Success)
            {
                return mc.Value;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 是否MATCH，兼容php
        /// </summary>
        /// <param name="content"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool Preg_match(string pattern, string content)
        {
            try
            {
                Regex r = new Regex(pattern);
                Match mc = r.Match(content);
                return mc.Success;
            }
            catch (Exception e)
            {
                throw new Exception("", e);
            }
        }



        public static MatchCollection Preg_match_all(string content, string pattern, out MatchCollection mc)
        {
            Regex r = new Regex(pattern);
            mc = r.Matches(content);
            return mc;
        }

        public static MatchCollection Preg_match_all(string content, string pattern)
        {
            MatchCollection mc;
            return Preg_match_all(content, pattern, out mc);
        }
    }
}
