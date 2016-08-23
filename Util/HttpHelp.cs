using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace WeChatApp.Bll
{
    public class HttpHelp
    {
        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string HttpPost(string Url, string postDataStr)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                byte[] data = encoding.GetBytes(postDataStr);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "POST";
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                CookieContainer cookie = new CookieContainer();
                request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(data, 0, data.Length);
                myRequestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;

            }
            catch (Exception)
            {
            }
            return "";
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="Url"></param>
        /// <param name="postDataStr"></param>
        /// <returns></returns>
        public string DownLoad(string url, string postDataStr, string name)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                byte[] data = encoding.GetBytes(postDataStr);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.AllowAutoRedirect = true;
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                CookieContainer cookie = new CookieContainer();
                request.CookieContainer = cookie;
                Stream myRequestStream = request.GetRequestStream();
                myRequestStream.Write(data, 0, data.Length);
                myRequestStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = cookie.GetCookies(response.ResponseUri);
                Stream myResponseStream = response.GetResponseStream();
                string fileName = HttpContext.Current.Server.MapPath("~/Content/Images/Material/" + name + ".jpg");
                using (FileStream filestream = File.Create(fileName))
                {
                    int rc = 0;
                    data = new byte[1024];
                    while ((rc = myResponseStream.Read(data, 0, data.Length)) > 0)
                    {
                        filestream.Write(data, 0, rc);
                    }
                    filestream.Close();
                }
                return fileName;
            }
            catch (Exception e)
            {

            }
            return null;
        }


        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public string HttpGet(string Url)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.AllowAutoRedirect = true;
                request.Method = "GET";
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, encoding);
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception)
            {
            }
            return "";
        }



    }
}