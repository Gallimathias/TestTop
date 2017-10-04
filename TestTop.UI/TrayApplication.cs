using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using NamedPipeWrapper;
using TestTop.Core;

namespace TestTop.UI
{
    public class TrayApplication : IDisposable
    {
        private NotifyIcon notifyIcon;
        private ContextMenu trayMenu;
        NamedPipeClient<string[]> client;
        private string name;

        public event EventHandler OnExit;

        public TrayApplication()
        {
            client = new NamedPipeClient<string[]>("testTopPipe");
            client.Start();
            client.Connected += Client_Connected;
            client.ServerMessage += Client_ServerMessage;

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Switch", Switch);
            trayMenu.MenuItems.Add("Switch Back", SwitchBack);
            trayMenu.MenuItems.Add("Screnshot", Screenshot);
            trayMenu.MenuItems.Add("Restore Icons", RestoreIcons);
            trayMenu.MenuItems.Add("Save Desktop", SaveDesktop);
            trayMenu.MenuItems.Add("Exit", Exit);



            notifyIcon = new NotifyIcon()
            {
                Text = "TestTop",
                Icon = new Icon(SystemIcons.Application, 40, 40),
                ContextMenu = trayMenu
            };
        }

        private void Client_ServerMessage(NamedPipeConnection<string[], string[]> connection, string[] message)
        {
            if (message[0] == "CurrentDesktopName" && string.IsNullOrWhiteSpace(name))
                name = message[1];
        }

        private void Client_Connected(NamedPipeConnection<string[], string[]> connection)
        {
            client.PushMessage(new string[]{"IAmAlive", name ?? ""});

        }

        private void SwitchBack(object sender, EventArgs e)
        {
            client.PushMessage(new string[] { "SwitchBack" });
        }

        private void SaveDesktop(object sender, EventArgs e)
        {
            client.PushMessage(new string[] { "SaveDesktop", name });
        }

        private void RestoreIcons(object sender, EventArgs e)
        {
            client.PushMessage(new string[] { "RestoreIcons", name });
        }

        private void Screenshot(object sender, EventArgs e)
        {
            DoStuff();
        }

        private async void DoStuff()
        {
            throw new NotImplementedException();
        }

        internal void Switch(object sender, EventArgs e)
        {
            client.PushMessage(new string[] { "Switch" });
        }

        internal void Exit(object sender, EventArgs e)
        {
            client.Stop();
            Dispose();
            OnExit?.Invoke(this, e);
        }

        internal void Run()
        {
            notifyIcon.Visible = true;

        }

        public void Dispose()
        {
            notifyIcon.Visible = false;
            notifyIcon.Dispose();
            trayMenu.Dispose();
            notifyIcon = null;
            trayMenu = null;

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
