using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTop.Core.JsonFiles;

namespace TestTop.Core
{
    public static class DesktopManager
    {
        public static MainIni MainIni { get; private set; }
        public static DirectoryInfo DesktopPath { get; private set; }
        public static DirectoryInfo DesktopFolder { get; private set; }
        public static List<Desktop> Desktops { get; set; }
        public static IntPtr MainDesktopHandle { get; set; }

        static DesktopManager()
        {
            string value = "";

            try
            {
                //TODO: No Hardcoded things

                value = (string)Registry
                    .CurrentUser
                    .OpenSubKey(@"SOFTWARE\NoobDev\TestTop\Main", RegistryKeyPermissionCheck.ReadWriteSubTree)
                    .GetValue("Desktopfolder");
            }
            catch (Exception ex)
            {
            }

            DesktopFolder = new DirectoryInfo(value);
            MainIni = FileManager.DeserializeMainIni(Path.Combine(DesktopFolder.FullName, "main.ini"));

        }
        
        public static void AddDesktopToIni(Desktop desktop)
        {
            MainIni.Desktops.Add(desktop.Name);
            SaveIni();
        }

        private static void SaveIni() 
            => FileManager.SerializeMainIni(MainIni, Path.Combine(DesktopPath.FullName,"main.ini"));

    }
}
