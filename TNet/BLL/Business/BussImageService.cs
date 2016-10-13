using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.Models;
using TCom.EF;

namespace TNet.BLL {
    public class BussImageService {

        public static List<BussImage> GetALL() {
            TN db = new TN();
            return db.BussImages.ToList();
        }

        public static List<BussImage> GetBussImagesByBussId(long idbuss) {
            TN db = new TN();
            return db.BussImages.Where(en => en.idbuss == idbuss).OrderBy(en => en.SortID).ToList();
        }

        public static BussImage GetBussImage(int BussImageId) {
            return GetALL().Where(en => en.BussImageId == BussImageId).FirstOrDefault();
        }

        public static bool Delete(int BussImageId) {
            bool result = false;
            try {
                TN db = new TN();

                db.BussImages.Remove(db.BussImages.Find(BussImageId));
                db.SaveChanges();
                result = true;
            }
            catch (Exception) {
                result = false;
            }

            return result;
        }
        public static bool DeleteBussImages(long idbuss) {
            bool result = false;
            try {
                TN db = new TN();
                db.BussImages.RemoveRange(db.BussImages.Where(en => en.idbuss == idbuss));
                db.SaveChanges();
                result = true;
            }
            catch (Exception) {
                result = false;
            }

            return result;
        }

        public static BussImage Edit(BussImage bussImage) {
            TN db = new TN();
            BussImage oldBussImage = db.BussImages.Where(en => en.BussImageId == bussImage.BussImageId).FirstOrDefault();

            oldBussImage.BussImageId = bussImage.BussImageId;
            oldBussImage.idbuss = bussImage.idbuss;
            oldBussImage.Path = bussImage.Path;
            oldBussImage.SortID = bussImage.SortID;
            oldBussImage.InUse = bussImage.InUse;

            db.SaveChanges();
            return oldBussImage;
        }

        public static List<BussImage> AddMuti(List<BussImage> bussImages) {
            TN db = new TN();
            db.BussImages.AddRange(bussImages);
            db.SaveChanges();
            return bussImages;
        }

        public static BussImage Add(BussImage bussImage) {
            TN db = new TN();
            db.BussImages.Add(bussImage);
            db.SaveChanges();
            return bussImage;
        }

        public static bool BatchChangSort(List<BussImage> list) {
            bool result = false;
            try {
                TN db = new TN();
                for (int i = 0; i < list.Count; i++) {
                    BussImage img = db.BussImages.Find(list[i].BussImageId);
                    img.SortID = list[i].SortID;
                    db.SaveChanges();
                }
                result = true;
            }
            catch (Exception) {
                result = false;
            }

            return result;
        }

        public static int MaxBussImageSortID(long idbuss) {
            TN db = new TN();
            return db.BussImages.Where(en => en.idbuss == idbuss).Max(en => en.SortID) ?? 0;
        }
    }

}