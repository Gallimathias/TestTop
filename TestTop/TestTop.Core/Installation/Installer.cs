using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core.Installation
{
    public static class Installer
    {
        public static void Install()
        {
            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\NoobDev\TestTop\Main");
            Registry.CurrentUser.SetValue(@"SOFTWARE\NoobDev\TestTop\Main", 
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree).GetValue("Desktop").ToString());
            var cur = new DirectoryInfo(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree).GetValue("Desktop").ToString());
            File.Create($@"{cur.Parent.Parent.FullName}\main.ini");
        }

        public static void Uninstall()
        {
            Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\NoobDev\TestTop\Main");
        }
    }
}
