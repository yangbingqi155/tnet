using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class AdvertiseTypeService
    {
        public static List<AdvertiseType> GetALL()
        {
            TN db = new TN();
            return db.AdvertiseTypes.OrderByDescending(en=>en.sortno).ToList();
        }

        public static AdvertiseType Get(string idat)
        {
            TN db = new TN();
            List<AdvertiseType> advertiseTypes = db.AdvertiseTypes.Where(en => en.idat == idat).ToList();
            return (advertiseTypes != null && advertiseTypes.Count > 0) ? advertiseTypes.First() : null;
        }

        public static AdvertiseType Edit(AdvertiseType advertiseType)
        {
            TN db = new TN();
            AdvertiseType oldAdvertiseType = db.AdvertiseTypes.Where(en => en.idat == advertiseType.idat).FirstOrDefault();

            oldAdvertiseType.idat = advertiseType.idat;
            oldAdvertiseType.typename = advertiseType.typename;
            oldAdvertiseType.sortno = advertiseType.sortno;
            oldAdvertiseType.inuse = advertiseType.inuse;
           

            db.SaveChanges();
            return oldAdvertiseType;
        }

        public static AdvertiseType Add(AdvertiseType advertiseType)
        {
            TN db = new TN();
            db.AdvertiseTypes.Add(advertiseType);
            db.SaveChanges();
            return advertiseType;
        }

        public static List<SelectItemViewModel<string>> SelectItems()
        {
            List<SelectItemViewModel<string>> mercTypeOptions = new List<SelectItemViewModel<string>>();
            List<AdvertiseType> advertiseTypes = GetALL();
            if (advertiseTypes != null && advertiseTypes.Count > 0)
            {
                for (int i = 0; i < advertiseTypes.Count; i++)
                {
                    mercTypeOptions.Add(new SelectItemViewModel<string>()
                    {
                        DisplayValue = advertiseTypes[i].idat,
                        DisplayText = advertiseTypes[i].typename
                    });
                }
            }

            return mercTypeOptions;
        }

    }
}