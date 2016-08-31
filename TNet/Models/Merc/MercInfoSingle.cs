using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TNet.Models.Service.Merc
{
    
    public class MercDataSingle
    {
        public EF.Merc Merc { get; set; }

        public List<EF.Spec> Spec { get; set; }

        public List<EF.Discount> Discount { get; set; }

    }
}