using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TNet.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class ManageUserService
    {
        public static List<ManageUser> GetALL() {
            TN tn = new TN();
            return tn.ManageUsers.ToList();
        }


        public static ManageUser GetManageUserByUserName(string userName) {
            return GetALL().Where(en => en.UserName == userName).FirstOrDefault();
        }
    }
}