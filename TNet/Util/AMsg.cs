using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using Util;

namespace TNet.Util
{
    public class AMsg
    {
        public static bool SendOrderFinishPay(string orderno, int otype, EF.TN db)
        {
            EF.Msg mo = new EF.Msg();
            mo.idmsg = Pub.ID().ToString();
            mo.cretime = DateTime.Now;
            mo.status = 0;
            mo.orderno = orderno;
            mo.otype = otype;
            mo.type = 1;
            mo.inuse = true;
            if (db == null)
            {
                using (db = new EF.TN())
                {
                    db.Msgs.Add(mo);
                    if (db.SaveChanges() > 0)
                    {
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

                //UdpClient udp = new UdpClient(host);
                byte[] data = new byte[] { 1 };
                int c = server.SendTo(data, data.Length, SocketFlags.None, host);
                if (c >= 1)
                {
                    return true;
                }
            }
            catch (Exception e)
            {

            }

            return false;

        }
    }
}