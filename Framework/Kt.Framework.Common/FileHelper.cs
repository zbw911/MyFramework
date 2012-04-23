using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Kt.Framework.Common
{
    /// <summary>
    /// 文件帮助方法 
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 指向的路径是否是文件及文件是否存在
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsFile(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// 取得文件信息
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static FileInfo GetFileInfo(string filepath)
        {
            if (!IsFile(filepath))
            {
                return null;
            }


            FileInfo FileInfo = new FileInfo(filepath);
            return FileInfo;

        }

        /// <summary>
        /// 是不是图片类型的文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsImageFile(string path)
        {
            // throw new NotImplementedException();
            //本方法 未实现，
            string extname = Path.GetExtension(path);

            if (extname.ToLower() == ".jpg" || extname.ToLower() == ".jpeg" || extname.ToLower() == ".gif" || extname.ToLower() == ".png")
                return true;
            return false;

        }


        public static void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
                File.Delete(filepath);
        }



        /// <summary>
        /// 创建图片保存目录
        /// </summary>
        /// <param name="path"></param>
        public static void FolderCreate(string path)
        {
            path = Path.GetDirectoryName(path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary> 
        /// 将 Stream 转成 byte[] 
        /// </summary> 
        public static byte[] StreamToBytes(Stream stream)
        {
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);

            // 设置当前流的位置为流的开始 
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        /// <summary>
        /// 将二进制流写为图片
        /// </summary>
        /// <param name="imageByte"></param>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string SaveImageFile(byte[] imageByte, string path)
        {
            Stream stream = new MemoryStream(imageByte);
            return SaveImageFile(stream, path);
        }

        public static string SaveImageFile(Stream stream, string path)
        {

            Image im = Image.FromStream(stream);




            FolderCreate(path);

            im.Save(path);

            return path;
        }

        /// <summary>
        /// 类型
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CheckImageExt(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            try
            {
                Image im = Image.FromStream(stream);
                 var format = im.RawFormat;

                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Bmp.Guid.ToString())
                     return ".bmp";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Emf.Guid.ToString())
                     return ".emf";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Exif.Guid.ToString())
                     return ".exif";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Gif.Guid.ToString())
                     return ".gif";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Icon.Guid.ToString())
                     return ".icon";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Jpeg.Guid.ToString())
                     return ".jpg";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.MemoryBmp.Guid.ToString())
                     return ".bmp";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Png.Guid.ToString())
                     return ".png";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Tiff.Guid.ToString())
                     return ".tiff";
                 if (format.Guid.ToString() == System.Drawing.Imaging.ImageFormat.Wmf.Guid.ToString())
                     return ".wmf";
            }
            catch 
            {
                
                return null;
            }
           
           

            return null;
        }
    }
}
