using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    //[NotMapped]
    public class MyOrderDetail
    {
        public EF.MyOrder Order { get; set; }



        public List<EF.MyOrderPress> Presses { get; set; }


        public OrderStatusItem Status
        {
            get
            {
                return OrderStatus.get(Order != null && Order.status != null ? Order.status.Value : 0);
            }
            set
            {

            }
        }




    }
}