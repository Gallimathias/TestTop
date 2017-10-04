using NamedPipeWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTop.Core;

namespace TestTop.TestService
{
    public class MainService : IDisposable
    {
        public static HashSet<string> Clients;

        NamedPipeServer<string[]> server;
        CommandManager commandManager;

        public MainService()
        {
            commandManager = new CommandManager();
            server = new NamedPipeServer<string[]>("testTopPipe");
            Clients = new HashSet<string>();
            //server.PushMessage()
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;

        }

        private async void Server_ClientMessage(NamedPipeConnection<string[], string[]> connection, string[] message)
        {
            await commandManager.DispatchAsync(message[0], message.Skip(1).ToArray());
        }

        private void Server_ClientConnected(NamedPipeConnection<string[], string[]> conn)
        {
            Console.WriteLine($"Client Connected with id: {conn.Name}");
            if (!Clients.Contains(DesktopManager.CurrentDesktop.Name))
                Clients.Add(DesktopManager.CurrentDesktop.Name);
            conn.PushMessage(new string[] { "CurrentDesktopName", DesktopManager.CurrentDesktop.Name });
        }

        internal void OnStart(string[] args)
        {
            server.Start();
        }

        //Until it's service
        //internal void OnStop()
        //{
        //    server.Stop();
        //}

        public void Dispose()
        {
            server.Stop();
        }
    }
}
