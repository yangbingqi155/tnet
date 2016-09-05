using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using WeChatApp.Models;

namespace Util
{
    public static class Pub
    {
        static long t = 0;
        volatile static int un = 0;
        private volatile static object lk = new object();
        public static long ID()
        {
            lock (lk)
            {
                long _t = DateTime.Now.Ticks * 10000;
                if (t == _t)
                {   
                    un++;
                }
                else
                {
                    t = _t;
                    un = 0;
                }
                return t + un;
            }
        }
        /// <summary>
        /// 微信公众号Token
        /// </summary>
        public static string token
        {
            get
            {
                return ConfigurationManager.AppSettings["r_token"];
            }
        }

        /// <summary>
        /// 微信公众号appid
        /// </summary>
        public static string appid
        {
            get
            {
                return ConfigurationManager.AppSettings["r_appid"];
            }
        }


        /// <summary>
        /// 微信公众号secret
        /// </summary>
        public static string secret
        {
            get
            {
                return ConfigurationManager.AppSettings["r_secret"];
            }
        }


        /// <summary>
        /// 从本地获取accessToken
        /// </summary>
        public static string accessToken
        {

            get
            {
                string filepath = HttpContext.Current.Server.MapPath("../App_Data/XMLToken.xml");
                StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
                XmlDocument xml = new XmlDocument();
                xml.Load(str);
                str.Close();
                str.Dispose();
                string Token = xml.SelectSingleNode("xml").SelectSingleNode("AccessToken").InnerText;
                DateTime AccessExpires = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("AccessExpires").InnerText);
                if (DateTime.Now >= AccessExpires)
                {
                    Token = getAccessToken();
                }

                return Token;
            }
        }



        /// <summary>
        /// 从微信服务器获取accessToken，并保存在本地xml中
        /// </summary>
        /// <returns></returns>
        private static string getAccessToken()
        {
            AccessTokenM mode = null;
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + appid + "&secret=" + secret;

            try
            {
                string reqStr = HttpHelp.Get(url);
                if (!string.IsNullOrWhiteSpace(reqStr))
                {
                    JavaScriptSerializer Serializer = new JavaScriptSerializer();
                    mode = Serializer.Deserialize<AccessTokenM>(reqStr);

                    if (mode != null)
                    {
                        string filepath = HttpContext.Current.Server.MapPath("../App_Data/XMLToken.xml");
                        StreamReader str = new StreamReader(filepath, System.Text.Encoding.UTF8);
                        XmlDocument xml = new XmlDocument();
                        xml.Load(str);
                        str.Close();
                        str.Dispose();
                        xml.SelectSingleNode("xml").SelectSingleNode("AccessToken").InnerText = mode.access_token;
                        DateTime _accessExpires = DateTime.Now.AddSeconds(int.Parse(mode.Expires_in));
                        xml.SelectSingleNode("xml").SelectSingleNode("AccessExpires").InnerText = _accessExpires.ToString();
                        xml.Save(filepath);
                    }
                }
                return mode.access_token;
            }
            catch (Exception)
            {

            }
            return "";
        }




        /// <summary>
        /// 本地路径转换成URL相对路径
        /// </summary>
        /// <param name="imagesurl1"></param>
        /// <returns></returns>
        public static string urlconvertor(string imagesurl1)
        {
            string tmpRootDir = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath.ToString());//获取程序根目录
            string imagesurl2 = imagesurl1.Replace(tmpRootDir, ".."); //转换成相对路径
            imagesurl2 = imagesurl2.Replace(@"\", @"/");
            //imagesurl2 = imagesurl2.Replace(@"Aspx_Uc/", @"");
            return imagesurl2;
        }


    }
}