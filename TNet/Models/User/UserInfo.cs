using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.User
{
    public class UserInfo
    {
        public UserInfo(EF.User u)
        {
            this.idweixin = u.idweixin;
            this.iduser = u.iduser+"";
            this.name = u.name;
            this.avatar = u.avatar;
        }
        public string idweixin { get; set; }

        public string iduser { get; set; }

        public string name { get; set; }

        public string avatar { get; set; }
    }
}