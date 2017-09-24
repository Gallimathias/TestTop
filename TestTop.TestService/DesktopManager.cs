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
    static class DesktopManager
    {
        public static List<Desktop> Desktops { get; set; }
        public static IntPtr MainDesktopHandle { get; private set; }
        public static Desktop CurrentDesktop { get; private set; }

        private static Desktop startDesktop;

        static DesktopManager()
        {
            Desktops = new List<Desktop>();

            MainDesktopHandle = User32.GetDesktopWindow();
            startDesktop = new Desktop("Default", MainDesktopHandle, MainDesktopHandle);
            CurrentDesktop = startDesktop;
            File.WriteAllText(@"C:\Users\Public\mainhandle.txt", MainDesktopHandle.ToString()); //TODO No hardcoded thinks

            GetDesktops();
        }

        public static void DoStuff() { }

        private static void Switch(string name)
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

        public static void GetDesktops()
        {
            IntPtr windowStation = User32.GetProcessWindowStation();
            bool result = User32.EnumDesktops(windowStation, DesktopEnumProc, IntPtr.Zero);
        }

        private static bool DesktopEnumProc(string lpszDesktop, IntPtr lParam)
        {
            Desktops.Add(new Desktop(lpszDesktop, MainDesktopHandle)); ;
            return true;
        }


        public static void SwitchBack() => Switch("Default");

    }
}
