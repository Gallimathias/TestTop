using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace TestTop.TestService
{
    class Program
    {
        static void Main(string[] args)
        {

            var service = new MainService();
            var handle = new ManualResetEvent(false);
            service.OnStart(args);
            Console.ReadKey();
            service.OnStop();
        }
    }
}
