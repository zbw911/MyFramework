using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kt.Framework.Common
{
    public static class GlobalExtensions
    {
        /// <summary>
        /// 截断字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="toLength"></param>
        /// <param name="cutOffReplacement"></param>
        /// <returns></returns>
        public static string Shorten(this string str, int toLength, string cutOffReplacement = " ...")
        {
            if (string.IsNullOrEmpty(str) || str.Length <= toLength)
                return str;
            else
                return str.Remove(toLength) + cutOffReplacement;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IEnumerable<T> TakePage<T>(this IEnumerable<T> items, int page, int pageSize = 10) where T : class
        {
            return items.Skip(pageSize * (page - 1)).Take(pageSize);
        }

        public static IHtmlString ToHtmlString(this string value)
        {
            return new HtmlString(value);
        }

        public static string SeperateWords(this string str)
        {
            if(string.IsNullOrWhiteSpace(str))
                return str;

            string output = "";
            char[] chars = str.ToCharArray();

            for(int i = 0; i < chars.Length; i++)
            {
                if (i == chars.Length - 1 || i == 0 || char.IsWhiteSpace(chars[i]))
                {
                    output += chars[i];
                    continue;
                }

                if (char.IsUpper(chars[i]) && char.IsLower(chars[i - 1]))
                    output += " " + chars[i];
                else
                    output += chars[i];
            }

            return output;
        }
    }
}
