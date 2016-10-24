using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class AdvertiseService
    {

        public static List<Advertise> GetALL()
        {
            TN db = new TN();
            return (from ad in db.Advertises join at in db.AdvertiseTypes on ad.idat equals at.idat orderby at.sortno descending, ad.sortno descending select ad).ToList();
        }

        private static List<AdvertiseViewModel> ConvertToViewModel(List<Advertise> entities)
        {
            List<AdvertiseViewModel> viewModels = new List<AdvertiseViewModel>();
            List<string> idtypes = entities.Select(mod => {
                return mod.idat;
            }).ToList();
            idtypes = idtypes.Distinct().ToList();

            TN db = new TN();
            List<AdvertiseType> advertiseTypes = db.AdvertiseTypes.Where(en => idtypes.Contains(en.idat)).ToList();
            viewModels = entities.Select(mod => {
                AdvertiseViewModel viewModel = new AdvertiseViewModel();
                viewModel.CopyFromBase(mod);
                
                if (advertiseTypes != null && advertiseTypes.Count > 0)
                {

                    List<AdvertiseType> tempAdvertiseTypes = advertiseTypes.Where(en => en.idat == viewModel.idat).ToList();
                    AdvertiseType tempAdvertiseType = (tempAdvertiseTypes != null && tempAdvertiseTypes.Count > 0) ? tempAdvertiseTypes.First() : null;
                    AdvertiseTypeViewModel tempAdvertiseTypeModel = new AdvertiseTypeViewModel();
                    if (tempAdvertiseType != null)
                    {
                        tempAdvertiseTypeModel.CopyFromBase(tempAdvertiseType);
                        viewModel.AdvertiseType = tempAdvertiseTypeModel;
                    }
                }

                return viewModel;
            }).ToList();

            return viewModels;
        }

        public static List<AdvertiseViewModel> SearchViewModels(DateTime? sdate, DateTime? edate, string idat = "", string title = "")
        {
            List<AdvertiseViewModel> viewModels = new List<AdvertiseViewModel>();
            List<Advertise> advertises = new List<Advertise>();
            advertises=Search(sdate, edate, idat, title);
            viewModels=ConvertToViewModel(advertises);
            return viewModels;
        }

        public static List<Advertise> Search(DateTime? sdate,DateTime? edate,string idat = "",string title="")
        {
            List<Advertise> advertises = new List<Advertise>();
            TN db = new TN();
            advertises = (from ad in db.Advertises join at in db.AdvertiseTypes on ad.idat equals at.idat orderby ad.sortno descending, ad.sortno descending select ad).Where(en => (
              (sdate == null || SqlFunctions.DateDiff("dd", sdate.Value, en.cretime) >= 0)
              && (edate == null || SqlFunctions.DateDiff("dd", edate.Value, en.cretime) <= 0)
             && (string.IsNullOrEmpty(title) || en.title.Contains(title))
             && (string.IsNullOrEmpty(idat) || en.idat == idat)
             )).ToList();
            return advertises;

        }

        public static Advertise Get(string idav)
        {
            TN db = new TN();
            List<Advertise> advertises = db.Advertises.Where(en => en.idav == idav).ToList();
            return (advertises != null && advertises.Count > 0) ? advertises.First() : null;
        }

        public static Advertise Edit(Advertise advertise)
        {
            TN db = new TN();
            Advertise oldAdvertise = db.Advertises.Where(en => en.idav == advertise.idav).FirstOrDefault();

            oldAdvertise.idav = advertise.idav;
            oldAdvertise.idat = advertise.idat;
            oldAdvertise.title = advertise.title;
            oldAdvertise.img = advertise.img;
            oldAdvertise.link = advertise.link;
            oldAdvertise.sortno = advertise.sortno;
            oldAdvertise.inuse = advertise.inuse;

            db.SaveChanges();
            return oldAdvertise;
        }

        public static Advertise Add(Advertise advertise)
        {
            TN db = new TN();
            db.Advertises.Add(advertise);
            db.SaveChanges();
            return advertise;
        }
      
    }
}