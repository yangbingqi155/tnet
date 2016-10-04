using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Merc
{
    public class MercList
    {
        public List<TCom.EF.Merc> Mercs { get; set; }

        public List<TCom.EF.MercType> Types { get; set; }
    }
}