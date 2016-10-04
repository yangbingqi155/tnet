using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using TNet.Models.Order;
using TNet.Models.Pay;
using TNet.Models.Service.Com;
using WxPayAPI;

namespace TNet.Service.Pay
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“PayService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 PayService.svc 或 PayService.svc.cs，然后开始调试。
    [AspNetCompatibilityRequirementsAttribute(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]

    public class PayService : IPayService
    {
        public Result<PayResult> Create(PayData data)
        {
            Result<PayResult> result = new Result<PayResult>();
            result.Msg = "生成支付订单失败";
            if (data != null && !string.IsNullOrWhiteSpace(data.iduser) && !string.IsNullOrWhiteSpace(data.orderno))
            {
                try
                {
                    long _iduser = long.Parse(data.iduser);
                    long _orderno = long.Parse(data.orderno);
                    using (EF.TN db = new EF.TN())
                    {
                        EF.User u = db.Users.Where(m => m.iduser == _iduser && m.inuse == true).FirstOrDefault();
                        if (u != null)
                        {
                            EF.MyOrder o = db.MyOrders.Where(m => m.orderno == _orderno && m.inuse == true).FirstOrDefault();
                            if (o != null)
                            {
                                if (o.paystatus == PayStatus.WeiXin_CLOSED)
                                {
                                    o.status = OrderStatus.Close;
                                    db.SaveChanges();
                                    result.Msg = "订单已关闭";
                                }
                                else
                                {
                                    o.paystatus = PayStatus.WaitPay;
                                    if (o.payway != "weixin")
                                    {
                                        o.payway = "weixin";
                                        if (db.SaveChanges() <= 0)
                                        {
                                            result.Msg = "更新支付方式失败";
                                            return result;
                                        }

                                    }
                                    result.Data = new PayResult
                                    {
                                        order = JsApiPay.PayOrder(o, u)
                                    };
                                    result.Code = R.Ok;
                                }


                            }

                        }
                    }
                }
                catch (Exception)
                {
                    result.Code = R.Error;
                    result.Msg = "拉取订单出错";
                }
            }
            else
            {
                result.Msg = "不存在该订单";
            }
            return result;

        }

    }
}
