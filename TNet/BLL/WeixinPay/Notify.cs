using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;
using TNet.EF;
using TNet.Models.Order;
using Util;
using TNet.Models.Pay;

namespace WxPayAPI
{
    /// <summary>
    /// 回调处理基类
    /// 主要负责接收微信支付后台发送过来的数据，对数据进行签名验证
    /// 子类在此类基础上进行派生并重写自己的回调处理过程
    /// </summary>
    public class Notify
    {

        /// <summary>
        /// 接收从微信支付后台发送过来的数据并验证签名
        /// </summary>
        /// <returns>微信支付后台返回的数据</returns>
        public WxPayData GetData()
        {
            WxPayData data = null;
            //接收从微信后台POST过来的数据
            string orderno = HttpContext.Current.Request.QueryString["orderno"];
            if (string.IsNullOrWhiteSpace(orderno))
            {


                System.IO.Stream s = HttpContext.Current.Request.InputStream;
                int count = 0;
                byte[] buffer = new byte[1024];
                StringBuilder builder = new StringBuilder();
                while ((count = s.Read(buffer, 0, 1024)) > 0)
                {
                    builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
                }
                s.Flush();
                s.Close();
                s.Dispose();

                //Log.Info(this.GetType().ToString(), "Receive data from WeChat : " + builder.ToString());

                //转换数据格式并验证签名
                data = new WxPayData();
                try
                {
                    data.FromXml(builder.ToString());
                }
                catch (WxPayException ex)
                {
                    returnPayPressResult("FAIL", ex.Message);
                    data = null;
                }
            }
            else
            {
                data = OrdeQquery(orderno);
            }

            return data;
        }

        public WxPayData OrdeQquery(string orderno)
        {
            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", orderno);
            return WxPayApi.OrderQuery(data);
        }

        //派生类需要重写这个方法，进行不同的回调处理
        public void Press()
        {
            WxPayData data = GetData();
            if (data != null)
            {
                string return_code = data.GetValue("return_code") + "";
                if (return_code == "SUCCESS")
                {
                    string orderno = data.GetValue("out_trade_no") + "";
                    string openid = data.GetValue("openid") + "";

                    if (!string.IsNullOrWhiteSpace(orderno) && !string.IsNullOrWhiteSpace(openid))
                    {
                        string trade_state = data.GetValue("trade_state") + "";
                        long _orderno = long.Parse(orderno);
                        DateTime paytime = DateTime.Now;
                        try
                        {
                            string time_end = data.GetValue("time_end") + "";
                            if (string.IsNullOrWhiteSpace(time_end))
                            {
                                paytime = DateTime.ParseExact(time_end, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        using (TN db = new TN())
                        {

                            TNet.EF.User u = db.Users.Where(m => m.idweixin == openid && m.inuse == true).FirstOrDefault();
                            if (u != null)
                            {
                                TNet.EF.MyOrder o = db.MyOrders.Where(m => m.orderno == _orderno && m.inuse == true).FirstOrDefault();
                                if (o != null)
                                {
                                    bool ok = false;
                                    if (o.status == OrderStatus.WaitPay
                                         && o.paystatus != PayStatus.WeiXin_SUCCESS &&
                                         o.paystatus != PayStatus.WeiXin_CLOSED)
                                    {
                                        if (trade_state == PayStatus.WeiXin_SUCCESS)
                                        {
                                            o.status = OrderStatus.PayFinish;
                                            MyOrderPress s = new MyOrderPress();
                                            s.idpress = Pub.ID().ToString();
                                            s.orderno = orderno;
                                            s.status = OrderStatus.PayFinish;
                                            s.statust = OrderStatus.get(s.status).text;
                                            s.oper = u.name;
                                            s.inuse = true;
                                            s.cretime = paytime;
                                            db.MyOrderPresses.Add(s);
                                        }else
                                        {
                                            o.status = OrderStatus.WaitPay;
                                        }
                                        o.paystatus = trade_state;
                                        if (db.SaveChanges() > 0)
                                        {
                                            ok = true;
                                        }
                                    }
                                    else
                                    {
                                        ok = true;
                                    }
                                    if (ok)
                                    {
                                        returnPayPressResult("SUCCESS", "OK");
                                        return;
                                    }

                                }
                            }

                        }

                    }

                }

            }
            returnPayPressResult("FAIL");

        }


        private void returnPayPressResult(string code, string msg = "")
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", code);
            res.SetValue("return_msg", msg);
            HttpContext.Current.Response.Write(res.ToXml());
        }
    }
}