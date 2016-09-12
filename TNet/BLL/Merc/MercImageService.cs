using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;


namespace TNet.BLL
{
    /// <summary>
    /// 产品图片
    /// </summary>
    public class MercImageService
    {
        public static List<MercImage> GetALL()
        {
            TN db = new TN();
            return db.MercImages.ToList();
        }

        public static List<MercImage> GetMercImagesByMercId(int idmerc)
        {
            TN db = new TN();
            return db.MercImages.Where(en=>en.idmerc== idmerc).ToList();
        }

        public static MercImage GetMercImage(int MercImageId)
        {
            return GetALL().Where(en => en.MercImageId == MercImageId).FirstOrDefault();
        }

        public static MercImage Edit(MercImage mercImage)
        {
            TN db = new TN();
            MercImage oldMercImage = db.MercImages.Where(en => en.MercImageId == mercImage.MercImageId).FirstOrDefault();

            oldMercImage.idmerc = mercImage.idmerc;
            oldMercImage.InUse = mercImage.InUse;
            oldMercImage.MercImageId = mercImage.MercImageId;
            oldMercImage.Path = mercImage.Path;
            oldMercImage.SortID = mercImage.SortID;

            db.SaveChanges();
            return oldMercImage;
        }

        public static MercImage Add(MercImage mercImage)
        {
            TN db = new TN();
            db.MercImages.Add(mercImage);
            db.SaveChanges();
            return mercImage;
        }
    }
}