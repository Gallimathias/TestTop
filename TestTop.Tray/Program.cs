using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Tray
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            var app = new TrayApplication();
            app.Run();
            app.Dispose();
        }
    }
}
