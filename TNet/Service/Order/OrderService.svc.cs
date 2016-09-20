﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TNet.EF;
using TNet.Models.Order;
using TNet.Models.Service.Com;
using Util;

namespace TNet.Service.Order
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“OrderService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 OrderService.svc 或 OrderService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class OrderService : IOrderService
    {
        public Result<CreateOrderResult> Create(CreateOrderData data)
        {
            Result<CreateOrderResult> result = new Result<CreateOrderResult>();
            try
            {
                using (TN db = new TN())
                {
                    MyOrder o = new MyOrder();
                    o.orderno = Pub.ID();
                    o.idmerc = data.idmerc;
                    o.iduser = data.iduser;
                    o.idmerc = data.idmerc;
                    o.merc = data.merc;
                    o.idspec = data.idspec;
                    o.spec = data.spec;
                    o.month = data.month;
                    o.attmonth = data.attmonth;
                    o.price = data.price;
                    o.count = data.count;
                    o.contact = data.contact;
                    o.addr = data.addr;
                    o.phone = data.phone;
                    o.cretime = DateTime.Now;
                    o.stime = DateTime.Now;
                    o.entime = DateTime.Now.AddMonths((data.month + data.attmonth) * o.count);
                    o.otype = OrderType.Merc;
                    o.status = OrderStatus.WaitPay;
                    o.notes = data.notes;
                    o.img = data.img;
                    o.inuse = true;
                    db.MyOrders.Add(o);
                    MyOrderPress s = new MyOrderPress();
                    s.idpress = Pub.ID().ToString();
                    s.orderno = o.orderno.ToString();
                    s.status = OrderStatus.Create;
                    s.statust = OrderStatus.get(s.status).text;
                    s.oper = "用户";
                    s.inuse = true;
                    s.cretime = DateTime.Now;
                    db.MyOrderPresses.Add(s);


                    s = new MyOrderPress();
                    s.idpress = Pub.ID().ToString();
                    s.orderno = o.orderno.ToString();
                    s.status = OrderStatus.Confirm;
                    s.statust = OrderStatus.get(s.status).text;
                    s.oper = "系统";
                    s.inuse = true;
                    s.cretime = DateTime.Now;
                    db.MyOrderPresses.Add(s);
                    if (db.SaveChanges() > 0)
                    {
                        result.Data = new CreateOrderResult();
                        result.Data.orderno = o.orderno;
                        result.Code = R.Ok;

                    }
                    else
                    {
                        result.Code = R.Error;
                    }
                    // result.Data = m;
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
                result.Msg = "出现异常";
            }
            return result;
        }



        public Result<OrderListInfo> List(string iduser)
        {
            Result<OrderListInfo> result = new Result<OrderListInfo>();
            try
            {
                if (!string.IsNullOrWhiteSpace(iduser))
                {
                    long _iduser = long.Parse(iduser);
                    using (TN db = new TN())
                    {
                        result.Data = new OrderListInfo()
                        {
                            Order = db.MyOrders.Where(m => m.inuse == true && m.iduser == _iduser).OrderByDescending(m => m.cretime).ToList()
                        };
                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception)
            {
                result.Code = R.Error;
                result.Msg = "出现异常";
            }
            return result;
        }


        public Result<bool> Cancel(string iduser, string orderno)
        {
            Result<bool> result = new Result<bool>();
            result.Code = R.Error;
            result.Msg = "取消订单失败";
            try
            {
                if (!string.IsNullOrWhiteSpace(iduser) && !string.IsNullOrWhiteSpace(orderno))
                {
                    long _iduser = long.Parse(iduser);
                    long _orderno = long.Parse(orderno);
                    using (TN db = new TN())
                    {
                        using (DbContextTransaction t = db.Database.BeginTransaction())
                        {

                            int r = db.Database.ExecuteSqlCommand("update myorder set status = {0} where iduser = {1} and orderno = {2}", OrderStatus.Cancel, _iduser, _orderno);
                           
                            if (r > 0)
                            {
                                r = db.Database.ExecuteSqlCommand("insert into MyOrderPress (idpress,orderno,status,statust,cretime,oper,inuse) values({0},{1},{2},{3},{4},{5},1)", Pub.ID(), orderno, OrderStatus.Cancel, OrderStatus.get(OrderStatus.Cancel).text,DateTime.Now, "用户");
                                if(r > 0)
                                {
                                    t.Commit();
                                    result.Data = true;
                                    result.Code = R.Ok;
                                    return result;
                                }
                                
                            }
                            t.Rollback();
                        }

                    }
                }
            }
            catch (Exception e)
            {
                result.Code = R.Error;
                result.Msg = "取消订单出现异常"+e.InnerException;
            }
            return result;
        }

        public Result<MyOrderDetail> Detail(string iduser, string orderno)
        {
            Result<MyOrderDetail> result = new Result<MyOrderDetail>();
            try
            {
                if (!string.IsNullOrWhiteSpace(iduser) && !string.IsNullOrWhiteSpace(orderno))
                {
                    long _iduser = long.Parse(iduser);
                    long _orderno = long.Parse(orderno);
                    using (TN db = new TN())
                    {
                        EF.MyOrder o = db.MyOrders.Where(m => m.inuse == true && m.iduser == _iduser && m.orderno == _orderno).FirstOrDefault();
                        result.Data = new MyOrderDetail();
                        result.Data.Order = o;
                        result.Data.Presses = db.MyOrderPresses.Where(m => m.inuse == true && m.orderno == orderno).ToList();
                        result.Code = R.Ok;
                    }
                }
            }
            catch (Exception e)
            {
                result.Code = R.Error;
                result.Msg = "出现异常"+e.InnerException;
            }
            return result;
        }
    }
}
