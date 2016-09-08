using System;
using System.Collections.Generic;
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
                    o.contact = data.contact;
                    o.addr = data.addr;
                    o.phone = data.phone;
                    o.cretime = DateTime.Now;
                    o.stime = DateTime.Now;
                    o.entime = DateTime.Now.AddMonths(data.month + data.attmonth);
                    o.otype = OrderType.Merc;
                    o.status = OrderStatus.WaitPay;
                    o.notes = data.notes;
                    o.inuse = true;
                    db.MyOrders.Add(o);
                    if (db.SaveChanges() > 0)
                    {
                        result.Data = new CreateOrderResult();
                        result.Data.orderno = o.orderno;
                        result.Code = R.Ok;

                    }else
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
    }
}
