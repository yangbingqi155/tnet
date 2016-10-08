using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TCom.Com;

namespace TCom.Msg
{
    //发送消息
    public sealed class MsgMgr
    {

        /// <summary>
        /// 发送支付订单消息
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="otype"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool PostFinishPay(string orderno, int otype, TCom.EF.TN db)
        {
            return crateMsg("", "", MsgType.PostPayFinishOrder, orderno, otype, db);

        }



        public static bool FinishPay(string idweixin, EF.MyOrder mo, string iduser, string uname, TCom.EF.TN db)
        {
            int otype = mo.otype != null ? mo.otype.Value : 0;
            JObject jo = new JObject();
            jo["touser"] = idweixin;
            jo["template_id"] = "_5rsT-d9H1iLDHr8B7IN5IYo4QftrnNxNofEeTn4EyI";

            jo["url"] = "http://app.i5shang.com/tnet/order/detail/" + mo.orderno + "?iduser=" + iduser;
            JObject jdo = new JObject();
            jdo["first"] = getJobj(mo.merc + "(" + mo.spec + ")");
            jdo["tradeDateTime"] = getJobj(mo.cretime != null ? mo.cretime.Value.ToString("MM月dd日 HH时mm分") : "无");
            jdo["orderType"] = getJobj((otype == 1) ? "宽带" : "报装");
            jdo["customerInfo"] = getJobj(uname);
            jdo["orderItemName"] = getJobj("交易金额");
            jdo["orderItemData"] = getJobj(mo.totalfee + "");
            jdo["remark"] = getJobj("欢迎再次购买");
            jo["data"] = jdo;

            return crateMsg(idweixin, jo.ToString(), MsgType.PayFinishOrder, mo.orderno + "", otype, db);

        }



        public static bool crateMsg(string idweixin, string msg, int msgType, string orderno, int otype, TCom.EF.TN db)
        {
            TCom.EF.Msg mo = new TCom.EF.Msg();
            mo.idweixin = idweixin;
            mo.idmsg = P.ID().ToString();
            mo.msg1 = msg;
            mo.cretime = DateTime.Now;
            mo.status = 0;
            mo.orderno = orderno;
            mo.otype = otype;
            mo.type = msgType;
            mo.inuse = true;
            if (db == null)
            {
                using (db = new TCom.EF.TN())
                {
                    db.Msgs.Add(mo);
                    if (db.SaveChanges() > 0)
                    {
                        Post();
                        return true;
                    }
                }
            }
            else
            {
                db.Msgs.Add(mo);
            }
            return true;
        }




        public static JObject getJobj(string value, string color = null)
        {
            JObject jio = new JObject();
            jio["value"] = value;
            if (!string.IsNullOrWhiteSpace(color))
            {
                jio["color"] = color;
            }
            return jio;
        }



        public static bool Post()
        {
            try
            {
                IPAddress ip = IPAddress.Parse("127.0.0.1");
                IPEndPoint host = new IPEndPoint(ip, 6699);
                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                //server.SetSocketOption()
                byte[] data = new byte[] { 1 };
                int c = server.SendTo(data, data.Length, SocketFlags.None, host);
                if (c >= 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {

            }

            return false;

        }
    }
}
