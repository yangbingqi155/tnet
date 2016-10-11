using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models.Manage;
using TNet.Models.User;
using Util;
using TNet.Models;

namespace TNet.BLL.User
{
    public class UserBll
    {
        public static List<TCom.EF.User> SearchByPhone(string phone) {
            List<TCom.EF.User> entities = new List<TCom.EF.User>();
            TN db = new TN();
            entities=db.Users.Where(en => en.phone.Contains(phone)).ToList();

            return entities;
        }

        public static TCom.EF.User Get(long iduser) {
            List<TCom.EF.User> entities = new List<TCom.EF.User>();
            TN db = new TN();
            return db.Users.Where(en=>en.iduser== iduser).First();
        }

        public static bool Auth(ref string user)
        {
            JObject json = getOpenid();
            if (json != null)
            {
                string openid = json["openid"] + "";
                string access_token = json["access_token"] + "";
                if (!string.IsNullOrWhiteSpace(openid))
                {
                    using (TN db = new TN())
                    {
                        TCom.EF.User us = db.Users.Where(m => m.idweixin == openid).FirstOrDefault();
                        if (us == null)
                        {
                            string nickname = "", headimgurl = "";
                            if (!string.IsNullOrWhiteSpace(access_token))
                            {
                                string url = "https://api.weixin.qq.com/sns/userinfo?access_token=" + access_token + "&openid=" + openid + "&lang=zh_CN";
                                string data = HttpHelp.Get(url);
                                if (!string.IsNullOrWhiteSpace(data))
                                {
                                    json = JObject.Parse(data);
                                    if (json != null)
                                    {
                                        nickname = json["nickname"] + "";
                                        headimgurl = json["headimgurl"] + "";
                                    }
                                }
                            }
                            us = new TCom.EF.User();
                            us.iduser = Pub.ID();
                            us.idweixin = openid;
                            us.inuse = true;
                            us.name = nickname;
                            us.notes = "微信";
                            us.phone = "";
                            us.sex = -1;
                            us.avatar = headimgurl;
                            us.comp = "";
                            us.cretime = DateTime.Now;
                            db.Users.Add(us);
                            if (db.SaveChanges() > 0)
                            {
                                user = setUser(us, null);
                                return true;
                            }
                        }
                        else
                        {
                            MUser mu = (from mo in db.ManageUsers
                                        where (mo.idweixin == us.idweixin && mo.inuse == true)
                                        select new MUser
                                        {
                                            name = mo.UserName,
                                            phone = mo.phone,
                                            recvOrder = mo.recv_order,
                                            recvSetup = mo.recv_setup,
                                            sendSetup = mo.send_setup

                                        }).FirstOrDefault();
                            user = setUser(us, mu);
                            return true;
                        }
                    }
                }


            }

            return false;
        }


        private static JObject getOpenid()
        {
            JObject result = new JObject();
            string openid = HttpContext.Current.Request.QueryString["idweixin"];
            result["openid"] = "";
            result["access_token"] = "";
            string code = HttpContext.Current.Request.QueryString["code"];
            if (!string.IsNullOrWhiteSpace(code))
            {
                string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + Pub.appid + "&secret=" + Pub.secret + "&code=" + code + "&grant_type=authorization_code";
                string data = HttpHelp.Get(url);
                if (!string.IsNullOrWhiteSpace(data))
                {
                    JObject json = JObject.Parse(data);
                    if (json != null)
                    {
                        openid = json["openid"] + "";
                        string access_token = json["access_token"] + "";
                        if (!string.IsNullOrWhiteSpace(openid) && !string.IsNullOrWhiteSpace(access_token))
                        {
                            result["openid"] = openid;
                            result["access_token"] = access_token;
                        }
                    }
                }
            }
            else
            {
                result["openid"] = openid;
            }
            return result;
        }

        private static string setUser(TCom.EF.User u, MUser mu)
        {
            UserInfo uo = new UserInfo(u, mu);
            string c = JsonConvert.SerializeObject(uo);
            return c;
        }
    }
}