using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;


namespace TNet.BLL
{
    /// <summary>
    /// 产品基本信息
    /// </summary>
    public class MercService
    {
        public static List<Merc> GetALL() {
            TN db = new TN();
            return db.Mercs.ToList();
        }

        public static Merc GetMerc(int idmerc) {
            return GetALL().Where(en => en.idmerc == idmerc).FirstOrDefault();
        }

        public static Merc Edit(Merc merc) {
            TN db = new TN();
            Merc oldMerc= db.Mercs.Where(en => en.idmerc == merc.idmerc).FirstOrDefault();

            oldMerc.idtype = merc.idtype;
            oldMerc.merc1 = merc.merc1;
            oldMerc.sellpt = merc.sellpt;
            oldMerc.baseprice = merc.baseprice;
            oldMerc.stime = merc.stime;
            oldMerc.entime = merc.entime;
            oldMerc.netype = merc.netype;
            //oldMerc.imgs = merc.imgs;
            oldMerc.descs = merc.descs;
            oldMerc.notes = merc.notes;
            oldMerc.sortno = merc.sortno;
            oldMerc.inuse = merc.inuse;

            db.SaveChanges();
            return oldMerc;
        }

        public static Merc Add(Merc merc)
        {
            TN db = new TN();
            db.Mercs.Add(merc);
            db.SaveChanges();
            return merc;
        }

        public static bool SetDefaultMercImage(int idmerc) {
            bool result = false;
            try {
                TN db = new TN();
                MercImage firstImage= db.MercImages.Where(en => en.idmerc == idmerc).OrderBy(en=>en.SortID).First();
                string imagPath = firstImage == null ? "" : firstImage.Path;
                Merc merc= db.Mercs.Find(idmerc);
                merc.imgs = imagPath;
                db.SaveChanges();

            }
            catch (Exception) {
                result = false;
            }

            return result;
        }
    }
}