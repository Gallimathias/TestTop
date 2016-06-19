using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Server
{
    public partial class TestTopService : ServiceBase
    {

        public TestTopService()
        {
            Run(this);

        }

        protected override void OnStart(string[] args)
        {
            Server.Start(4344);
        }

        protected override void OnStop()
        {
            Server.Stop();
        }
    }
}
