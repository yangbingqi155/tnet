using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;
using Util;

namespace TNet.BLL.User
{
    public class UserBll
    {
        public static bool Auth2()
        {
            string code = HttpContext.Current.Request.QueryString["code"];
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Pub.appid + "&secret=" + Pub.secret + "&code=" + code + "&grant_type=authorization_code";
            string data = HttpHelp.Get(url);
            if (!string.IsNullOrWhiteSpace(data))
            {
                JObject json = JObject.Parse(data);
                if (json != null)
                {
                    string openid = json["openid"] + "";
                    string access_token = json["access_token"] + "";
                    if (!string.IsNullOrWhiteSpace(openid) && !string.IsNullOrWhiteSpace(access_token))
                    {

                        using (TN db = new TN())
                        {
                            int c = db.Users.Where(m => m.idweixin == openid).Count();
                            if (c <= 0)
                            {
                                string nickname = "", headimgurl = "";
                                url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                                data = HttpHelp.Get(url);
                                if (!string.IsNullOrWhiteSpace(data))
                                {
                                    json = JObject.Parse(data);
                                    if (json != null)
                                    {
                                        nickname = json["nickname"] + "";
                                        headimgurl = json["headimgurl"] + "";
                                    }
                                }

                                EF.User u = new EF.User();
                                u.iduser = Pub.ID();
                                u.idweixin = openid;
                                u.inuse = true;
                                u.isoper = false;
                                u.name = nickname;
                                u.notes = "微信";
                                u.phone = "";
                                u.sex = 1;
                                u.avatar = headimgurl;
                                u.comp = "";
                                u.cretime = DateTime.Now;
                                db.Users.Add(u);
                                //try
                                //{
                                    if (db.SaveChanges() > 0)
                                    {
                                        return true;
                                    }
                                //}
                                //catch (Exception e)
                                //{
                                     
                                //}
                              
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }
    }
}