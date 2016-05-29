using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Linq;

namespace LoLPatcherProxy
{
    public class Program
    {
        public static byte[] localhost;
        public static byte[] riothost;
        public static int diff;
        public static int PORT_NUMBER;

        public static MainForm mf;

        [STAThread]
        static void Main(string[] args)
        {
            if (!Directory.Exists("httpd"))
                Directory.CreateDirectory("httpd");

            PORT_NUMBER = FreeTcpPort();
            localhost = Encoding.UTF8.GetBytes("Host: 127.0.0.1:" + PORT_NUMBER);//http header of our tweaked patcher
            riothost = Encoding.UTF8.GetBytes("Host: l3cdn.riotgames.com");//http header needed
            diff = Math.Abs(riothost.Length - localhost.Length);

            Console.WriteLine("Configure the patcher");
            Application.EnableVisualStyles();
            Application.Run(mf = new MainForm());

            if (!Directory.Exists("httpd"))
            {
                MessageBox.Show("Error", "No patch specified, try again.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }


            if (File.Exists("lol.launcher.admin.exe"))
            {
                try
                {
                    Process.Start("lol.launcher.admin.exe");
                }
                catch
                {
                    //UAC failure... Let's try again?
                    if (File.Exists("lol.launcher.exe"))
                    {
                        try
                        {
                            Process.Start("lol.launcher.exe");
                        }
                        catch
                        {
                            //screw this
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You have to manually start the patcher (lol.launcher.exe)");
                            Console.ResetColor();
                        }
                    }
                }
            }

            SimpleProxy p = new SimpleProxy(new IPEndPoint(IPAddress.Loopback, PORT_NUMBER), "l3cdn.riotgames.com", 80);

            Console.WriteLine("The end?");
            Console.ReadKey();
        }

        public static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}


//Config

namespace ManifestManager
{
    public class Program
    {
        public static string Realm = "live";
        public static string Region = "EUNE";
        public static string API_BASE { get { return $"http://l3cdn.riotgames.com/releases/{Program.Realm.ToLower()}/"; } }
    }

}