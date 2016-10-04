using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models.User;
using Util;

namespace TNet.BLL.User
{
    public class UserBll
    {
        public static bool Auth(ref string user)
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
                            TCom.EF.User us = db.Users.Where(m => m.idweixin == openid).FirstOrDefault();
                            if (us == null)
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
                                us = new TCom.EF.User();
                                us.iduser = Pub.ID();
                                us.idweixin = openid;
                                us.inuse = true;
                                us.isoper = false;
                                us.name = nickname;
                                us.notes = "微信";
                                us.phone = "";
                                us.sex = 1;
                                us.avatar = headimgurl;
                                us.comp = "";
                                us.cretime = DateTime.Now;
                                db.Users.Add(us);
                                if (db.SaveChanges() > 0)
                                {
                                    user = setUser(us);
                                    return true;
                                }
                            }
                            else
                            {
                                user = setUser(us);
                                return true;
                            }
                        }
                    }
                }

            }

            return false;
        }



        private static string setUser(TCom.EF.User u)
        {
            UserInfo uo = new UserInfo(u);
            string c = JsonConvert.SerializeObject(uo);
            return c;
        }
    }
}