using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using NamedPipeWrapper;

namespace TestTop
{
    public partial class MainService : ServiceBase
    {
        NamedPipeServer<string> server;
        public MainService()
        {
            InitializeComponent();
            server = new NamedPipeServer<string>("testTopPipe");
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;

            
        }

        private void Server_ClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
        }

        private void Server_ClientConnected(NamedPipeConnection<string, string> conn)
        {
            
        }

        protected override void OnStart(string[] args)
        {
            server.Start();
        }

        protected override void OnStop()
        {
            server.Stop();
        }
    }
}
