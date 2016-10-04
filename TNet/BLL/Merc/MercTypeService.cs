using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

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

        public static MercType GetMercType(int idtype)
        {
            return GetALL().Where(en => en.idtype == idtype).FirstOrDefault();
        }

        public static MercType Edit(MercType mercType)
        {
            TN db = new TN();
            MercType oldMercType = db.MercTypes.Where(en => en.idtype == mercType.idtype).FirstOrDefault();

            oldMercType.idtype = mercType.idtype;
            oldMercType.name = mercType.name;
            oldMercType.notes = mercType.notes;
            oldMercType.sortno = mercType.sortno;
            oldMercType.inuse = mercType.inuse;

            db.SaveChanges();
            return oldMercType;
        }

        public static MercType Add(MercType mercType)
        {
            TN db = new TN();
            db.MercTypes.Add(mercType);
            db.SaveChanges();
            return mercType;
        }
    }
}