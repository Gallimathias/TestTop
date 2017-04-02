using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Server
{
    public static class Server
    {
        internal static void Start(int port)
        {
            var url = new Uri("http://localhost:"+port);
            using (var host = new NancyHost(url))
            {
                host.Start();
                Console.WriteLine("Släuft");
                Console.ReadKey();
            }
        }


        internal static void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
