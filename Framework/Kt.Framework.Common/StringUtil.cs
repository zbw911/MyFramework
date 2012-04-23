using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Text.RegularExpressions;

namespace Kt.Framework.Common
{
    public class StringUtil
    {
        #region 对字符串进行有效的HTML转换
        /// <summary>
        /// 对字符串进行有效的HTML转换
        /// </summary>
        /// <param name="inputString">字符串</param>
        /// <param name="maxLength">最多长度</param>
        /// <returns></returns>
        public static string InputText(string inputString, int maxLength)
        {
            // 定义一个可变字符字符串
            StringBuilder retVal = new StringBuilder();
            // 检测字符串是否为空
            if ((inputString != null) && (inputString != String.Empty))
            {   // 去掉空格
                inputString = inputString.Trim();

                // 取最大长度，多余的截取掉
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                // 把字符转换为HTML字符
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                retVal.Replace("'", " ");
            }

            return retVal.ToString();
        }
        #endregion


        public static string GetDoMain(string url, string key)
        {
            if (string.IsNullOrEmpty(url))
            {
                return "";
            }

            if (string.IsNullOrEmpty(key))
            {
                return url;
            }

            int pos = url.IndexOf(key);

            if (pos < 0)
            {
                return url;
            }
            else
            {
                string rtn = url.Substring(0, pos - 1);
                return rtn;
            }
        }

        public static string inputHtml(string inputString)
        {
            string str = inputString;
            //将此实例中的指定 Unicode 字符的所有匹配项替换为其他指定的 Unicode 字符。
            str = str.Replace("\r", "<br>");
            str = str.Replace(" ", "&nbsp;");
            return str;
        }

        public static string HtmlInputTex(string inputHtml)
        {
            string str = inputHtml;
            str = str.Replace("<br>", "\r");
            str = str.Replace("&nbsp;", " ");
            return str;
        }

        public static string GetPlainText(string inputText, int outNum)
        {
            try
            {
                string tempStr = inputText;
                int num1 = StringUtil.GetStringCount(inputText, "&nbsp;");
                num1 = num1 * 6;
                int num2 = StringUtil.GetStringCount(inputText, "<br>");
                num2 = num2 * 4;
                int numCount = inputText.Length - num1 - num2;

                int forNum = outNum;
                if (outNum > inputText.Length)
                    forNum = inputText.Length;
                if (numCount > forNum)
                {
                    string tempDescr = "";
                    for (int i = 0; i < forNum; i++)
                    {
                        inputText = tempStr.Substring(0, i + 1);
                        tempDescr = inputText.Substring(i, 1);
                        if (tempDescr == "&" || tempDescr == "n" || tempDescr == "b" || tempDescr == "s" || tempDescr == "p"
                            || tempDescr == ";" || tempDescr == "<" || tempDescr == "b" || tempDescr == "r" || tempDescr == ">")
                        {
                            forNum = forNum + 1;
                        }
                    }
                }
                return inputText + "...";
            }
            catch
            {
                return inputText = inputText + "...";// "格式问题，无法显示";
            }
        }

        #region 字符是否小写
        /// <summary>
        /// 字符是否小写
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>bool</returns>
        public static bool isLower(char ch)
        {
            if (ch >= 'a' && ch <= 'z')
                return true;
            else
                return false;
        }
        #endregion

        #region 字符是否大写
        /// <summary>
        /// 字符是否大写
        /// </summary>
        /// <param name="ch">字符</param>
        /// <returns>bool</returns>
        public static bool isUpper(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;
        }
        #endregion

        #region 输入的字符是否是数字
        /// <summary>
        /// 输入的字符是否是数字
        /// </summary>
        /// <param name="ch">一个字符</param>
        /// <returns>bool</returns>
        public static bool isNumberic(char ch)
        {
            if (ch >= '0' && ch <= '9')
                return true;
            else
                return false;
        }
        #endregion

        #region 特殊字符检验
        /// <summary>
        /// 特殊字符检验
        /// </summary>
        /// <param name="ch">特殊字符</param>
        /// <returns></returns>
        public static bool isSpecialCharacter(char ch)
        {
            if (ch == '!')
                return true;
            else if (ch == '@')
                return true;
            else if (ch == '#')
                return true;
            else if (ch == '$')
                return true;
            else if (ch == '^')
                return true;
            else if (ch == '&')
                return true;
            else if (ch == '*')
                return true;
            else if (ch == '?')
                return true;
            else if (ch == '/')
                return true;
            else if (ch == '\\')
                return true;
            else
                return false;
        }
        #endregion

        #region 从字符串中的尾部删除指定的字符串
        /// <summary>
        /// 从字符串中的尾部删除指定的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="removedString"></param>
        /// <returns></returns>
        public static string Remove(string sourceString, string removedString)
        {
            try
            {
                if (sourceString.IndexOf(removedString) < 0)  //判断删除的字符串是否存在
                    throw new Exception("原字符串中不包含移除字符串！");
                string result = sourceString;
                int lengthOfSourceString = sourceString.Length;
                int lengthOfRemovedString = removedString.Length;
                int startIndex = lengthOfSourceString - lengthOfRemovedString;
                string tempSubString = sourceString.Substring(startIndex);
                if (tempSubString.ToUpper() == removedString.ToUpper())
                {
                    result = sourceString.Remove(startIndex, lengthOfRemovedString);
                }
                return result;
            }
            catch
            {
                return sourceString;
            }
        }
        #endregion

        #region 获取拆分符右边的字符串
        /// <summary>
        /// 获取拆分符右边的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string RightSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[tempString.Length - 1].ToString();
            }
            return result;
        }
        #endregion

        #region 获取拆分符左边的字符串
        /// <summary>
        /// 获取拆分符左边的字符串
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="splitChar"></param>
        /// <returns></returns>
        public static string LeftSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[0].ToString();
            }
            return result;
        }
        #endregion

        #region 去掉最后一个逗号
        /// <summary>
        /// 去掉最后一个逗号
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        public static string DelLastComma(string origin)
        {
            if (origin.IndexOf(",") == -1)
            {
                return origin;
            }
            return origin.Substring(0, origin.LastIndexOf(","));
        }
        #endregion

        #region 删除不可见字符
        /// <summary>
        /// 删除不可见字符
        /// </summary>
        /// <param name="sourceString"></param>
        /// <returns></returns>
        public static string DeleteUnVisibleChar(string sourceString)
        {
            System.Text.StringBuilder sBuilder = new System.Text.StringBuilder(131);
            for (int i = 0; i < sourceString.Length; i++)
            {
                int Unicode = sourceString[i];
                if (Unicode >= 16)
                {
                    sBuilder.Append(sourceString[i].ToString());
                }
            }
            return sBuilder.ToString();
        }
        #endregion

        #region 获取数组元素的合并字符串
        /// <summary>
        /// 获取数组元素的合并字符串
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string GetArrayString(string[] stringArray)
        {
            string totalString = null;
            for (int i = 0; i < stringArray.Length; i++)
            {
                totalString = totalString + stringArray[i];
            }
            return totalString;
        }
        #endregion

        #region 获取某一字符串在字符串数组中出现的次数
        /// <summary>
        /// 获取某一字符串在字符串数组中出现的次数
        /// </summary>
        /// <param name="stringArray">字符数字</param>
        /// <param name="findString">寻找的字符串</param>
        /// <returns>INT</returns>
        public static int GetStringCount(string[] stringArray, string findString)
        {
            int count = -1;
            string totalString = GetArrayString(stringArray);	//获取数组元素的合并字符串	
            string subString = totalString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = totalString.Substring(subString.IndexOf(findString));
                count += 1;
            }
            return count;
        }
        #endregion

        #region 获取某一字符串在字符串中出现的次数
        /// <summary>
        ///     获取某一字符串在字符串中出现的次数
        /// </summary>
        /// <param name="stringArray" type="string">
        ///     <para>
        ///         原字符串
        ///     </para>
        /// </param>
        /// <param name="findString" type="string">
        ///     <para>
        ///         匹配字符串
        ///     </para>
        /// </param>
        /// <returns>
        ///     匹配字符串数量
        /// </returns>
        public static int GetStringCount(string sourceString, string findString)
        {
            int count = 0;
            int findStringLength = findString.Length;
            string subString = sourceString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
                count += 1;
            }
            return count;
        }
        #endregion

        #region 截取从startString开始到原字符串结尾的所有字符
        /// <summary>
        /// 截取从startString开始到原字符串结尾的所有字符   
        /// </summary>
        /// <param name="sourceString" type="string">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <param name="startString" type="string">
        ///     <para>
        ///         
        ///     </para>
        /// </param>
        /// <returns>
        ///     A string value...
        /// </returns>
        public static string GetSubString(string sourceString, string startString)
        {
            try
            {
                int index = sourceString.ToUpper().IndexOf(startString);
                if (index > 0)
                {
                    return sourceString.Substring(index);
                }
                return sourceString;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ????
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceString"></param>
        /// <param name="beginRemovedString"></param>
        /// <param name="endRemovedString"></param>
        /// <returns></returns>
        public static string GetSubString(string sourceString, string beginRemovedString, string endRemovedString)
        {
            try
            {
                if (sourceString.IndexOf(beginRemovedString) != 0)
                    beginRemovedString = "";

                if (sourceString.LastIndexOf(endRemovedString, sourceString.Length - endRemovedString.Length) < 0)
                    endRemovedString = "";

                int startIndex = beginRemovedString.Length;
                int length = sourceString.Length - beginRemovedString.Length - endRemovedString.Length;
                if (length > 0)
                {
                    return sourceString.Substring(startIndex, length);
                }
                return sourceString;
            }
            catch
            {
                return sourceString; ;
            }
        }
        #endregion

        #region 按字节数取出字符串的长度
        /// <summary>
        /// 按字节数取出字符串的长度
        /// </summary>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字符串的字节数</returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }
        #endregion

        #region 按字节数要在字符串的位置
        /// <summary>
        /// 按字节数要在字符串的位置
        /// </summary>
        /// <param name="intIns">字符串的位置</param>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字节的位置</returns>
        public static int GetByteIndex(int intIns, string strTmp)
        {
            int intReIns = 0;
            if (strTmp.Trim() == "")
            {
                return intIns;
            }
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intReIns = intReIns + 2;
                }
                else
                {
                    intReIns = intReIns + 1;
                }
                if (intReIns >= intIns)
                {
                    intReIns = i + 1;
                    break;
                }
            }
            return intReIns;
        }
        #endregion

        #region 去掉字符中的空格
        /// <summary>
        /// 去掉字符中的空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveMiddleSpace(string str)
        {
            char[] ch = str.ToCharArray();
            StringBuilder sb = new StringBuilder();

            foreach (char c in ch)
            {
                if (char.IsWhiteSpace(c))
                {
                    continue;
                }
                else
                {
                    sb.Append(c.ToString());
                }
            }
            return sb.ToString();
        }
        #endregion

        #region 记算流水号
        /// <summary>
        /// 记算流水号
        /// </summary>
        /// <param name="strId">开始字符串</param>
        /// <param name="i">流水位数</param>
        /// <returns>字符串</returns>
        private static string DoInc(string strId, int i)
        {
            string chrId;
            if (i > 0)
            {
                chrId = strId.Substring(i - 1, 1);
                switch (chrId)
                {
                    case "0":
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                        return strId.Substring(0, i - 1) + (int.Parse(chrId) + 1).ToString() + strId.Substring(i, strId.Length - i);
                    case "9":
                        if (i == 1)
                        {
                            return "10" + strId.Substring(1, strId.Length - 1); ;
                        }
                        else
                        {
                            return DoInc(strId.Substring(0, i - 1) + "0" + strId.Substring(i, strId.Length - i), i - 1);
                        }
                    default:
                        return DoInc(strId, i - 1);
                }
            }
            else
            {
                return strId;
            }
        }
        #endregion

        #region 记算流水号
        /// <summary>
        /// 记算流水号
        /// </summary>
        /// <param name="strId">输入的字符</param>
        /// <returns></returns>
        public static string IncStr(string strId)
        {
            return DoInc(strId, strId.Length);
        }
        #endregion

        #region 按格式取出字符串中变量值-杨栋添加
        public static string GetDataArea(string StrDataArea, string strData)
        {
            int i_PositionBof = 0;
            int i_PositionEof = 0;
            int i_length = 0;
            string StrRet = "";
            //			i_PositionBof = InStr(LCase(strData), LCase(StrDataArea)); //'查找域的起始位置
            i_PositionBof = strData.IndexOf(StrDataArea);
            if (i_PositionBof == -1)
            {
                return "";
            }
            i_PositionBof = i_PositionBof + StrDataArea.Length + 1;
            i_PositionEof = strData.IndexOf("&", i_PositionBof);//  InStr(i_PositionBof, strData, "&");      
            i_length = i_PositionEof - i_PositionBof;
            if (i_PositionEof == -1)    //'如果没有找到分号;，说明是最后一个域		
            {
                StrRet = strData.Substring(i_PositionBof);//Mid(strData, m_PositionBof);		
            }
            else
            {
                StrRet = strData.Substring(i_PositionBof, i_length);//Mid(strData, m_PositionBof, m_length);		
            }
            return StrRet;
        }
        #endregion


        #region 获取包含代理ip的ip串的第一个IP
        public static string GetFirstIp(string ips)
        {
            if ((ips == null) || (ips.Length <= 0))
            {
                return "";
            }

            string[] ip = ips.Split(',');

            if (ip.Length > 0)
            {
                return ip[0].ToString().Trim();
            }
            else
            {
                return ips;
            }

        }
        #endregion


        #region 字符串分解排序 lianyee
        public static string StrSorts(string text, string splitStr)
        {
            char[] splitChar = splitStr.ToCharArray();
            string[] keywords = text.Split(splitChar);
            string newtext = "";
            Array.Sort(keywords);
            ArrayList list = new ArrayList();
            for (int i = 0; i < keywords.Length; i++)
            {
                newtext = newtext + keywords[i];
            }
            return newtext;
        }

        #endregion

        #region 转换ArrayList为字符串
        public static string ConvertArrayList2String(ArrayList list)
        {
            return ConvertArrayList2String(list, ',');
        }
        public static string ConvertArrayList2String(ArrayList list, char separator)
        {
            StringBuilder sb = new StringBuilder();
            if (list == null) return string.Empty;
            foreach (object o in list)
            {
                if (sb.Length != 0)
                    sb.Append(separator);
                sb.Append(o.ToString());
            }
            return sb.ToString();
        }
        #endregion

        #region 取随机数字
        /// <summary>
        /// 生成15位通用ID＝ yMMddHHmmssff ＋1位随机数 = 年1+月2+日2+时2+分2+秒2+毫秒2+随机2=15位       
        /// 此id 10年后会重复 有效期至 2019年7月
        /// </summary>
        /// <returns></returns>
        public static string GetID_15()
        {
            string OutID = "";
            try
            {
                //Random rnd = new Random(Commons.Security.GetRandomSeed());
                //string random2 = rnd.Next(0, 99).ToString();
                //if (random2.Length == 1)
                //{
                //    random2 += "0";
                //}

                ////OutID = DateTime.Now.ToString("yyMMddHHmmss") + random2;
                ////Old 生成14位通用ID＝ 年(2)+月(2)+日(2)+时(2)+分(2)+秒(2)+随机数(2) =14位 随机数
                //TimeSpan myTimeSpan = System.DateTime.Now - Convert.ToDateTime("1900-1-1 00:00:00");
                ////string Time1 = Convert.ToInt64(myTimeSpan.TotalSeconds).ToString();
                //string Time1 = Convert.ToInt64(myTimeSpan.TotalMilliseconds).ToString();
                //OutID = Time1 + random2;

                OutID = DateTime.Now.ToString("yyMMddHHmmss") + GetSYSSerialID(3);
                return OutID;
            }
            catch
            {
                return "";
            }


        }

        public static string GetID_14()
        {
            string OutID = "";
            try
            {
                OutID = DateTime.Now.ToString("yyMMddHHmmss") + GetSYSSerialID(2);
                return OutID;
            }
            catch
            {
                return "";
            }
        }

        public static string GetID_20()
        {
            string OutID = "";
            try
            {
                OutID = DateTime.Now.ToString("yyMMddHHmmssfff") + GetSYSSerialID(5);
                return OutID;
            }
            catch
            {
                return "";
            }
        }

        public static string GetID_25()
        {
            string OutID = "";
            try
            {
                OutID = DateTime.Now.ToString("yyMMddHHmmssfff") + GetSYSSerialID(10);
                return OutID;
            }
            catch
            {
                return "";
            }
        }

        public static string GetID_30()
        {
            string OutID = "";
            try
            {
                OutID = DateTime.Now.ToString("yyMMddHHmmssfff") + GetSYSSerialID(15);
                return OutID;
            }
            catch
            {
                return "";
            }
        }

        private static Object thisLock = new Object();
        public static string GetSYSSerialID(int num)
        {
            lock (thisLock)
            {
                Int64 SYS_ID = 0;

                string IdName = "__SYS_ID_" + num.ToString();

                if (HttpContext.Current.Application[IdName] != null)
                    SYS_ID = Convert.ToInt64(HttpContext.Current.Application[IdName]);

                //num++;
                Int64 MaxId = 99999;
                MaxId = Convert.ToInt64(System.Math.Pow(10.00D, (double)num) - 1);

                if (SYS_ID >= MaxId)
                    SYS_ID = 0;
                else
                    SYS_ID++;

                HttpContext.Current.Application[IdName] = SYS_ID;

                string pad = string.Empty;
                pad = pad.PadRight(num, '0');
                return SYS_ID.ToString(pad);
            }
        }

        public static string GetID_10()
        {
            string OutID = "";
            try
            {
                //Random rnd = new Random(Commons.Security.GetRandomSeed());
                //string random2 = rnd.Next(0, 99).ToString();
                //if (random2.Length == 1)
                //{
                //    random2 += "0";
                //}

                ////OutID = DateTime.Now.ToString("yyMMddHHmmss") + random2;
                ////Old 生成14位通用ID＝ 年(2)+月(2)+日(2)+时(2)+分(2)+秒(2)+随机数(2) =14位 随机数
                //TimeSpan myTimeSpan = System.DateTime.Now - Convert.ToDateTime("1900-1-1 00:00:00");
                ////string Time1 = Convert.ToInt64(myTimeSpan.TotalSeconds).ToString();
                //string Time1 = Convert.ToInt64(myTimeSpan.TotalMilliseconds).ToString();
                //OutID = Time1 + random2;

                OutID = DateTime.Now.ToString("HHmmss") + GetSYSSerialID(4);
                return OutID;
            }
            catch
            {
                return "";
            }

        }

        #endregion


        private static char[] _markupChar = { ' ', ' ', ' ', '<', '>', '&', '"', '*', '/' };
        private static string[] _replaceString = { "&ensp;", "&emsp;", "&nbsp;", "&lt;", "&gt;", "&amp;", "&quot;", "&times;", "&divide;" };
        public static string ReplaceMarkupChar(string source)
        {
            for (int i = 0; i < _replaceString.Length; i++)
                source = source.Replace(_replaceString[i], _markupChar[i].ToString());

            return source;
        }



        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="AHtml"></param>
        /// <returns></returns>
        public static string Strip_Tags(string AHtml)
        {
            Regex regex = new Regex(@"<[^>]*>");
            AHtml = regex.Replace(AHtml, "");

            AHtml = ReplaceMarkupChar(AHtml);

            return AHtml;
        }



        /// <summary>   
        /// 计算文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        /// </summary>
        /// <param name="Text">需计算长度的字符串</param>
        /// <returns>int</returns>
        public static int GbStrLength(string Text)
        {
            int len = 0;

            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = System.Text.Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }

            return len;
        }


        /// <summary>
        /// 按字节数截取,并去除半个汉字 
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="len">截取长度</param>
        /// <returns></returns>
        public static string CutGBStr(string str, int len, string dot = "")
        {
            if (string.IsNullOrEmpty(str)) return "" + dot;


            if (len >= System.Text.Encoding.Default.GetByteCount(str))
                return str;


            string strRe = "";
            int count = 0;
            for (int i = 0; i < str.Length; i++)
            {
                strRe += str.Substring(i, 1);
                count += System.Text.Encoding.Default.GetByteCount(str.Substring(i, 1));
                if (count >= len)
                    break;
            }
            //if(count>len)//截取字符串，最后如果是半个中文，舍掉最后的半个
            //strRe = strRe.Substring(0,strRe.Length - 1);
            return strRe + dot;
        }

        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
        private static Regex RegEmail = new Regex("^[\\w-]+@[\\w-]+\\.(com|net|cn|org|edu|mil|tv|biz|info)(\\.cn)?$");//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
        private static Regex RegUrl = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        private static Regex RegID = new Regex("^[0-9a-zA-Z]*$");
        private static Regex RegData = new Regex(@"^[1-2]\d{3}-((0?[1-9])|(1[0-2]))-((0[1-9])|([1-2]?\d)|(3[0-1]))$");
        private static Regex RegDataTime = new Regex(@"^[1-9]\d{3}-(0?[1-9]|1[0|1|2])-(0?[1-9]|[1|2][0-9]|3[0|1])\s(0?[0-9]|1[0-9]|2[0-3]):(0?[0-9]|[1|2|3|4|5][0-9]):(0?[0-9]|[1|2|3|4|5][0-9])$");


        #region 数字字符串检查

        /// <summary>
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="inputKey">Request的键值</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>返回Request查询字符串</returns>
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (inputKey != null && inputKey != string.Empty)
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }
        /// <summary>
        /// 是否数字字符串
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumber(string inputData)
        {
            Match m = RegNumber.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 邮件地址
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsEmail(string inputData)
        {
            Match m = RegEmail.Match(inputData);
            return m.Success;
        }

        #endregion


        /// <summary>是否电话,包含手机 </summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool isPhone(string strInput)
        {

            if ((strInput == null) || (strInput == ""))
            {
                return false;
            }
            else
            {
                /*
                char[] ca =strInput.ToCharArray();
                for (int i=0;i<ca.Length;i++)
                {
                    if ((ca[i]<'0' || ca[i]>'9') && ca[i]!='-' && ca[i]!='(' && ca[i]!=')' && ca[i]!='+')
                    {					 
                        found=false;
                        break;
                    };
                };
                if (strInput.Substring(strInput.Length-1,1) == "-") found = false;
                */
                Match tt = Regex.Match(strInput, @"^((\(?\d{2,3})\)?)?-?(\(\d{3,4}\)|\d{3,4}-)?((\d{7,8})|(\d{11}))$");
                return tt.Success;

            }

        }
        /// <summary>
        /// 是否是URL
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        public static bool isUrl(string strInput)
        {
            Match m = RegUrl.Match(strInput);
            return m.Success;
        }

        public static bool isnum(string strid)
        {
            Match m = RegID.Match(strid);
            return m.Success;
        }

        /// <summary>
        /// 是不是日期+时间格式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDateTime(string p)
        {
            Match m = RegDataTime.Match(p);
            return m.Success;
        }
        /// <summary>
        /// 是不是日期格式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDate(string p)
        {
            Match m = RegData.Match(p);
            return m.Success;
        }

        /// <summary>
        /// 判断是不是时间格式
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool IsDateOrDateTime(string p)
        {
            try
            {
                Convert.ToDateTime(p);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #region 其他

        /// <summary>
        /// 检查字符串最大长度，返回指定长度的串
        /// </summary>
        /// <param name="sqlInput">输入字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>			
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }
        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(string inputData)
        {
            return HttpUtility.HtmlEncode(inputData);
        }



        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldvalue"></param>
        /// <param name="newvalue"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string ReplaceBat(IList<string> oldvalue, IList<string> newvalue, string content)
        {
            //StringBuilder sb = new StringBuilder(content);

            for (int i = 0; i < oldvalue.Count; i++)
            {
                content = content.Replace(oldvalue[i], newvalue[i]);
                //sb = sb.Replace(dest[i], source[i]);
            }

            return content;
            //return sb.ToString();
        }


        /// <summary>
        ///  用分隔符连接多个串
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="spliter"></param>
        /// <returns></returns>
        public static string ConcatStrs(string[] strs, string spliter = "")
        {
            StringBuilder sb = new StringBuilder();

            foreach (var str in strs)
            {
                if (sb.Length > 0)
                {
                    sb.Append(spliter);
                }
                sb.Append(str);
            }
            return sb.ToString();
        }

        #region 中文参数转换
        public static string UrlEncode(string url, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.GetEncoding("GB2312");
            return HttpUtility.UrlEncode(url, encoding);
        }
        #endregion

        /// <summary>
        /// 解析json
        /// </summary>
        /// <param name="str">json串</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetJsonValue(string str, string key)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(key))
            {
                return "";
            }

            int pos = str.IndexOf(key);
            if (pos < 0)
            {
                return "";
            }

            string temp = str.Substring(pos + 1, str.Length - pos - 1);

            pos = temp.IndexOf(":");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            temp = temp.Substring(pos + 1, temp.Length - pos - 1);

            pos = temp.IndexOf("\"");
            if (pos < 0)
            {
                return "";
            }

            string value = temp.Substring(0, pos);

            return value;
        }
    }
}
