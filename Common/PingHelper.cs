using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace DrugInfo.Crawler
{
    public class PingHelper
    {
        public static bool Ping(System.Net.IPAddress address)
        {
            //Ping 实例对象;
            Ping pingSender = new Ping();
            //ping选项;
            PingOptions options = new PingOptions();
            options.DontFragment = true;
            string data = "ping test data";
            byte[] buf = Encoding.ASCII.GetBytes(data);

            PingReply reply = pingSender.Send(address, 120, buf, options);

            //Console.WriteLine("主机地址::" + reply.Address);
            //Console.WriteLine("往返时间::" + reply.RoundtripTime);
            //Console.WriteLine("生存时间TTL::" + reply.Options.Ttl);
            //Console.WriteLine("缓冲区大小::" + reply.Buffer.Length);
            //Console.WriteLine("数据包是否分段::" + reply.Options.DontFragment);

            return reply.Status == IPStatus.Success;

        }
    }
}
