using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TNet.Models.Service.Merc
{
    
    public class MercDataSingle
    {
        public TCom.EF.Merc Merc { get; set; }

        public List<TCom.EF.Spec> Spec { get; set; }

        public List<TCom.EF.Discount> Discount { get; set; }

        public List<TCom.EF.Setup> Setups { get; set; }

        public List<TCom.EF.SetupAddr> SetupAddrs { get; set; }

    }
}