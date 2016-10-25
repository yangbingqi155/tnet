using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using TCom.EF;

namespace TNet.Models
{
    [NotMapped]
    public class CityRelationViewModel:CityRelation, IEqualityComparer<CityRelationViewModel>
    {
        [Display(Name ="关系编号")]
        [StringLength(50)]
        public new string idrelation { get; set; }

        [Display(Name = "城市编号")]
        [StringLength(50)]
        public new string idcity { get; set; }

        [Display(Name = "模块编号")]
        [StringLength(50)]
        public new string idmodule { get; set; }

        [Display(Name = "模块类型")]
        public new int? moduletype { get; set; }

        [Display(Name = "启用")]
        public new bool? inuse { get; set; }

        public bool Equals(CityRelationViewModel x, CityRelationViewModel y)
        {
            return (x.idcity == y.idcity&&x.idmodule==y.idmodule&&x.moduletype==y.moduletype);
        }

        public int GetHashCode(CityRelationViewModel model)
        {
            return model.ToString().GetHashCode();
        }

        public void CopyFromBase(CityRelation cityRelation)
        {
            this.idrelation = cityRelation.idrelation;
            this.idcity = cityRelation.idcity;
            this.idmodule = cityRelation.idmodule;
            this.moduletype = cityRelation.moduletype;
            this.inuse = cityRelation.inuse;
        }

        public void CopyToBase(CityRelation cityRelation)
        {
            cityRelation.idrelation = this.idrelation;
            cityRelation.idcity = this.idcity;
            cityRelation.idmodule = this.idmodule;
            cityRelation.moduletype = this.moduletype;
            cityRelation.inuse = this.inuse;
        }
    }
}