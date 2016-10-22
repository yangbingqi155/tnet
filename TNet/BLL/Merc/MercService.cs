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
        public static List<Merc> GetALL()
        {
            TN db = new TN();
            return (from mo in db.Mercs join mt in db.MercTypes on mo.idtype equals mt.idtype orderby mt.sortno descending,mo.sortno descending select mo).ToList() ;
            //return db.Mercs.ToList();
        }

        public static List<Merc> Search(int idtype = 0, string merc = "", int netype = -1, int isetup = -1) {
            List<Merc> mercs = new List<Merc>();
            TN db = new TN();
            mercs= (from mo in db.Mercs join mt in db.MercTypes on mo.idtype equals mt.idtype orderby mt.sortno descending, mo.sortno descending select mo).Where(en => (
            (idtype == 0 || en.idtype== idtype)
            && (string.IsNullOrEmpty(merc) || en.merc1.Contains(merc))
            &&(netype==-1 || en.netype==netype)
            &&(isetup==-1 || ((isetup==0 || isetup==1) && en.isetup==(isetup == 1)) )
            )).ToList();
            return mercs;

        }

        public static Merc GetMerc(int idmerc)
        {
            return GetALL().Where(en => en.idmerc == idmerc).FirstOrDefault();
        }

        public static Merc Edit(Merc merc)
        {
            TN db = new TN();
            Merc oldMerc = db.Mercs.Where(en => en.idmerc == merc.idmerc).FirstOrDefault();

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
            oldMerc.isetup = merc.isetup;
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

        public static bool SetDefaultMercImage(int idmerc)
        {
            bool result = false;
            try
            {
                TN db = new TN();
                MercImage firstImage = db.MercImages.Where(en => en.idmerc == idmerc).OrderBy(en => en.SortID).First();
                string imagPath = firstImage == null ? "" : firstImage.Path;
                Merc merc = db.Mercs.Find(idmerc);
                merc.imgs = imagPath;
                db.SaveChanges();

            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }
    }
}