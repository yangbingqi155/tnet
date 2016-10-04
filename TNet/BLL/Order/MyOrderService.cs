using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using TCom.EF;
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

        public static List<MyOrder> GetOrderByFilter(DateTime? startOrDate, DateTime? endOrDate, int orderTypes = 0, int orderStatus = 0, long orderNo = 0, long userNo = 0) {
            TN db = new TN();
            
            return db.MyOrders.Where(en =>
                (startOrDate.Value == null || SqlFunctions.DateDiff("dd",startOrDate.Value,en.cretime)>=0 )
                && (endOrDate.Value == null || SqlFunctions.DateDiff("dd", endOrDate.Value, en.cretime) <= 0)
                && (orderTypes == 0 || orderTypes == en.otype)
                && (orderStatus == 0 || orderStatus == en.status)
                || orderNo == en.orderno
                && (userNo == 0 || userNo==en.iduser)
            ).ToList();
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