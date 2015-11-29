using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpProxy
{
    class Program
    {
        public static byte[] localhost;
        public static byte[] riothost;
        public static int diff;
        static void Main(string[] args)
        {
            localhost = Encoding.UTF8.GetBytes("Host: 127.0.0.1");//http header of our tweaked patcher
            riothost = Encoding.UTF8.GetBytes("Host: l3cdn.riotgames.com");//http header needed
            diff = Math.Abs(riothost.Length - localhost.Length);

            IPAddress ip = Dns.GetHostAddresses("l3cdn.riotgames.com")[0];
            Console.WriteLine("You can now start the clean patcher");
            SimpleProxy p = new SimpleProxy(new IPEndPoint(IPAddress.Loopback, 80), "l3cdn.riotgames.com", 80);

            Console.WriteLine("The end?");
            Console.ReadKey();
        }
    }
}
