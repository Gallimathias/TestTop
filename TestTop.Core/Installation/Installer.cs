using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestTop.Core.Installation
{
    public static class Installer
    {
        public static bool Install()
        {
            var dialog = new FolderBrowserDialog();

            var main = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NoobDev\TestTop\Main", RegistryKeyPermissionCheck.ReadWriteSubTree);
            if ( main != null){
                var v = (string)main.GetValue("Version");
                if("develop" == v) //TODO
                    return true;
            }
            
            if (dialog.ShowDialog() != DialogResult.OK)
                return false;

            Registry.CurrentUser.CreateSubKey(@"SOFTWARE\NoobDev\TestTop\Main");
            main = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NoobDev\TestTop\Main", RegistryKeyPermissionCheck.ReadWriteSubTree);
            
            main.SetValue("Origin", 
                Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree).GetValue("Desktop").ToString());
            var cur = dialog.SelectedPath;
            main.SetValue("Desktopfolder", cur, RegistryValueKind.String);
            
            
            var f = File.Create($@"{cur}\main.ini");
            f.Close();

            main.SetValue("Version", "develop");
            return true;
        }

        public static void Uninstall()
        {
            Registry.CurrentUser.DeleteSubKey(@"SOFTWARE\NoobDev\TestTop\Main");
        }
    }
}
