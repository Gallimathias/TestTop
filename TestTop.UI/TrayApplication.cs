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
        public string DesktopName { get; private set; }

        private NotifyIcon notifyIcon;
        private ContextMenu trayMenu;
        private NamedPipeClient<string[]> client;

        public event EventHandler OnExit;

        public TrayApplication()
        {
            client = new NamedPipeClient<string[]>("testTopPipe");
            client.Connected += Client_Connected;
            client.ServerMessage += Client_ServerMessage;
            client.Start();


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

        public void Dispose()
        {
            notifyIcon.Visible = false;
            notifyIcon?.Dispose();
            notifyIcon = null;

            trayMenu?.Dispose();
            trayMenu = null;

            client?.Stop();
            client = null;

            GC.Collect();
            GC.SuppressFinalize(this);
        }

        internal void Switch(object sender, EventArgs e) => PushMessage("Switch");

        internal void Exit(object sender, EventArgs e)
        {
            client.Stop();
            Dispose();
            OnExit?.Invoke(this, e);
        }

        internal void Run() => notifyIcon.Visible = true;

        private void Client_ServerMessage(NamedPipeConnection<string[], string[]> connection, string[] message)
        {
            if (message[0] == "CurrentDesktopName" && string.IsNullOrWhiteSpace(DesktopName))
            {
                DesktopName = message[1];
                PushMessage("NewDesktopSwitch", DesktopName, new DesktopHelper().DesktopHandle.ToString());
            }
        }

        private void Client_Connected(NamedPipeConnection<string[], string[]> connection) => PushMessage("IAmAlive", DesktopName ?? "");

        private void SwitchBack(object sender, EventArgs e) => PushMessage("SwitchBack");

        private void SaveDesktop(object sender, EventArgs e) => PushMessage("SaveDesktop", DesktopName);

        private void RestoreIcons(object sender, EventArgs e) => PushMessage("RestoreIcons", DesktopName);

        private void Screenshot(object sender, EventArgs e)
        {
            DoStuff();
        }

        private void DoStuff()
        {
            throw new NotImplementedException();
        }

        private void PushMessage(params string[] args) => client.PushMessage(args);
    }
}
