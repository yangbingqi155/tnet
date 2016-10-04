using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TCom.EF
{
    public partial class TN
    {
        static TN()
        {
            System.Data.Entity.Database.SetInitializer<TN>(null);
        }
    }
}