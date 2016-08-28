using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Util;
using WeChatApp.Bll;
using WeChatApp.Models;

namespace WeChatApp.Controllers
{
    public class WeChatController : Controller
    {
        [HttpGet]
        public ActionResult Main(signatureM model)
        {
            responseWeChat(model);
            return View();
        }

        [HttpPost]
        public ActionResult Main()
        {
            using (Stream stream = Request.InputStream)
            {
                Byte[] postBytes = new Byte[stream.Length];
                stream.Read(postBytes, 0, (Int32)stream.Length);
                string postString = Encoding.UTF8.GetString(postBytes);
                MsgHelp msgHelp = new MsgHelp();
                string responseContent = msgHelp.responseMsg(postString);
                Response.Write(responseContent);
            }
            return View();
        }


        /// <summary>
        ///响应微信接口测试
        /// </summary>
        private void responseWeChat(signatureM model)
        {
            string echoStr = model.echostr;
            if (checkSignature(model))
            {
                if (!string.IsNullOrWhiteSpace(echoStr))
                {
                    Response.Write(echoStr);
                    Response.End();
                }
            }
        }

        /// <summary>
        /// 验证微信签名,请求是否来源于微信
        /// </summary>
        /// <returns></returns>
        private bool checkSignature(signatureM model)
        {
            string token = Pub.token;
            string[] ArrTmp = { token, model.timestamp, model.nonce };
            Array.Sort(ArrTmp);
            string tmpStr = string.Join("", ArrTmp);

            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();

            if (tmpStr == model.signature)
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
