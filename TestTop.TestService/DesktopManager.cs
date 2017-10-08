using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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

            startDesktop = new Desktop("Default", MainDesktopHandle, MainDesktopHandle);
            MainDesktopHandle = User32.GetDesktopWindow();
            CurrentDesktop = startDesktop;
            File.WriteAllText(@"C:\Users\Public\mainhandle.txt", MainDesktopHandle.ToString()); //TODO No hardcoded thinks

            GetDesktops();
        }

        public static void DoStuff() { }

        public static void Switch(string name)
        {
            startDesktop.Save();

            if (string.IsNullOrWhiteSpace(name))
                return;

            var desk = Desktops.FirstOrDefault(d => d.Name == name);

            if (desk == null)
            {
                desk = new Desktop(name, MainDesktopHandle);
                Desktops.Add(desk);
            }
            //var list = Process.GetProcesses().Where(x => x.ProcessName.ToLower().Contains("explorer")).ToList();
            //list.ForEach(x => x.Kill());
            desk.Show();
            CurrentDesktop = desk;

            if (MainService.Clients.Contains(name))
                return;
            desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            desk.CreateProcess(@"..\..\..\\TestTop.UI\bin\Debug\TestTop.UI.exe");

        }

        public static void NewDesktopSwitch(string name, string handle)
        {
            var desk = Desktops.FirstOrDefault(x => x.Name == name);
            desk.DesktopHelper.DesktopHandle = new IntPtr(int.Parse(handle));
            Task.Run(() => { MessageBox.Show(handle.ToString()); });
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
