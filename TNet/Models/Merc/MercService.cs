using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;


namespace TNet.Models.Service
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
    }
}