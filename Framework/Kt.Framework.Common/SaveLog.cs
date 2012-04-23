using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kt.Framework.Common
{

    public class SaveLog
    {

        public string ErrFileDir = string.Empty;

        public static string errpath = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"].ToString();

        public SaveLog()
        {
            if (System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"] != null)
                ErrFileDir = System.Configuration.ConfigurationSettings.AppSettings["ErrorLogPath"];
        }

        #region 直储充值结果日志 保存方式 html
        public bool WriteLog(string name, string strbody)
        {
            strbody = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + strbody.Replace("\r\n", "<br>") + "<br>";
            try
            {
                if (string.IsNullOrEmpty(errpath)) return false;
                string mm = System.DateTime.Now.ToString(@"yyyyMM");
                string date = System.DateTime.Now.ToString(@"yyyyMMdd");
                date = mm + @"\" + date;
                string fileDir = errpath.TrimEnd('\\') + @"\" + date + @"\";
                string filepath = fileDir;// +name + ".htm";

                name = name + ".htm";

                return WriteLog(filepath, name, strbody);

            }
            catch
            {
                return false;
            }
        }

        public static void WriteErrLog(string strbody)
        {
            strbody = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + strbody.Replace("\r\n", "<br>") + "<br>";


            try
            {
                if (string.IsNullOrEmpty(errpath)) return;
                string mm = System.DateTime.Now.ToString(@"yyyyMM");
                string date = System.DateTime.Now.ToString(@"yyyyMMdd");
                date = mm + @"\" + date;
                string fileDir = errpath.TrimEnd('\\') + @"\" + date + @"\";
                string filepath = fileDir;// +name + ".htm";

                string name = "EsalesError.htm";

                WriteErrLog(filepath, name, strbody);

            }
            catch
            {

            }
        }


        public static void WriteErrLog(string filepath, string name, string strbody)
        {

            StreamWriter my_writer = null;

            try
            {
                if (Directory.Exists(filepath) == false)
                {
                    Directory.CreateDirectory(filepath);
                }
                //如果文件存在，则自动追加方式写
                my_writer = new StreamWriter(filepath + name, true, System.Text.Encoding.Default);
                my_writer.Write(strbody);
                my_writer.Flush();

            }
            catch
            {

            }
            finally
            {
                if (my_writer != null)
                    my_writer.Close();
            }
        }

        /// <summary>
        /// 普通错误日志 按日自动创建文件夹，日志文件名为 Error.htm
        /// </summary>
        /// <param name="strbody">日志详细内容</param>
        /// <returns></returns>
        public bool WriteLog(string strbody)
        {
            strbody = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") + "  " + strbody.Replace("\r\n", "<br>") + "<br>";
            return WriteLog("EsalesError", strbody);
        }

        public static bool WriteFormatLog1(string filename, string strbody, params object[] arg)
        {
            string error = string.Format(strbody, arg);
            return new SaveLog().WriteLog(filename, error);
        }

        public static bool WriteFormatLog(string strbody, params object[] arg)
        {
            string error = string.Format(strbody, arg);
            return new SaveLog().WriteLog(error);
        }

        public static bool Debug(string strbody, params object[] arg)
        {
            bool result = true;
            string debug = string.Empty;

            try
            {
                debug = System.Configuration.ConfigurationSettings.AppSettings["Debug"];
                if (!string.IsNullOrEmpty(debug) && debug.ToLower() == "true")
                {
                    string error = string.Format(strbody, arg);
                    result = new SaveLog().WriteLog(error);
                }
                else
                    result = false;
            }
            catch { }

            return result;
        }

        public static bool Debug1(string filename, string strbody, params object[] arg)
        {
            bool result = true;
            string debug = string.Empty;

            try
            {
                debug = System.Configuration.ConfigurationSettings.AppSettings["Debug"];
                if (!string.IsNullOrEmpty(debug) && debug.ToLower() == "true")
                {
                    string error = string.Format(strbody, arg);
                    result = new SaveLog().WriteLog(filename, error);
                }
                else
                    result = false;
            }
            catch { }

            return result;
        }

        /// <summary>
        /// 创建日志文件主函数（一般不直接调用）
        /// </summary>
        /// <param name="filepath">文件路径＋文件名</param>
        /// <param name="strbody">文件内容</param>
        /// <returns></returns>
        public bool WriteLog(string filepath, string name, string strbody)
        {

            StreamWriter my_writer = null;

            try
            {
                if (Directory.Exists(filepath) == false)
                {
                    Directory.CreateDirectory(filepath);
                }
                //如果文件存在，则自动追加方式写
                my_writer = new StreamWriter(filepath + name, true, System.Text.Encoding.Default);
                my_writer.Write(strbody);
                my_writer.Flush();
                return true;

            }
            catch
            {
                return false;
            }
            finally
            {
                if (my_writer != null)
                    my_writer.Close();
            }
        }
        #endregion
    }

}
