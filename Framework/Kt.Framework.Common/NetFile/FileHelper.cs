using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kt.Framework.Common.NetFile
{
    /// <summary>
    /// 网络文件帮助方法
    /// added by zbw911 2011-4-25
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 取得文件
        /// </summary>
        /// <param name="dirname"></param>
        /// <returns></returns>
        public string[] findFile(string dirname)
        {
            using (IdentityScope iss = new IdentityScope(username, hostIp, password))
            {
                //try
                //{
                return System.IO.Directory.GetFiles(@"\\" + hostIp + @"\" + dirname);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
                //return null;
            }
        }

        /// <summary>
        /// 取得目录
        /// </summary>
        /// <param name="dirname"></param>
        /// <returns></returns>
        public string[] GetDirectories(string dirname)
        {
            using (IdentityScope iss = new IdentityScope(username, hostIp, password))
            {
                //try
                //{
                return System.IO.Directory.GetDirectories(@"\\" + hostIp + @"\" + dirname);
                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
                //return null;
            }
        }

        /// <summary>
        /// 写入指定的文件，如果不存在创建目录并写入文件
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="filename"></param>
        /// <param name="fileByte"></param>
        public void WriteFile(string dirname, string filename, byte[] fileByte)
        {
            using (IdentityScope iss = new IdentityScope(username, hostIp, password))
            {
                string filepath = GetFileName(dirname, filename);

                var path = Path.GetDirectoryName(filepath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                FileStream fs_stream = new FileStream(filepath, FileMode.CreateNew);

                BinaryWriter writefile = new BinaryWriter(fs_stream);

                writefile.Write(fileByte);

                writefile.Close();         

                //using (StreamWriter sw = new StreamWriter(filepath))
                //{
                //    Stream stream = new MemoryStream(imageByte);
                //    sw.Write(fileByte);
                //}
            }
        }

        public void WriteFile(string dirname, string filename, Stream stream)
        {
            using (IdentityScope iss = new IdentityScope(username, hostIp, password))
            {
                string filepath = GetFileName(dirname, filename);

                var path = Path.GetDirectoryName(filepath);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.WriteLine(stream);
                }
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dirname"></param>
        /// <param name="filename"></param>
        public void DeleteFile(string dirname, string filename)
        {
            using (IdentityScope iss = new IdentityScope(username, hostIp, password))
            {
                string filepath = GetFileName(dirname, filename);

                if (File.Exists(filepath))
                    File.Delete(filepath);
            }
        }


        private string GetFileName(string dirname, string filename)
        {
            string filepath = @"\\" + hostIp + @"\" + startdirname + @"\" + dirname + @"\" + filename;
            return filepath;
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// IP
        /// </summary>
        public string hostIp { get; set; }

        /// <summary>
        /// 起始路径
        /// </summary>
        public string startdirname { get; set; }
    }
}
