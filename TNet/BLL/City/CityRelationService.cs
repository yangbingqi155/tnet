using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TCom.EF;
using TNet.Models;
using Util;

namespace TNet.BLL
{
    public class CityRelationService
    {

        public static List<CityRelation> GetALL()
        {
            TN db = new TN();
            return db.CityRelations.ToList();
        }

        public static List<CityRelation> GetByModuleId(string idmodule,ModuleType moduleType)
        {
            TN db = new TN();
            List<CityRelation> cityRelations = db.CityRelations.Where(en => 
            (
                en.moduletype == (int)moduleType 
                && en.idmodule==idmodule
            )).ToList();

            return cityRelations;
        }
        public static List<CityRelation> GetByModuleIdList(List<string> idmoduleList, ModuleType moduleType)
        {
            TN db = new TN();
            List<CityRelation> cityRelations = db.CityRelations.Where(en =>
            (
                en.moduletype == (int)moduleType
                && idmoduleList.Contains(en.idmodule)
            )).ToList();

            return cityRelations;
        }

        public static CityRelation Get(string idrelation)
        {
            TN db = new TN();
            List<CityRelation> cityRelations = db.CityRelations.Where(en => en.idrelation == idrelation).ToList();
            return (cityRelations != null && cityRelations.Count > 0) ? cityRelations.First() : null;
        }

        public static CityRelation Edit(CityRelation cityRelation)
        {
            TN db = new TN();
            CityRelation oldCityRelation = db.CityRelations.Where(en => en.idrelation == cityRelation.idrelation).FirstOrDefault();

            oldCityRelation.idrelation = cityRelation.idrelation;
            oldCityRelation.idcity = cityRelation.idcity;
            oldCityRelation.idmodule = cityRelation.idmodule;
            oldCityRelation.moduletype = cityRelation.moduletype;
            oldCityRelation.inuse = cityRelation.inuse;

            db.SaveChanges();
            return oldCityRelation;
        }

        public static bool Save(List<CityRelation> cityRelations) {
            bool result = false;
            if (cityRelations==null|| cityRelations.Count==0) {
                return result;
            }
            try
            {
                TN db = new TN();
                string idmodule = cityRelations.First().idmodule;
                int? moduleType = cityRelations.First().moduletype;

                List<CityRelation> currentRelations = db.CityRelations.Where(en => en.idmodule == idmodule && en.moduletype == moduleType).ToList();
                List<CityRelation> existCityRelations  = currentRelations.Where(en => (cityRelations.Contains<CityRelation>(en, CityRelationEqualityComparer.Instance))).ToList();
                List<CityRelation> removeCityRelations = currentRelations.Where(en => !existCityRelations.Contains<CityRelation>(en, CityRelationEqualityComparer.Instance)).ToList();
                List<CityRelation> addCityRelations = cityRelations.Where(en=> !currentRelations.Contains<CityRelation>(en, CityRelationEqualityComparer.Instance)).ToList();

                addCityRelations = addCityRelations.Select(mod=> {
                    mod.idrelation = Pub.ID().ToString();
                    return mod;
                }).ToList();

                if (removeCityRelations.Count > 0)
                {
                    Delete(removeCityRelations);
                }

                if (addCityRelations.Count > 0)
                {
                    Add(addCityRelations);
                }
                result = true;
            }
            catch (Exception ex) {
                result = false;
            }

            return result;
        }

        public static List<CityRelation> Delete(List<CityRelation> cityRelations)
        {
            TN db = new TN();
            List<string> idrelations = cityRelations.Select(mod=> {
                return mod.idrelation;
            }).ToList();
            db.CityRelations.RemoveRange(db.CityRelations.Where(en => idrelations.Contains(en.idrelation)));
           
            db.SaveChanges();
            return cityRelations;
        }

        public static List<CityRelation> Add(List<CityRelation> cityRelations)
        {
            TN db = new TN();
            db.CityRelations.AddRange(cityRelations);
            db.SaveChanges();
            return cityRelations;
        }
    }
}