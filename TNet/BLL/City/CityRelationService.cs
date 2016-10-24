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

        public static List<CityRelation> Add(List<CityRelation> cityRelations)
        {
            TN db = new TN();
            db.CityRelations.AddRange(cityRelations);
            db.SaveChanges();
            return cityRelations;
        }
    }
}