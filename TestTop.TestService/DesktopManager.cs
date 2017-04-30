using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTop.Core;
using TestTop.Core.WinAPI;

namespace TestTop.TestService
{
    class DesktopManager
    {
        public List<Desktop> Desktops { get; set; }
        public IntPtr MainDesktopHandle { get; private set; }
        public Desktop CurrentDesktop { get; private set; }

        private Desktop startDesktop;

        public DesktopManager()
        {
            Desktops = new List<Desktop>();

            MainDesktopHandle = User32.GetDesktopWindow();
            startDesktop = new Desktop("Default", MainDesktopHandle, MainDesktopHandle);
            CurrentDesktop = startDesktop;
            File.WriteAllText(@"C:\Users\Public\Desktops\mainhandle.txt", MainDesktopHandle.ToString()); //TODO No hardcoded thinks

            GetDesktops();

        }

        private void Switch(string name)
        {
            startDesktop.Save();

            if (string.IsNullOrWhiteSpace(name))
                return;

            var desk = Desktops.FirstOrDefault(d => d.Name == name);

            if (desk == null)
            {
                desk = new Desktop(name, MainDesktopHandle);
                Desktops.Add(desk);
                GetDesktops();
            }

            desk.Show();
            //if( Process.GetProcessesByName("explorer.exe").FirstOrDefault() == null)
            desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            CurrentDesktop = desk;

        }

        public void GetDesktops()
        {
            IntPtr windowStation = User32.GetProcessWindowStation();
            bool result = User32.EnumDesktops(windowStation, DesktopEnumProc, IntPtr.Zero);
        }

        private bool DesktopEnumProc(string lpszDesktop, IntPtr lParam)
        {
            Desktops.Add(new Desktop(lpszDesktop, MainDesktopHandle)); ;
            return true;
        }


        public void SwitchBack() => Switch("Default");

    }
}
