using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;
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

        public static Spec GetSpecs(int idspec)
        {
            return GetALL().Where(en => en.idspec == idspec).FirstOrDefault();
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