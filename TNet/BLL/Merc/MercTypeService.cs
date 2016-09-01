using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;

namespace TNet.BLL
{
    /// <summary>
    /// 产品类型
    /// </summary>
    public class MercTypeService
    {
        public static List<MercType> GetALL()
        {
            TN db = new TN();
            return db.MercTypes.ToList();
        }

        public static MercType GetMerc(int idtype)
        {
            return GetALL().Where(en => en.idtype == idtype).FirstOrDefault();
        }
    }
}