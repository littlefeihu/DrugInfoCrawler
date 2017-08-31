using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    public class PingHelper
    {
        public static bool Ping(string ipAddress, int portNum)
        {
            IPAddress ip = IPAddress.Parse(ipAddress);
            try
            {
                IPEndPoint point = new IPEndPoint(ip, portNum);
                using (Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    sock.SendTimeout = 500;
                    sock.ReceiveTimeout = 500;
                    sock.Connect(point);
                    sock.Close();
                    return true;
                }
            }
            catch (SocketException)
            {
                return false;
            }

        }
    }
}
