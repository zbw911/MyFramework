using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Management;
using System.Text.RegularExpressions;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.IO.Compression;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Kt.Framework.Common
{
    public class NetSocket
    {




        #region ����������������Ip��ַ,����ж��IP�����һ�����û�����ʵIP������ȫ�Ǵ����IP���ö��Ÿ���
        public static string getRealIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.Headers["Cdn-Src-Ip"] != null)
            {
                UserIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))  //�õ����������������ip��ַ
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    UserIP = StringUtil.GetFirstIp(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                else
                    UserIP = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            }
            else
            {
                UserIP = HttpContext.Current.Request.UserHostAddress;
            }
            return UserIP;
        }

        public static string getAllIp()
        {
            string UserIP;
            if (HttpContext.Current.Request.Headers["Cdn-Src-Ip"] != null)
            {
                UserIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            }
            else if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)  //�õ����������������ip��ַ
            {

                UserIP = string.Format("{0},{1}",
                    HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"],
                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            }
            else
            {
                UserIP = HttpContext.Current.Request.UserHostAddress;
            }
            return UserIP;
        }
        /// <summary>
        /// ����������IP��ַ
        /// </summary>
        /// <returns></returns>
        public static string getRealIp_NEW()
        {
            string UserIP;
            if (HttpContext.Current.Request.Headers["Cdn-Src-Ip"] != null)
            {
                UserIP = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            }
            else if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))  //�õ����������������ip��ַ
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    UserIP = StringUtil.GetFirstIp(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                else
                    UserIP = HttpContext.Current.Request.ServerVariables["HTTP_VIA"];
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                    UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                else
                    UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

                UserIP = UserIP.Replace("��", ",");
                UserIP = UserIP.Replace(";", ",");
                UserIP = UserIP.Replace("��", ",");

                UserIP = StringUtil.GetFirstIp(UserIP);
            }
            return UserIP;
        }
        #endregion



        #region ��ȡָ��WEBҳ��
        /// <summary>
        /// ��ȡָ��WEBҳ��
        /// </summary>
        /// <param name="strurl">URL</param>
        /// <returns>string</returns>
        public static string GetWebUrl(string strurl)
        {
            try
            {

                WebClient MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                Byte[] pageData = MyWebClient.DownloadData(strurl);
                //string pageHtml = Encoding.UTF8.GetString(pageData);
                string pageHtml = Encoding.Default.GetString(pageData);
                return pageHtml;

            }
            catch (WebException webEx)
            {
                return "error_GetWebUrl:" + webEx.ToString();
            }

        }
        #endregion



        #region GET������ȡҳ��

        
        /// <summary>
        /// hjm
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieheader"></param>
        /// <param name="outcookieheader"></param>
        /// <param name="Header_Referer"></param>
        /// <param name="AutoRedirect"></param>
        /// <param name="Header_UserAgent"></param>
        /// <param name="http_type"></param>
        /// <param name="encoding"></param>
        /// <param name="timeout"></param>
        /// <param name="mywebproxy"></param>
        /// <param name="NetworkCredentialName"></param>
        /// <param name="NetworkCredentialPassword"></param>
        /// <param name="HttpExpect100Continue"></param>
        /// <param name="ServicePointManagerExpect100Continue"></param>
        /// <returns></returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string encoding, int timeout, string mywebproxy, string NetworkCredentialName, string NetworkCredentialPassword, bool HttpExpect100Continue, bool ServicePointManagerExpect100Continue)
        {
            outcookieheader = string.Empty;
            if ((http_type == "https") || url.ToLower().IndexOf("https") != -1)
            {
                //System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy(); //https ����֤��
                //ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            }
            HttpWebResponse res = null;
            string strResult = "";
            try
            {


                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 2)
                {
                    req.Referer = Header_Referer;
                }
                if (Header_UserAgent.Length < 2)
                {
                    Header_UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)"; ;
                }
                if (timeout > 1)
                {
                    req.Timeout = timeout;
                }
                else
                {
                    req.Timeout = timeout;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");��������
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                    if (mywebproxy.IndexOf(":") > 0)
                    {
                        string mywebip = mywebproxy.Substring(0, mywebproxy.IndexOf(":"));
                        req.Headers.Add("X_FORWARDED_FOR", mywebip);
                        req.Headers.Add("VIA", mywebip);
                    }
                }

                if ((NetworkCredentialName.Length > 0) || (NetworkCredentialPassword.Length > 0))
                {
                    NetworkCredential myCred = new NetworkCredential(NetworkCredentialName, NetworkCredentialPassword);
                    CredentialCache myCache = new CredentialCache();
                    myCache.Add(new Uri(url), "Basic", myCred);
                    req.Credentials = myCache;//�������������֤��Ϣ
                }
                req.UserAgent = Header_UserAgent;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");

                if (HttpExpect100Continue == false)
                {
                    req.ServicePoint.Expect100Continue = false;
                }

                if (ServicePointManagerExpect100Continue == false)
                {
                    ServicePointManager.Expect100Continue = false;
                }

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        int IndexOfSeparater = ls_cookies[i].IndexOf("=");//�ҵ���һ��=�ŵ�λ��
                        if (IndexOfSeparater == -1)
                        {
                            continue;
                        }
                        string CookieKey = ls_cookies[i].Substring(0, IndexOfSeparater);
                        string CookieValue = ls_cookies[i].Substring(IndexOfSeparater + 1);
                        cookieCon.Add(new Uri(url), new Cookie(CookieKey.Trim(), CookieValue));
                    }
                    req.CookieContainer = cookieCon;
                }
                Stream ReceiveStream = null;
                //try
                res = (HttpWebResponse)req.GetResponse();

                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {
                    //ReceiveStream = res.GetResponseStream();
                    //ReceiveStream = System.IO.Compression.GZipStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {
                    //ReceiveStream = new GZipInputStream(res.GetResponseStream());                    
                    //ReceiveStream = System.IO.Compression.DeflateStream.Synchronized(res.GetResponseStream());
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }

                //try
                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                if (outcookieheader.Length < 2)
                {
                    //try
                    outcookieheader = res.Headers["Set-Cookie"];
                    if (outcookieheader == null)
                    {
                        outcookieheader = "";
                    }
                    outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                }
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                if (encoding.Trim().Length > 2)
                {
                    encodestr = encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");
                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }


        /// GET������ȡҳ��
        /// ������:GetUrl	
        /// ��������:GET������ȡҳ��	
        /// ��������:
        /// �㷨����:
        /// �� ��: �
        /// �� ��: 2006-11-19 12:00
        /// �� ��: 2007-01-29 17:00 2007-01-29 17:00
        /// �� ��:
        /// �� ��:
        #region GetUrl(String url)
        /// <summary>
        /// GET������ȡҳ��
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <returns></returns>
        public static string GetUrl(String url)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                CookieContainer cookieCon = new CookieContainer();

                //				req.Headers.Add("Accept-Encoding", "gzip, deflate"); 
                //				req.Headers.Add("Accept-Language", "zh-cn"); 
                //				req.Headers.Add("UA-CPU", "x86"); 
                //				req.UserAgent="Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				req.Headers.Add("User-Agent", "Mozilla/4.0"); 
                //				Accept-Language: zh-cn

                req.CookieContainer = cookieCon;
                //				req.CookieContainer.SetCookies(new Uri(url),cookieheader);	

                //res = (HttpWebResponse)req.GetResponse();
                //Stream ReceiveStream = res.GetResponseStream();
                Stream ReceiveStream = null;
                res = (HttpWebResponse)req.GetResponse();
                string Res_ContentEncoding = res.ContentEncoding.ToLower();
                if (Res_ContentEncoding.Contains("gzip"))
                {                    
                    ReceiveStream = new GZipStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else if (Res_ContentEncoding.Contains("deflate"))
                {                   
                    ReceiveStream = new DeflateStream(res.GetResponseStream(), System.IO.Compression.CompressionMode.Decompress);
                }
                else
                {
                    ReceiveStream = res.GetResponseStream();

                }
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,int timeout)
        /// <summary>
        /// GET������ȡҳ��
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <returns></returns>
        public static string GetUrl(String url, int timeout)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.Timeout = timeout;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                CookieContainer cookieCon = new CookieContainer();

                //				req.Headers.Add("Accept-Encoding", "gzip, deflate"); 
                //				req.Headers.Add("Accept-Language", "zh-cn"); 
                //				req.Headers.Add("UA-CPU", "x86"); 
                //				req.UserAgent="Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				req.Headers.Add("User-Agent", "Mozilla/4.0"); 
                //				Accept-Language: zh-cn

                req.CookieContainer = cookieCon;
                //				req.CookieContainer.SetCookies(new Uri(url),cookieheader);	

                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                //				outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,out string  outcookieheader)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <returns></returns>
        public static string GetUrl(String url, out string outcookieheader)
        {
            outcookieheader = string.Empty;
            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //				req.CookieContainer.SetCookies(new Uri(url),cookieheader);			
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }

                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string  cookieheader)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <returns></returns>
        public static string GetUrl(String url, string cookieheader)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                //				outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string  cookieheader,bool AutoRedirect)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <returns></returns>
        public static string GetUrl(String url, string cookieheader, bool AutoRedirect)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;
            string strResult = "";

            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = AutoRedirect;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                //				outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string  cookieheader,out string  outcookieheader,string Header_UserAgent,string http_type)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <returns></returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type)
        {
            outcookieheader = "";



            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                //req.Referer = "https://esales.16288.com/homepage.aspx";
                req.Headers.Add("UA-CPU", "x86");
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				string[] ls_cookies = cookieheader.Split(';');
                //				if (ls_cookies.Length <= 1)
                //				{
                //					req.CookieContainer = cookieCon;
                //					if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //					{
                //						req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //					}
                //				}
                //				else
                //				{
                //
                //					//////////////////////////////////
                //					
                //					string[] ls_cookie = null;
                //
                //					for(int i=0;i<ls_cookies.Length;i++)
                //					{
                //						ls_cookie = ls_cookies[i].Split('=');
                //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                //					}				
                //					
                //					req.CookieContainer = cookieCon;
                //
                //					////////////////////////////////////
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string  cookieheader,out string  outcookieheader,string Header_UserAgent,string http_type,string mywebproxy)
        /// <summary>
        /// skjie ���� date:2008.9.25 10:44
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <param name="mywebproxy">�ò���Ϊ�����ַ  ��ʽΪ "xxx.xxx.xxx.xxx:xxx"</param>
        /// <returns></returns>
        public static string GetUrl(String url, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type, string mywebproxy)
        {
            outcookieheader = "";



            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                //req.Referer = "https://esales.16288.com/homepage.aspx";

                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }

                req.Headers.Add("UA-CPU", "x86");
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				string[] ls_cookies = cookieheader.Split(';');
                //				if (ls_cookies.Length <= 1)
                //				{
                //					req.CookieContainer = cookieCon;
                //					if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //					{
                //						req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //					}
                //				}
                //				else
                //				{
                //
                //					//////////////////////////////////
                //					
                //					string[] ls_cookie = null;
                //
                //					for(int i=0;i<ls_cookies.Length;i++)
                //					{
                //						ls_cookie = ls_cookies[i].Split('=');
                //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                //					}				
                //					
                //					req.CookieContainer = cookieCon;
                //
                //					////////////////////////////////////
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string Referer,string  cookieheader,out string  outcookieheader,string Header_UserAgent,string http_type)
        /// <summary>
        /// ����:ʯ���� 2008-7-12 14:30
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="Referer">����url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <returns></returns>
        public static string GetUrl(String url, string Referer, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type)
        {
            outcookieheader = "";



            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                if (Referer != null && Referer != "")
                {
                    req.Referer = Referer;
                }
                req.Headers.Add("UA-CPU", "x86");
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				string[] ls_cookies = cookieheader.Split(';');
                //				if (ls_cookies.Length <= 1)
                //				{
                //					req.CookieContainer = cookieCon;
                //					if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //					{
                //						req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //					}
                //				}
                //				else
                //				{
                //
                //					//////////////////////////////////
                //					
                //					string[] ls_cookie = null;
                //
                //					for(int i=0;i<ls_cookies.Length;i++)
                //					{
                //						ls_cookie = ls_cookies[i].Split('=');
                //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                //					}				
                //					
                //					req.CookieContainer = cookieCon;
                //
                //					////////////////////////////////////
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }
        #endregion

        #region GetUrl(String url,string Referer,string  cookieheader,out string  outcookieheader,string Header_UserAgent,string http_type,string Encoding)
        /// <summary>
        /// ����:ʯ���� 2008-8-5 11:39
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="Referer">����url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <param name="Encoding"> ָ���ַ����� </param>
        /// <returns></returns>
        public static string GetUrl(String url, string Referer, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type, string Encoding)
        {
            outcookieheader = "";



            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                if (Referer != null && Referer != "")
                {
                    req.Referer = Referer;
                }
                req.Headers.Add("UA-CPU", "x86");
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";


                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				string[] ls_cookies = cookieheader.Split(';');
                //				if (ls_cookies.Length <= 1)
                //				{
                //					req.CookieContainer = cookieCon;
                //					if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //					{
                //						req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //					}
                //				}
                //				else
                //				{
                //
                //					//////////////////////////////////
                //					
                //					string[] ls_cookie = null;
                //
                //					for(int i=0;i<ls_cookies.Length;i++)
                //					{
                //						ls_cookie = ls_cookies[i].Split('=');
                //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                //					}				
                //					
                //					req.CookieContainer = cookieCon;
                //
                //					////////////////////////////////////
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }

                if (Encoding.Trim().Length > 2)
                {
                    encodestr = Encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region GetUrl(String url,string Referer,string  cookieheader,out string  outcookieheader,string Header_UserAgent,string http_type,string Encoding,string mywebproxy)
        /// <summary>
        ///  
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="Referer">����url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="outcookieheader">���Cookie</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <param name="Encoding"> ָ���ַ����� </param>
        /// <returns></returns>
        public static string GetUrl(String url, string Referer, string cookieheader, out string outcookieheader, string Header_UserAgent, string http_type, string Encoding, string mywebproxy)
        {
            outcookieheader = "";


            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                if (Referer != null && Referer != "")
                {
                    req.Referer = Referer;
                }
                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }
                req.Headers.Add("UA-CPU", "x86");
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";



                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }


                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }

                if (Encoding.Trim().Length > 2)
                {
                    encodestr = Encoding;
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion
        #endregion

        #region POST������ȡҳ��
        /// POST������ȡҳ��
        /// ������:PostUrl	
        /// ��������:POST������ȡҳ��	
        /// ��������:
        /// �㷨����:
        /// �� ��: �
        /// �� ��: 2006-11-19 12:00
        /// �� ��: 2007-01-29 17:00
        /// �� ��:
        /// �� ��:
        #region PostUrl(String url, String paramList)
        /// <summary>
        /// POST������ȡҳ��
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <returns></returns>
        public static string PostUrl(String url, String paramList)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,string Header_Referer)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <returns></returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string Header_Referer)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";

                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";


                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }


                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }



        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,string Header_Referer,string mywebproxy)
        /// <summary>
        /// skjie ���� date:2008.9.24 17:35 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="mywebproxy">�ò���Ϊ�����ַ  ��ʽΪ "xxx.xxx.xxx.xxx:xxx"</param> 
        /// <returns></returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string Header_Referer, string mywebproxy)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";

                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";

                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }


                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }



        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,bool Header)
        /// <summary>
        /// added by zbw 2008-1-12
        /// ��������ֻ����ʢ������۰�����
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="Headerr">��ͷ Referer</param>
        /// <returns></returns>
        public static string PostUrl(String url, String paramList, string cookieheader, string Header_Referer, bool OtherHeader)
        {
            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";

                if (OtherHeader)
                {
                    if (Header_Referer.Length > 1)
                    {
                        req.Referer = Header_Referer;
                    }
                    req.KeepAlive = true;
                    req.ContentType = "application/x-www-form-urlencoded";


                    //added by zbw 2008-1-12
                    req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                    req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                    req.Headers.Add("Accept-Encoding", "gzip, deflate");

                }
                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();

                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }


                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,out string outcookieheader)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, out string outcookieheader)
        {
            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }


        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,out string outcookieheader,string Header_Referer)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer)
        {
            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,out string outcookieheader,string Header_Referer,bool AutoRedirect)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="AutoRedirect">�Ƿ��Զ���ת</param>
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect)
        {
            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));						
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));

                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if ((outcookieheader.Length < 2) || (paramList.IndexOf("sSpaceString") >= 0))
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,out string outcookieheader,string Header_Referer,bool AutoRedirect,string mywebproxy)
        /// <summary>
        /// skjie ���� date:2008.9.24 17:35 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="AutoRedirect">�Ƿ��Զ���ת</param>
        /// <param name="mywebproxy">�ò���Ϊ�����ַ  ��ʽΪ "xxx.xxx.xxx.xxx:xxx"</param> 
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string mywebproxy)
        {
            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";

                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);
                //				}


                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));						
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));

                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //							UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //						UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;
                    Stream newStream = req.GetRequestStream();
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if ((outcookieheader.Length < 2) || (paramList.IndexOf("sSpaceString") >= 0))
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }
            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,out string outcookieheader,string Header_Referer,bool AutoRedirect,string Header_UserAgent,string http_type)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="AutoRedirect">�Ƿ��Զ���ת</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type)
        {


            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                req.Referer = Header_Referer;
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;


                    Stream newStream = null;
                    try
                    {
                        newStream = req.GetRequestStream();
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        throw;
                    }
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                //ȡ�÷��ص���Ӧ
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }

                res = (HttpWebResponse)req.GetResponse();

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }

            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #region PostUrl(String url, String paramList,string cookieheader,out string outcookieheader,string Header_Referer,bool AutoRedirect,string Header_UserAgent,string http_type,string mywebproxy)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramList">��ʽ: a=xxx&b=xxx&c=xxx</param>
        /// <param name="cookieheader">����cookie</param>
        /// <param name="outcookieheader">���cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="AutoRedirect">�Ƿ��Զ���ת</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <returns></returns>

        public static string PostUrl(String url, String paramList, string cookieheader, out string outcookieheader, string Header_Referer, bool AutoRedirect, string Header_UserAgent, string http_type, string mywebproxy)
        {


            outcookieheader = string.Empty;

            HttpWebResponse res = null;
            string strResult = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                req.Method = "POST";
                req.AllowAutoRedirect = AutoRedirect;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/msword, application/vnd.ms-powerpoint, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("Cache-Control", "no-cache");
                req.KeepAlive = true;
                req.ContentType = "application/x-www-form-urlencoded";
                req.Headers.Add("UA-CPU", "x86");
                req.Referer = Header_Referer;

                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                StringBuilder UrlEncoded = new StringBuilder();
                Char[] reserved = { '?', '=', '&' };
                byte[] SomeBytes = null;
                if (paramList != null)
                {
                    int i = 0, j;
                    while (i < paramList.Length)
                    {
                        j = paramList.IndexOfAny(reserved, i);
                        if (j == -1)
                        {
                            //UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, paramList.Length-i)));
                            UrlEncoded.Append((paramList.Substring(i, paramList.Length - i)));
                            break;
                        }
                        //UrlEncoded.Append(HttpUtility.UrlEncode(paramList.Substring(i, j-i)));
                        UrlEncoded.Append((paramList.Substring(i, j - i)));
                        UrlEncoded.Append(paramList.Substring(j, 1));
                        i = j + 1;
                    }
                    SomeBytes = Encoding.Default.GetBytes(UrlEncoded.ToString());
                    req.ContentLength = SomeBytes.Length;


                    Stream newStream = null;
                    try
                    {
                        newStream = req.GetRequestStream();
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        throw;
                    }
                    newStream.Write(SomeBytes, 0, SomeBytes.Length);
                    newStream.Close();
                }
                else
                {
                    req.ContentLength = 0;
                }

                //ȡ�÷��ص���Ӧ
                res = (HttpWebResponse)req.GetResponse();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }

                res = (HttpWebResponse)req.GetResponse();

                Stream ReceiveStream = res.GetResponseStream();
                //				Encoding encode = System.Text.Encoding.Default;//GetEncoding("utf-8");
                string encodeheader = res.ContentType;
                string encodestr = System.Text.Encoding.Default.HeaderName;
                if ((encodeheader.IndexOf("charset=") >= 0) && (encodeheader.IndexOf("charset=GBK") == -1) && (encodeheader.IndexOf("charset=gbk") == -1))
                {
                    int i = encodeheader.IndexOf("charset=");
                    encodestr = encodeheader.Substring(i + 8);
                }
                Encoding encode = System.Text.Encoding.GetEncoding(encodestr);//GetEncoding("utf-8");

                StreamReader sr = new StreamReader(ReceiveStream, encode);
                Char[] read = new Char[256];
                int count = sr.Read(read, 0, 256);
                while (count > 0)
                {
                    String str = new String(read, 0, count);
                    strResult += str;
                    count = sr.Read(read, 0, 256);
                }

            }
            catch (Exception e)
            {
                strResult = e.ToString();
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            return strResult;
        }

        #endregion

        #endregion


        #region ��ȡͼƬ

        public static byte[] GetUrlImage(String url, string path, string uid, out string imgfilepath)
        {
            string PathBigImg = path + uid + "nnnnxxxx" + System.IO.Path.GetExtension(url);

            WebClient wc = new WebClient();
            wc.DownloadFile(url, PathBigImg);
            imgfilepath = PathBigImg;
            FileStream file = File.OpenRead(imgfilepath);
            byte[] content = new byte[file.Length];
            file.Read(content, 0, content.Length);
            file.Close();
            return content;
        }
        /// GET������ȡҳ��
        /// ������:GetImage	
        /// ��������:GET������ȡҳ��	
        /// ��������:
        /// �㷨����:
        /// �� ��: �
        /// �� ��: 2006-11-21 09:00
        /// �� ��: 2007-01-29 17:00 2006-12-05 17:00
        /// �� ��:
        /// �� ��:
        /// 
        #region GetImage(String url,string  cookieheader)


        public static byte[] GetImage(String url)
        {
            return GetImage(url, "");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;


            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                //				req.Method = "GET";
                //				req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}


                //req.Headers.Add("X_FORWARDED_FOR", "0.0.0.0"); �����ֶ�

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();


                byte[] mybytes = new byte[15360];

                int count = ReceiveStream.Read(mybytes, 0, 15360);

                byte[] image = new byte[count];

                Array.Copy(mybytes, image, count);



                if (res != null)
                {
                    res.Close();
                }
                return image;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
            }
        }

        #endregion

        #region GetImage(String url,string  cookieheader,out string outcookieheader,string Header_Referer)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;


            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                //				req.Method = "GET";
                //				req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }


                byte[] mybytes = new byte[15360];

                int count = ReceiveStream.Read(mybytes, 0, 15360);

                byte[] image = new byte[count];

                Array.Copy(mybytes, image, count);



                if (res != null)
                {
                    res.Close();
                }
                return image;
            }
            finally
            {
            }
        }

        #endregion

        #region GetImage(String url,string  cookieheader,string Header_Referer)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;


            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                //				req.Method = "GET";
                //				req.ContentType = "application/x-www-form-urlencoded";
                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322)";
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();


                byte[] mybytes = new byte[15360];

                int count = ReceiveStream.Read(mybytes, 0, 15360);

                byte[] image = new byte[count];

                Array.Copy(mybytes, image, count);



                if (res != null)
                {
                    res.Close();
                }
                return image;
            }
            finally
            {
            }
        }

        #endregion

        #region GetImage(String url,string  cookieheader,string Header_Referer,string Header_UserAgent,string http_type)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <param name="Header_Referer">��ͷ Referer</param>
        /// <param name="Header_UserAgent">��ͷ UserAgent</param>
        /// <param name="http_type"> �������� http https </param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, string Header_Referer, string Header_UserAgent, string http_type)
        {
            //			outcookieheader="";



            HttpWebResponse res = null;

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }
                //				req.Method = "GET";
                //				req.ContentType = "application/x-www-form-urlencoded";
                if (Header_UserAgent.Length > 2)
                {
                    req.UserAgent = Header_UserAgent;
                }
                //				CookieContainer cookieCon = new CookieContainer();
                //
                //				//				string name=cookieheader.Substring(0,cookieheader.IndexOf("="));
                //				//				string key=cookieheader.Substring(cookieheader.IndexOf("=")+1,cookieheader.Length-cookieheader.IndexOf("=")-1);
                //				//				cookieCon.Add(new Uri(url),new Cookie(name,key));
                //				
                //				
                //				req.CookieContainer = cookieCon;
                //				if ((cookieheader.Length>0 )&(cookieheader.IndexOf("=")>0))
                //				{
                //					req.CookieContainer.SetCookies(new Uri(url),cookieheader);	
                //				}

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();


                byte[] mybytes = new byte[15360];

                int count = ReceiveStream.Read(mybytes, 0, 15360);

                byte[] image = new byte[count];

                Array.Copy(mybytes, image, count);



                if (res != null)
                {
                    res.Close();
                }
                return image;
            }
            finally
            {
            }
        }

        #endregion

        #region public static byte[]  GetImage(String url,string  cookieheader,out string  outcookieheader )
        /// <summary>
        /// ������ű�ά�ӵ�,����������ֻ����FengYunXiongBaTianXiaImg.aspx��,
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookieheader"></param>
        /// <param name="outcookieheader"></param>
        /// <param name="Header_UserAgent"></param>
        /// <param name="http_type"></param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader)
        {
            outcookieheader = "";


            HttpWebResponse res = null;
            //string strResult = "";
            try
            {
                #region comm funtions
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "GET";
                req.AllowAutoRedirect = false;
                req.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-shockwave-flash, */*";
                req.Headers.Add("Accept-Encoding", "gzip, deflate");

                req.KeepAlive = true;

                req.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();

                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }

                Stream ReceiveStream = null;
                try
                {
                    res = (HttpWebResponse)req.GetResponse();
                    ReceiveStream = res.GetResponseStream();
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }

                try
                {
                    outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie
                }
                catch (Exception exp)
                {
                    string s = exp.Message.ToString();
                    throw;
                }


                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch (Exception exp)
                    {
                        string s = exp.Message.ToString();
                        outcookieheader = "";
                    }
                }

                #endregion


                //��ͼƬ
                //��ʵ����������
                string len = res.Headers["Content-Length"];
                byte[] image = new byte[int.Parse(len)];

                byte[] read = new byte[256];

                int all = 0;

                int count = ReceiveStream.Read(read, 0, 256);
                Array.Copy(read, 0, image, all, count);
                all += count;
                while (count > 0)
                {
                    count = ReceiveStream.Read(read, 0, 256);
                    Array.Copy(read, 0, image, all, count);

                    all += count;
                }

                return image;

            }
            catch (Exception e)
            {
                //strResult = e.ToString();
                return null;
            }
            finally
            {
                if (res != null)
                {
                    res.Close();
                }
            }
            //return strResult;
        }

        #endregion

        #region GetImage(String url,string  cookieheader,out string outcookieheader,string Header_Referer,string mywebproxy)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Ŀ��url</param>
        /// <param name="cookieheader">����Cookie</param>
        /// <returns></returns>
        public static byte[] GetImage(String url, string cookieheader, out string outcookieheader, string Header_Referer, string mywebproxy)
        {
            //			outcookieheader="";

            HttpWebResponse res = null;


            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                if (Header_Referer.Length > 1)
                {
                    req.Referer = Header_Referer;
                }

                if (mywebproxy.Length > 10)
                {
                    //WebProxy myproxy=new WebProxy("218.12.17.138:80");
                    WebProxy myproxy = new WebProxy(mywebproxy);
                    req.Proxy = myproxy;
                }

                //Ϊ�������cookies 
                CookieContainer cookieCon = new CookieContainer();
                //				req.CookieContainer = cookieCon;
                //ȡ��cookies ����
                string[] ls_cookies = cookieheader.Split(';');
                if (ls_cookies.Length <= 1) //�����һ����û��cookies �Ͳ�������ķ�����
                {
                    req.CookieContainer = cookieCon;
                    if ((cookieheader.Length > 0) & (cookieheader.IndexOf("=") > 0))
                    {
                        req.CookieContainer.SetCookies(new Uri(url), cookieheader);
                    }
                }
                else
                {
                    //����Ƕ��cookie �ͷֱ���� cookies ������
                    //////////////////////////////////
                    string[] ls_cookie = null;

                    for (int i = 0; i < ls_cookies.Length; i++)
                    {
                        if (ls_cookies[i].IndexOf("=") == -1)
                        {
                            continue;
                        }
                        ls_cookie = ls_cookies[i].Split('=');
                        //						cookieCon.Add(new Uri(url),new Cookie(ls_cookie[0].ToString().Trim(),ls_cookie[1].ToString().Trim()));
                        cookieCon.Add(new Uri(url), new Cookie(ls_cookie[0].ToString().Trim(), ls_cookies[i].Substring(ls_cookies[i].IndexOf("=") + 1)));
                    }
                    req.CookieContainer = cookieCon;

                    ////////////////////////////////////
                }



                res = (HttpWebResponse)req.GetResponse();
                Stream ReceiveStream = res.GetResponseStream();

                outcookieheader = req.CookieContainer.GetCookieHeader(new Uri(url));//���cookie

                if (outcookieheader.Length < 2)
                {
                    try
                    {
                        outcookieheader = res.Headers["Set-Cookie"];
                        outcookieheader = outcookieheader.Replace(",", ";");//outcookieheader=outcookieheader.Substring(0,outcookieheader.IndexOf(";"));
                    }
                    catch
                    {
                        outcookieheader = "";
                    }
                }


                byte[] mybytes = new byte[15360];

                int count = ReceiveStream.Read(mybytes, 0, 15360);

                byte[] image = new byte[count];

                Array.Copy(mybytes, image, count);



                if (res != null)
                {
                    res.Close();
                }
                return image;
            }
            finally
            {
            }
        }

        #endregion

        #endregion


    }
}