using System;
using System.Collections.Concurrent;
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
    internal static class DesktopManager
    {
        public static ConcurrentBag<Desktop> Desktops { get; internal set; }
        public static IntPtr MainDesktopHandle { get; private set; }
        public static Desktop CurrentDesktop { get; private set; }
        

        static DesktopManager()
        {
            Desktops = new ConcurrentBag<Desktop>();

            CurrentDesktop = new Desktop("Default", MainDesktopHandle, MainDesktopHandle);
            MainDesktopHandle = User32.GetDesktopWindow();
            File.WriteAllText(@"C:\Users\Public\mainhandle.txt", MainDesktopHandle.ToString()); //TODO No hardcoded thinks

            GetDesktops();
        }

        public static void DoStuff() { }

        public static void Switch(string name)
        {
            CurrentDesktop.Save();

            if (string.IsNullOrWhiteSpace(name))
                return;

            var desk = Desktops.FirstOrDefault(d => d.Name == name);

            if (desk == null)
            {
                desk = new Desktop(name, MainDesktopHandle);
                Desktops.Add(desk);
            }

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
            //Task.Run(() => { MessageBox.Show(handle.ToString()); }); //only for manual debugging
        }

        public static bool GetDesktops()
        {
            IntPtr windowStation = User32.GetProcessWindowStation();
            return User32.EnumDesktops(windowStation, DesktopEnumProc, IntPtr.Zero);
        }
        
        public static void SwitchBack() => Switch("Default");

        private static bool DesktopEnumProc(string lpszDesktop, IntPtr lParam)
        {
            var count = Desktops.Count;
            Desktops.Add(new Desktop(lpszDesktop, MainDesktopHandle));
            return Desktops.Count > count;
        }

    }
}
