using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Merc
{
    public class SetupList
    {
        public List<TCom.EF.Merc> Mercs { get; set; }

        public List<TCom.EF.MercType> Types { get; set; }

        public List<TCom.EF.Setup> Setups { get; set; }

        public List<TCom.EF.SetupAddr> SetupAddrs { get; set; }
    }
}