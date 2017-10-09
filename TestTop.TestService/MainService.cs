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

        private NamedPipeServer<string[]> server;
        private CommandManager commandManager;

        public MainService()
        {
            commandManager = new CommandManager();
            server = new NamedPipeServer<string[]>("testTopPipe");
            Clients = new HashSet<string>();
            //server.PushMessage()
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;

        }

        public void Dispose() => server.Stop();


        internal void OnStart(string[] args) => server.Start();

        //Until it's service
        //internal void OnStop() =>  server.Stop();

        private async void Server_ClientMessage(NamedPipeConnection<string[], string[]> connection, string[] message)
        {
            var result = await commandManager.DispatchAsync(message[0], message.Skip(1).ToArray());

            if (!result)
                connection.PushMessage(new[] { "Error" });
        }

        
        private void Server_ClientConnected(NamedPipeConnection<string[], string[]> connection)
        {
            Console.WriteLine($"Client Connected with id: {connection.Name}");

            if (!Clients.Contains(DesktopManager.CurrentDesktop.Name))
                Clients.Add(DesktopManager.CurrentDesktop.Name);

            connection.PushMessage(new[] { "CurrentDesktopName", DesktopManager.CurrentDesktop.Name });
        }

    }
}
