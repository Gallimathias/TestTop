using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTop.Tray
{
    static class Program
    {
        private static TrayApplication trayApplication;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            trayApplication = new TrayApplication();
            trayApplication.OnExit += (s, e) => Application.Exit();
            trayApplication.Run();
            Application.Run();
            
        }
    }
}
