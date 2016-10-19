using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class SetupService
    {

        public static List<Setup> GetALL()
        {
            return new TN().Setups.ToList();
        }

        public static List<SelectItemViewModel<string>> SelectItems() {
            List<SelectItemViewModel<string>> setupsOptions = new List<SelectItemViewModel<string>>();
            List<Setup> setups = GetALL();
            if (setups != null && setups.Count > 0)
            {
                for (int i = 0; i < setups.Count; i++)
                {
                    setupsOptions.Add(new SelectItemViewModel<string>() {
                        DisplayValue = setups[i].idsetup,
                        DisplayText = setups[i].setup1
                    });
                }
            }

            return setupsOptions;
        }

        public static Setup Get(string idsetup)
        {
            TN db = new TN();
            List<Setup> setups= db.Setups.Where(en => en.idsetup == idsetup).ToList();
            return (setups!=null&&setups.Count()>0)?setups.First():null;
        }

        public static List<Setup> GetByIdType(string idtype) {
            TN db = new TN();
            List<Setup> setups = db.Setups.Where(en => en.idtype == idtype).ToList();

            return setups;
        }

        public static List<Setup> GetByIdTypes(List<string> idtypes)
        {
            TN db = new TN();
            List<Setup> setups = db.Setups.Where(en => idtypes.Contains(en.idtype)).ToList();

            return setups;
        }

        public static Setup Edit(Setup setup)
        {
            TN db = new TN();
            Setup oldSetup = db.Setups.Where(en => en.idsetup == setup.idsetup).FirstOrDefault();

            oldSetup.idsetup = setup.idsetup;
            oldSetup.idtype = setup.idtype;
            oldSetup.setup1 = setup.setup1;
            oldSetup.resource = setup.resource;
            oldSetup.setuptype = setup.setuptype;
            oldSetup.notes = setup.notes;
            oldSetup.inuse = setup.inuse;
           
            db.SaveChanges();
            return oldSetup;
        }

        public static Setup Add(Setup setup)
        {
            TN db = new TN();
            db.Setups.Add(setup);
            db.SaveChanges();
            return setup;
        }
    }
}