using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.Models.Manage;

namespace TNet.Models.User
{
    public class UserInfo
    {
        public UserInfo(TCom.EF.User u, MUser mu)
        {
            this.idweixin = u.idweixin;
            this.iduser = u.iduser+"";
            this.name = u.name;
            this.phone = u.phone;
            this.sex = u.sex;
            this.comp = u.comp;
            this.avatar = u.avatar;
            this.cretime = u.cretime != null? u.cretime.Value.ToString("yyyy-MM-dd HH:mm"):"";
            this.notes = u.notes;
            this.mu = mu;
        }
        public string idweixin { get; set; }

        public string iduser { get; set; }

        public string name { get; set; }

        public string phone { get; set; }

        public string avatar { get; set; }

        public int? sex { get; set; }

        public string comp { get; set; }


        public string cretime { get; set; }

        public string notes { get; set; }

        public MUser mu { get; set; }
        
    }
}