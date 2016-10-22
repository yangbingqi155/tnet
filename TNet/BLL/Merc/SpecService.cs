using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class SpecService
    {
        public static List<Spec> GetALL()
        {
            TN db = new TN();
            return db.Specs.ToList();
        }
        public static List<SpecViewModel> GetSpecsByIdMerc(int idmerc)
        {
            List<SpecViewModel> specs = new List<SpecViewModel>();
            TN tn = new TN();
            List<Spec> entities =GetALL().Where(en => en.idmerc == idmerc).ToList();
          
            if (entities!=null&&entities.Count>0) {
                specs = entities.Select(model => {
                    SpecViewModel viewModel = new SpecViewModel();
                    viewModel.CopyFromBase(model);
                    return viewModel;
                }).ToList();
                List<Merc> mercs = tn.Mercs.ToList();
                specs=specs.Select(en=> {
                    Merc merc = mercs.Find(model=>model.idmerc==en.idmerc);
                    en.merc = merc != null ? merc.merc1 : "";
                    return en;
                }).ToList();
            }

            return specs;
        }

        public static SpecViewModel GetSpec(string idspec)
        {
            SpecViewModel viewModel = null;
            Spec spec= GetALL().Where(en => en.idspec == idspec).FirstOrDefault();
            if (spec!=null) {
                viewModel = new SpecViewModel();
                viewModel.CopyFromBase(spec);
                Merc merc= MercService.GetMerc(viewModel.idmerc);
                viewModel.merc = merc != null ? merc.merc1 : "";
            }
            return viewModel;
        }

        public static Spec Get(string idspec)
        {
            Spec spec = GetALL().Where(en => en.idspec == idspec).FirstOrDefault();
            return spec;
        }

        public static Spec Edit(Spec spec)
        {
            TN db = new TN();
            Spec oldSpec = db.Specs.Where(en => en.idspec == spec.idspec).FirstOrDefault();

            oldSpec.idmerc = spec.idmerc;
            oldSpec.idspec = spec.idspec;
            oldSpec.spec1 = spec.spec1;
            oldSpec.price = spec.price;
            oldSpec.sellcount = spec.sellcount;
            oldSpec.month = spec.month;
            oldSpec.unit = spec.unit;
            oldSpec.up = spec.up;
            oldSpec.down = spec.down;
            oldSpec.attmonth = spec.attmonth;
            oldSpec.stuprice = spec.stuprice;
            oldSpec.moveprice = spec.moveprice;
            oldSpec.usertype = spec.usertype;
            oldSpec.notes = spec.notes;
            oldSpec.inuse = spec.inuse;

            db.SaveChanges();
            return oldSpec;
        }

        public static Spec Add(Spec spec)
        {
            TN db = new TN();
            db.Specs.Add(spec);
            db.SaveChanges();
            return spec;
        }
    }
}