using NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.TestService
{
    public class MainService
    {
        NamedPipeServer<string> server;
        public MainService()
        {

            server = new NamedPipeServer<string>("testTopPipe");
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;
            

        }
        
        private void Server_ClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
            Console.WriteLine($"Message from {connection.Id}: {message}");
         
        }

        private void Server_ClientConnected(NamedPipeConnection<string, string> conn)
        {
            Console.WriteLine($"Client Connected with id: {conn.Id}");
        }

        internal void OnStart(string[] args)
        {
            server.Start();
        }

        internal void OnStop()
        {
            server.Stop();
        }
    }
}
