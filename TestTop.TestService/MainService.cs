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
        CommandManager commandManager;

        public MainService()
        {

            server = new NamedPipeServer<string>("testTopPipe");
            server.ClientConnected += Server_ClientConnected;
            server.ClientMessage += Server_ClientMessage;
            
            commandManager = new CommandManager();    

        }
        
        private void Server_ClientMessage(NamedPipeConnection<string, string> connection, string message)
        {
            commandManager.DispatchAsync(message, message.Split());
            

            //OnSwitch
            /*
             startDesktop.Save();

            if (string.IsNullOrWhiteSpace(comboBox.Text))
                return;

            var desk = Desktops.FirstOrDefault(d => d.Name == comboBox.Text);

            if (desk == null)
            {
                desk = new Desktop(comboBox.Text, MainDesktopHandle, CreateGraphics());
                Desktops.Add(desk);
                GetDesktops();
            }

            desktopControl1.Add(CurrentDesktop.Name, CurrentDesktop.TakeScreenshot());
            desk.Show();
            desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            //desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            CurrentDesktop = desk;
            */
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
