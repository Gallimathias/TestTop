using Nancy;
using Nancy.IO;
using Nancy.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Server
{
    static class Program
    {
        static void Main() => ServiceBase.Run(new ServiceBase[] { new TestTopService { } });
    }
    
}
