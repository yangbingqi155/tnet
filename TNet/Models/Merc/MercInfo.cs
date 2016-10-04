using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCom.EF;
namespace TNet.Models.Service.Merc
{
    public class MercInfo
    {
        public List<TCom.EF.Merc> Merc { get; set; }
                   
        public List<Spec> Spec { get; set; }
                   
        public List<Discount> Discount { get; set; }

    }
}