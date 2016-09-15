using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;

namespace TNet.BLL
{
    public class BusinessService
    {
        public static List<Business> GetALL()
        {
            TN db = new TN();
            return db.Businesses.ToList();
        }

        public static Business GetBusiness(int idbuss)
        {
            return GetALL().Where(en => en.idbuss == idbuss).FirstOrDefault();
        }

    }
}