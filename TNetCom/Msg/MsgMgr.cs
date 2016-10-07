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
    public class MsgMgr
    {

        /// <summary>
        /// 发送支付订单消息
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="otype"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static bool FinishPay(string orderno, int otype, TCom.EF.TN db)
        {
            TCom.EF.Msg mo = new TCom.EF.Msg();
            mo.idmsg = P.ID().ToString();
            mo.cretime = DateTime.Now;
            mo.status = 0;
            mo.orderno = orderno;
            mo.otype = otype;
            mo.type = MsgType.PayFinishOrder;
            mo.inuse = true;
            if (db == null)
            {
                using (db = new TCom.EF.TN())
                {
                    db.Msgs.Add(mo);
                    if (db.SaveChanges() > 0)
                    {
                        Send();
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

        public static bool Send()
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
