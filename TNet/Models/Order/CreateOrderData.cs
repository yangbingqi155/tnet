using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    public class CreateOrderData
    {     
        public long iduser { get; set; }
         
        public int idmerc { get; set; }
         
        public string merc { get; set; }

        public int idspec { get; set; }

        public string spec { get; set; }

        public double price { get; set; }

        public int month { get; set; }

        public int attmonth { get; set; }

        
        public string contact { get; set; }
        
        
        public string addr { get; set; }

        
        public string phone { get; set; }

 
        public string notes { get; set; }
 
    }
}