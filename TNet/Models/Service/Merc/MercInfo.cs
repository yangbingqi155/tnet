using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Service.Merc
{
    public class MercInfo
    {
        public List<EF.Merc> Merc { get; set; }

        public List<EF.Spec> Spec { get; set; }

        public List<EF.Discount> Discount { get; set; }

    }
}