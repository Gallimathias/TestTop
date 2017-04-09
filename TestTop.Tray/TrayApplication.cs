using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO.Pipes;
using System.IO;
using NamedPipeWrapper;

namespace TestTop.Tray
{
    public class TrayApplication : IDisposable
    {
        private NotifyIcon notifyIcon;
        private ContextMenu trayMenu;
        private ManualResetEvent resetEvent;
        NamedPipeClient<string> client;

        public TrayApplication()
        {
            resetEvent = new ManualResetEvent(false);
            client = new NamedPipeClient<string>("testTopPipe");
            client.Start();

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Switch", Switch);
            trayMenu.MenuItems.Add("Exit", Exit);
            

            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "TestTop";
            notifyIcon.Icon = new Icon(SystemIcons.Application, 40, 40);
            notifyIcon.ContextMenu = trayMenu;
        }

        internal void Switch(object sender, EventArgs e)
        {
            client.PushMessage("Hallo Welt");
        }

        internal void Exit(object sender, EventArgs e)
        {
            client.Stop();
            resetEvent.Set();
        }

        internal void Run()
        {
            notifyIcon.Visible = true;
            resetEvent.WaitOne();
            
        }

        public void Dispose()
        {
            notifyIcon.Dispose();
            trayMenu.Dispose();
            notifyIcon = null;
            trayMenu = null;
            resetEvent.Dispose();
            resetEvent = null;

            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
