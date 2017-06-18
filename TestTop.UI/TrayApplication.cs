using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using NamedPipeWrapper;

namespace TestTop.UI
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
            trayMenu.MenuItems.Add("Screnshot", Screenshot);
            trayMenu.MenuItems.Add("Exit", Exit);



            notifyIcon = new NotifyIcon()
            {
                Text = "TestTop",
                Icon = new Icon(SystemIcons.Application, 40, 40),
                ContextMenu = trayMenu
            };
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
