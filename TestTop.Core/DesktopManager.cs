﻿using Microsoft.Win32;
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
        public static DirectoryInfo CurrentDesktopPath { get; private set; }
        public static DirectoryInfo CurrentDesktopsFolder { get; private set; }
        public static List<Desktop> Desktops { get; set; }
        public static IntPtr MainDesktopHandle { get; set; }
        //public static Desktop CurrentDesktop { get; private set; }
        //
        //private static Desktop startDesktop;


        public static bool IsInitialized { get; set; }

        public static void Initialize()
        {
            string value = "";
            try
            {
                value = (string)Registry.CurrentUser.OpenSubKey(@"SOFTWARE\NoobDev\TestTop\Main", RegistryKeyPermissionCheck.ReadWriteSubTree).GetValue("Desktopfolder");
            }
            catch(Exception ex)
            {
            }
           
            CurrentDesktopsFolder = new DirectoryInfo(value);
            MainIni = FileManager.DeserializeMainIni($@"{CurrentDesktopsFolder.FullName}\main.ini");

            IsInitialized = true;
        }

        public static void AddDesktopToIni(Desktop desktop)
        {
            MainIni.Desktops.Add(desktop.Name);
            SaveIni();
        }

        private static void SaveIni() => FileManager.SerializeMainIni(MainIni, $@"{CurrentDesktopPath.FullName}\main.ini");

    }
}
