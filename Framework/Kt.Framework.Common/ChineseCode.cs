using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Kt.Framework.Common
{
    /// <summary>
    /// 中文方法
    /// </summary>
    public class ChineseCode
    {
        /// <summary>
        /// 是否是中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsChineseLetter(char input)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);

            int bdfrom = 65281; //   中文符号
            int bdend = 65374;
            if (input != char.MinValue)
            {
                code = input; //获得字符串input中指定索引index处字符unicode编码

                if ((code >= chfrom && code <= chend) || (code > bdfrom && code < bdend))
                {
                    return true;     //当code在中文范围内返回true

                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是中文字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool IsChineseLetter(string input, int index)
        {
            int code = 0;
            int chfrom = Convert.ToInt32("4e00", 16);    //范围（0x4e00～0x9fff）转换成int（chfrom～chend）
            int chend = Convert.ToInt32("9fff", 16);
            if (input != "")
            {
                code = Char.ConvertToUtf32(input, index);    //获得字符串input中指定索引index处字符unicode编码

                if (code >= chfrom && code <= chend)
                {
                    return true;     //当code在中文范围内返回true

                }
                else
                {
                    return false;    //当code不在中文范围内返回false
                }
            }
            return false;
        }

        //方法二：
        //http://hi.baidu.com/yhfd/blog/item/3222e1fca22cfb80b901a027.html

        /// <summary>
        /// 
        /// </summary>
        /// <param name="CString"></param>
        /// <returns></returns>
        public bool IsChina(string CString)
        {
            bool BoolValue = false;
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) < Convert.ToInt32(Convert.ToChar(128)))
                {
                    BoolValue = false;
                }
                else
                {
                    return BoolValue = true;
                }
            }
            return BoolValue;
        }

       

        /// <summary>
        /// 判断句子中是否含有中文     宁夏大学 张冬 zd4004.blog.163.com
        /// </summary>
        /// <param >字符串</param> 
        public bool WordsIScn(string words)
        {
            string TmmP;

            for (int i = 0; i < words.Length; i++)
            {
                TmmP = words.Substring(i, 1);

                byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);

                if (sarr.Length == 2)
                {
                    return true;
                }
            }
            return false;
        }



        //方法四：
        //for (int i=0; i<s.length; i++)
        //{
        //Regex rx = new Regex("^[\u4e00-\u9fa5]$");
        //if (rx.IsMatch(s[i]))
        //// 是
        //else
        //// 否
        //}
        //正解！
        //\u4e00-\u9fa5 汉字的范围。
        //^[\u4e00-\u9fa5]$ 汉字的范围的正则

        //方法五
        //unicodeencoding   unicodeencoding   =   new   unicodeencoding();   
        //  byte   []   unicodebytearray   =   unicodeencoding.getbytes(   inputstring   );   
        //  for(   int   i   =   0;   i   <   unicodebytearray.length;   i++   )   
        //  {   
        //  i++;   
        //  //如果是中文字符那么高位不为0   
        //  if   (   unicodebytearray[i]   !=   0   )   
        //  {   
        //  }   


        /// <summary>
        /// 给定一个字符串，判断其是否只包含有汉字
        /// </summary>
        /// <param name="testStr"></param>
        /// <returns></returns>
        public bool IsOnlyContainsChinese(string testStr)
        {
            char[] words = testStr.ToCharArray();
            foreach (char word in words)
            {
                if (IsGBCode(word.ToString()) || IsGBKCode(word.ToString()))  // it is a GB2312 or GBK chinese word
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断一个word是否为GB2312编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsGBCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(word);
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code or other code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 176 && byte1 <= 247 && byte2 >= 160 && byte2 <= 254)    //判断是否是GB2312
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 判断一个word是否为GBK编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsGBKCode(string word)
        {
            byte[] bytes = Encoding.GetEncoding("GBK").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if (byte1 >= 129 && byte1 <= 254 && byte2 >= 64 && byte2 <= 254)     //判断是否是GBK编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }



        /// <summary>
        /// 判断一个word是否为Big5编码的汉字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private bool IsBig5Code(string word)
        {
            byte[] bytes = Encoding.GetEncoding("Big5").GetBytes(word.ToString());
            if (bytes.Length <= 1)  // if there is only one byte, it is ASCII code
            {
                return false;
            }
            else
            {
                byte byte1 = bytes[0];
                byte byte2 = bytes[1];
                if ((byte1 >= 129 && byte1 <= 254) && ((byte2 >= 64 && byte2 <= 126) || (byte2 >= 161 && byte2 <= 254)))     //判断是否是Big5编码
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}
