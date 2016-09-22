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

        public static MyOrder GetOrder(long orderno)
        {
            return GetALL().Where(en => en.orderno == orderno).FirstOrDefault();
        }

        public static List<MyOrder> GetOrderByFilter(DateTime startDate,DateTime endDate) {
            return GetALL();
        }


        public static MyOrder Edit(MyOrder order)
        {
            TN db = new TN();
            MyOrder oldOrder = db.MyOrders.Where(en => en.orderno == order.orderno).FirstOrDefault();

            oldOrder.orderno = order.orderno;
            oldOrder.iduser = order.iduser;
            oldOrder.idmerc = order.idmerc;
            oldOrder.merc = order.merc;
            oldOrder.idspec = order.idspec;
            oldOrder.spec = order.spec;
            oldOrder.month = order.month;
            oldOrder.attmonth = order.attmonth;
            oldOrder.count = order.count;
            oldOrder.price = order.orderno;
            oldOrder.contact = order.contact;
            oldOrder.addr = order.addr;
            oldOrder.phone = order.phone;
            oldOrder.cretime = order.cretime;
            oldOrder.stime = order.stime;
            oldOrder.entime = order.entime;
            oldOrder.otype = order.otype;
            oldOrder.status = order.status;
            oldOrder.img = order.img;
            oldOrder.notes = order.notes;
            oldOrder.idc = order.idc;
            oldOrder.idc_img1 = order.idc_img1;
            oldOrder.idc_img2 = order.idc_img2;
            oldOrder.inuse = order.inuse;
          
            db.SaveChanges();
            return oldOrder;
        }
    }
}