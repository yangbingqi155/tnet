using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCom.Msg;
using TNetService.BLL;
using Util;

namespace TNetService
{
    partial class TService : ServiceBase
    {

        static int port = 6699;
        static TThread t;
        static TNetThread tnet;
        public TService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            stopt();
            t = new TThread();
            tnet = new TNetThread();
        }


        protected override void OnStop()
        {
            stopt();
        }

        private void stopt()
        {
            if (t != null)
            {
                t.Stop();
            }
            t = null;
            if (tnet != null)
            {
                tnet.Stop();
            }
            tnet = null;

        }

        private class TThread
        {
            public bool run;
            Thread t;
            Socket service;
            public TThread()
            {
                run = true;
                t = new Thread(startService);
                t.Start();
            }

            public void startService()
            {
                //得到本机IP，设置TCP端口号         
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
                service = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //绑定网络地址
                service.Bind(ip);
                byte[] buffer = new byte[1024];
                while (run)
                {
                    try
                    {
                        EndPoint point = new IPEndPoint(IPAddress.Any, 0);
                        int length = service.ReceiveFrom(buffer, ref point);
                        Pub.e("UDP=" + length);
                    }
                    catch (Exception)
                    {

                    }
                    if (run && tnet != null)
                    {
                        tnet.Set();
                    }
                }

            }



            public void Stop()
            {
                run = false;
                try
                {
                    service.Shutdown(SocketShutdown.Both);
                    service.Close();

                }
                catch (Exception)
                {
                }
                service = null;

            }
        }



        private class TNetThread
        {
            public bool run;
            Thread t;
            public ManualResetEvent _event = new ManualResetEvent(false);
            public object lockObj = new object();
            public TNetThread()
            {
                run = true;
                t = new Thread(startService);
                t.Start();
            }

            public void startService()
            {
                //Thread.Sleep(1000 * 60);
                while (run)
                {
                    try
                    {
                        using (TCom.EF.TN db = new TCom.EF.TN())
                        {
                            List<TCom.EF.Msg> ms = db.Msgs.Where(m => m.inuse == true && m.status != 1).ToList();
                            var mus = (from muo in db.ManageUsers
                                       join uo in db.Users on muo.idweixin equals uo.idweixin
                                       where (muo.idweixin != null && muo.idweixin.Trim() != string.Empty && muo.inuse == true && uo.inuse == true)
                                       select new
                                       {
                                           idweixin = muo.idweixin,
                                           name = uo.name
                                       }).ToList();
                            if (ms != null && mus != null)
                            {
                                for (int i = 0; i < ms.Count; i++)
                                {
                                    TCom.EF.Msg m = ms[i];

                                    if (m.type == MsgType.PostPayFinishOrder)//订单支付完成，等待派发通知
                                    {
                                        long _orderno = long.Parse(m.orderno);
                                        TCom.EF.MyOrder mo = db.MyOrders.Where(moo => moo.orderno == _orderno).FirstOrDefault();

                                        if (mo != null)
                                        {
                                            var uo = (from u in db.Users
                                                      where (u.iduser == mo.iduser)
                                                      select new
                                                      {
                                                          u.iduser,
                                                          u.name
                                                      }).FirstOrDefault();
                                            if (uo != null)
                                            {
                                                for (int j = 0; j < mus.Count; j++)
                                                {
                                                    var mu = mus[j];
                                                    JObject jo = new JObject();
                                                    jo["touser"] = mu.idweixin;
                                                    jo["template_id"] = "_5rsT-d9H1iLDHr8B7IN5IYo4QftrnNxNofEeTn4EyI";

                                                    jo["url"] = "http://app.i5shang.com/tnet/order/detail/" + mo.orderno + "?iduser=" + uo.iduser;
                                                    JObject jdo = new JObject();
                                                    jdo["first"] = getJobj(mo.merc + "(" + mo.spec + ")");
                                                    jdo["tradeDateTime"] = getJobj(DateTime.Now.ToString("MM月dd日 HH时mm分"));
                                                    jdo["orderType"] = getJobj((m.otype == 1) ? "宽带" : "报装");
                                                    jdo["customerInfo"] = getJobj(uo.name);
                                                    jdo["orderItemName"] = getJobj("交易金额");
                                                    jdo["orderItemData"] = getJobj(mo.totalfee + "");
                                                    jdo["remark"] = getJobj("欢迎再次购买");
                                                    jo["data"] = jdo;

                                                    TCom.EF.Msg mso = new TCom.EF.Msg();
                                                    mso.idmsg = Pub.ID().ToString();
                                                    mso.idweixin = mu.idweixin;
                                                    mso.msg1 = jo.ToString();
                                                    mso.cretime = DateTime.Now;
                                                    mso.status = 1;
                                                    mso.orderno = m.orderno;
                                                    mso.otype = m.otype;
                                                    mso.type = MsgType.PayFinishOrder;
                                                    mso.inuse = true;
                                                    db.Msgs.Add(mso);
                                                    Pub.e("sendMsg-data:" + mso.msg1 + "-lg=" + mso.msg1.Length);
                                                    Msg.sendMsg(mso.msg1);
                                                }

                                                m.status = 1;
                                                //db.Msgs.Add(m);
                                                if (db.SaveChanges() > 0)
                                                {
                                                    Pub.e("Msg-SaveChanges:" + 1);
                                                }
                                                else
                                                {
                                                    Pub.e("Msg-SaveChanges:" + 0);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        ////ohYxpv5NBMEpBZ_d8Q3K2VYoF70U axx
                        ////ohYxpv7EviImnLfanp8nulC8nsDU f10
                        //string msg = "{\r\n\"touser\":\"ohYxpv7EviImnLfanp8nulC8nsDU\",\r\n";
                        //msg += "\"template_id\":\"i20N5RdW4MZqpL8NvC368ZTuksa0LTUnOi8RHc5gJy0\",\r\n";
                        //msg += "\"url\":\"http://weixin.qq.com/download\",\r\n";

                        //msg += "\"data\":{ \r\n";
                        //msg += "\"name\": {\r\n";
                        //msg += "\"value\":\"黄先生\"\r\n}\r\n}\r\n";
                        //msg += "}";

                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var item in e.EntityValidationErrors)
                        {
                            foreach (var io in item.ValidationErrors)
                            {
                                Pub.e("db-error=" + e.InnerException + "===" + e.Message + "--" + io.ErrorMessage);
                            }

                        }

                    }
                    catch (Exception e)
                    {
                        Pub.e("error=" + e.InnerException + "_+_+_+_+_+" + e.Message);
                        //throw e;
                    }
                    _event.Reset();
                    Pub.e("WaitOne=" + DateTime.Now);
                    _event.WaitOne(1000 * 60 * 10);
                    Pub.e("WaitOne-f=" + DateTime.Now);
                }
                _event = null;
            }

            private JObject getJobj(string value, string color = null)
            {
                JObject jio = new JObject();
                jio["value"] = value;
                if (!string.IsNullOrWhiteSpace(color))
                {
                    jio["color"] = color;
                }
                return jio;
            }
            public void Set()
            {
                _event.Set();
            }

            public void Stop()
            {
                run = false;
                Set();
            }
        }
    }
}
