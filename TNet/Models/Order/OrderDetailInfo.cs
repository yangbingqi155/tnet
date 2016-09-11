using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    public class OrderDetailInfo
    {
        public EF.MyOrder Order { get; set; }

        public List<OrderStatusItem> Status
        {
            get
            {
                return OrderStatus.get();
            }
            set
            {
            }

        }
    }
}