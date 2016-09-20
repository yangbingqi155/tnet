using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Merc
{
    public class MercList
    {
        public List<EF.Merc> Mercs { get; set; }

        public List<EF.MercType> Types { get; set; }
    }
}