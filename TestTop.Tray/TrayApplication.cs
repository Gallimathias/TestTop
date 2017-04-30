using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using NamedPipeWrapper;

namespace TestTop.Tray
{
    public class TrayApplication : IDisposable
    {
        private NotifyIcon notifyIcon;
        private ContextMenu trayMenu;
        NamedPipeClient<string> client;

        public event EventHandler OnExit;

        public TrayApplication()
        {
            client = new NamedPipeClient<string>("testTopPipe");
            client.Start();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Switch", Switch);
            trayMenu.MenuItems.Add("Exit", Exit);



            notifyIcon = new NotifyIcon()
            {
                Text = "TestTop",
                Icon = new Icon(SystemIcons.Application, 40, 40),
                ContextMenu = trayMenu
            };
        }

        internal void Switch(object sender, EventArgs e)
        {
            client.PushMessage("Hallo Welt");
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
