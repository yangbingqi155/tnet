using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;
using TNet.Models;

namespace TNet.BLL
{
    public class MyOrderService
    {
        public static List<MyOrder> GetALL()
        {
            TN db = new TN();
            return db.MyOrders.ToList();
        }

        public static MyOrder GetOrder(int orderno)
        {
            return GetALL().Where(en => en.orderno == orderno).FirstOrDefault();
        }

        public static List<MyOrder> GetOrderByFilter(DateTime startDate,DateTime endDate) {
            return GetALL();
        }
    }
}