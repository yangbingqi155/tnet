using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.BLL
{
    public class BusinessService
    {
        public static List<Business> GetALL()
        {
            TN db = new TN();
            return db.Businesses.ToList();
        }

        public static Business GetBusiness(long idbuss)
        {
            return GetALL().Where(en => en.idbuss == idbuss).FirstOrDefault();
        }

        public static Business Edit(Business business)
        {
            TN db = new TN();
            Business oldBusiness = db.Businesses.Where(en => en.idbuss == business.idbuss).FirstOrDefault();

            oldBusiness.idbuss = business.idbuss;
            oldBusiness.buss = business.buss;
            oldBusiness.contact = business.contact;
            oldBusiness.phone = business.phone;
            oldBusiness.city = business.city;
            oldBusiness.addr = business.addr;
            oldBusiness.sellpt = business.sellpt;
            oldBusiness.imgs = business.imgs;
            oldBusiness.cretime = business.cretime;
            oldBusiness.busstime = business.busstime;
            oldBusiness.price = business.price;
            oldBusiness.longitude = business.longitude;
            oldBusiness.latitude = business.latitude;
            oldBusiness.runtime = business.runtime;
            oldBusiness.notes = business.notes;
            oldBusiness.blevel = business.blevel;
            oldBusiness.inuse = business.inuse;

            db.SaveChanges();
            return oldBusiness;
        }

        public static Business Add(Business business)
        {
            TN db = new TN();
            db.Businesses.Add(business);
            db.SaveChanges();
            return business;
        }

    }
}