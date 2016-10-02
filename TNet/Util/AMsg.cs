using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace TNet.Util
{
    public class AMsg
    {
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