using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TNet.EF;
using Util;

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

        public int count { get; set; }

        public int month { get; set; }

        public int attmonth { get; set; }


        public string contact { get; set; }


        public string addr { get; set; }


        public string phone { get; set; }


        public string notes { get; set; }

        public string img { get; set; }

        public string otype { get; set; }

        public string idc { get; set; }

        public string idc_img1 { get; set; }

        public string idc_img2 { get; set; }

        public EF.MyOrder getData()
        {
            MyOrder o = new MyOrder();
            o.orderno = Pub.ID();
            o.idmerc = this.idmerc;
            o.iduser = this.iduser;
            o.idmerc = this.idmerc;
            o.merc = this.merc;
            o.idspec = this.idspec;
            o.spec = this.spec;
            o.month = this.month;
            o.attmonth = this.attmonth;
            o.price = this.price;
            o.count = this.count;
            o.contact = this.contact;
            o.addr = this.addr;
            o.phone = this.phone;
            o.cretime = DateTime.Now;
            o.stime = DateTime.Now;
            o.entime = DateTime.Now.AddMonths((this.month + this.attmonth) * o.count);
            if (this.otype == "merc")
            {
                o.otype = OrderType.Merc;
            }
            else if (this.otype == "setup")
            {
                o.otype = OrderType.Setup;
            }
            o.idc = this.idc;
            o.idc_img1 = this.idc_img1;
            o.idc_img2 = this.idc_img2;
            o.status = OrderStatus.WaitPay;
            o.notes = this.notes;
            o.img = this.img;
            o.inuse = true;
            return o;
        }
    }
}