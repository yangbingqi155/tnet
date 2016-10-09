using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;


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
            return db.MercImages.Where(en=>en.idmerc== idmerc).OrderBy(en=>en.SortID).ToList();
        }

        public static MercImage GetMercImage(int mercImageId)
        {
            return GetALL().Where(en => en.MercImageId == mercImageId).FirstOrDefault();
        }

        public static bool Delete(int mercImageId) {
            bool result = false;
            try {
                TN db = new TN();

                db.MercImages.Remove(db.MercImages.Find(mercImageId));
                db.SaveChanges();
                result = true;
            }
            catch (Exception ex) {
                result = false;
            }
            
            return result;
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

        public static bool BatchChangSort(List<MercImage> list) {
            bool result = false;
            try {
                TN db = new TN();
                for (int i = 0; i < list.Count; i++) {
                    MercImage img= db.MercImages.Find(list[i].MercImageId);
                    img.SortID = list[i].SortID;
                    db.SaveChanges();
                }
                    result = true;
            }
            catch (Exception ex) {
                result = false;
            }
            
            return result;
        }

        public static int MaxMercImageSortID(int idmerc) {
            TN db = new TN();
            return db.MercImages.Where(en => en.idmerc == idmerc).Max(en=>en.SortID)??0;
        }
    }
}