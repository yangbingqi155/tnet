using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TNet.Models.Order
{
    public class OrderListInfo
    {
        public List<TCom.EF.MyOrder> Order { get; set; }

        public Dictionary<int, OrderStatusItem> Status
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