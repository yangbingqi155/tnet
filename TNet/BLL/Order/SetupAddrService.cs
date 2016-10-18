using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;

namespace TNet.BLL
{
    public class SetupAddrService
    {
        public static List<SetupAddr> GetALL()
        {
            return new TN().SetupAddrs.ToList();
        }

        public static SetupAddr Get(string idaddr)
        {
            TN db = new TN();
            List<SetupAddr> setupAddrs = db.SetupAddrs.Where(en => en.idaddr == idaddr).ToList();
            return (setupAddrs != null && setupAddrs.Count() > 0) ? setupAddrs.First() : null;
        }

        public static List<SetupAddr> GetByIdSetup(string idsetup)
        {
            TN db = new TN();
            List<SetupAddr> setupAddrs = db.SetupAddrs.Where(en => en.idsetup == idsetup).ToList();

            return setupAddrs;
        }

        public static SetupAddr Edit(SetupAddr setupAddr)
        {
            TN db = new TN();
            SetupAddr oldSetupAddr = db.SetupAddrs.Where(en => en.idaddr == setupAddr.idaddr).FirstOrDefault();

            oldSetupAddr.idaddr = setupAddr.idaddr;
            oldSetupAddr.idtype = setupAddr.idtype;
            oldSetupAddr.idsetup = setupAddr.idsetup;
            oldSetupAddr.addr = setupAddr.addr;
            oldSetupAddr.phone = setupAddr.phone;
            oldSetupAddr.service = setupAddr.service;
            oldSetupAddr.notes = setupAddr.notes;
            oldSetupAddr.acceptime = setupAddr.acceptime;
            oldSetupAddr.setuptime = setupAddr.setuptime;
            oldSetupAddr.inuse = setupAddr.inuse;

            db.SaveChanges();
            return oldSetupAddr;
        }

        public static SetupAddr Add(SetupAddr setup)
        {
            TN db = new TN();
            db.SetupAddrs.Add(setup);
            db.SaveChanges();
            return setup;
        }
    }
}