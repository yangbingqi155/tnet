using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class SetupAddrService
    {
        public static List<SetupAddr> GetALL()
        {
            return new TN().SetupAddrs.ToList();
        }

        public static List<SetupAddrViewModel> GetALLViewModels() {
            List<SetupAddrViewModel> viewModels = new List<SetupAddrViewModel>();
            List<SetupAddr> list = GetALL();
            if (list!=null&&list.Count>0) {
                viewModels = ConvertToViewModel(list);
            }

            return viewModels;
        }

        public static List<SetupAddrViewModel> ConvertToViewModel(List<SetupAddr> entities) {
            List<SetupAddrViewModel> viewModels = new List<SetupAddrViewModel>();
            List<string> idsetups = entities.Select(mod=> {
                return mod.idsetup;
            }).ToList();
            List<string> idtypes = entities.Select(mod => {
                return mod.idtype;
            }).ToList();
            idsetups = idsetups.Distinct().ToList();
            idtypes = idtypes.Distinct().ToList();

            TN db = new TN();
            List<Setup> setups=db.Setups.Where(en => idsetups.Contains(en.idsetup)).ToList();
            List<MercType> mercTypes=db.MercTypes.Where(en => idtypes.Contains(en.idtype.ToString())).ToList();
            viewModels = entities.Select(mod => {
                SetupAddrViewModel viewModel = new SetupAddrViewModel();
                viewModel.CopyFromBase(mod);
                if (setups != null && setups.Count > 0) {
                    List<Setup> tempSetups = setups.Where(en => en.idsetup == viewModel.idsetup).ToList();
                    Setup tempSetup = (tempSetups != null && tempSetups.Count > 0) ? tempSetups.First() : null;
                    SetupViewModel tempSetupModel = new SetupViewModel();
                    if (tempSetup!=null) {
                        tempSetupModel.CopyFromBase(tempSetup);
                        viewModel.setup = tempSetupModel;
                    }
                }
                if (mercTypes!=null&& mercTypes.Count>0) {

                    List<MercType> tempMercTypes = mercTypes.Where(en => en.idtype.ToString() == viewModel.idtype).ToList();
                    MercType tempMercType = (tempMercTypes != null && tempMercTypes.Count > 0) ? tempMercTypes.First() : null;
                    MercTypeViewModel tempMercTypeModel = new MercTypeViewModel();
                    if (tempMercType != null)
                    {
                        tempMercTypeModel.CopyFromBase(tempMercType);
                        viewModel.merctype = tempMercTypeModel;
                    }
                }

                return viewModel;
            }).ToList();

            return viewModels;
        }

        public static SetupAddr Get(string idaddr)
        {
            TN db = new TN();
            List<SetupAddr> setupAddrs = db.SetupAddrs.Where(en => en.idaddr == idaddr).ToList();
       
            return (setupAddrs != null && setupAddrs.Count() > 0) ? setupAddrs.First() : null;
        }

        public static SetupAddrViewModel GetViewModel(string idaddr) {
            List<SetupAddrViewModel> viewModels = new List<SetupAddrViewModel>();
            SetupAddr setupAddr = Get(idaddr);
            if (setupAddr != null )
            {
                viewModels = ConvertToViewModel(new List<SetupAddr>() { setupAddr });
            }

            return (viewModels != null && viewModels.Count > 0) ? viewModels.First() : null;
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