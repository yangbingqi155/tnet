using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;


namespace TNet.BLL
{
    /// <summary>
    /// 商品基本信息
    /// </summary>
    public class MercService
    {
        public static List<TNet.EF.Merc> GetALL() {
            TN db = new TN();
            return db.Mercs.ToList();
        }

        public static TNet.EF.Merc GetMerc(int idmerc) {
            return GetALL().Where(en => en.idmerc == idmerc).FirstOrDefault();
        }
    }
}