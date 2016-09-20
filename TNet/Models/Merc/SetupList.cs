using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Merc
{
    public class SetupList
    {
        public List<EF.Merc> Mercs { get; set; }

        public List<EF.MercType> Types { get; set; }

        public List<EF.Setup> Setups { get; set; }

        public List<EF.SetupAddr> SetupAddrs { get; set; }
    }
}