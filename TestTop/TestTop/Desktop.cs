using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using TestTop.Basic;

namespace TestTop
{
    [Serializable]
    public class Desktop : IDisposable
    {
        public string Name { get; set; }
        [XmlIgnore]
        public DirectoryInfo Dir { get; set; }
        [XmlIgnore]
        public IntPtr HandleDesktop { get; private set; }
        [XmlIgnore]
        private IntPtr NormalDesktop { get; set; }
        [XmlIgnore]
        public Dictionary<string, byte[]> ItemPos { get; set; }

        public Desktop()
        {

        }
        public Desktop(string name, IntPtr normalDesktop)
        {
            Name = name;
            HandleDesktop = createNewDesktop();
            User32.OpenDesktop(Name, 0x0001, false, (long)DesktopAcessMask.GENERIC_ALL);
            NormalDesktop = normalDesktop;
            ItemPos = new Dictionary<string, byte[]>();
            Dir = new DirectoryInfo(
                Path.Combine(ConfigurationManager.AppSettings.GetValues("savepath").First(),
                Name, Name));

            if (!File.Exists(Dir.Parent.FullName + "\\options.dt"))
            {
                Dir.Create();
                Save();
            }
            else
            {
                DesktopSerializer.DeSerializer(this);
            }
        }

        private IntPtr createNewDesktop()
        {
            return User32.CreateDesktop(Name, IntPtr.Zero, IntPtr.Zero, 0, (long)DesktopAcessMask.GENERIC_ALL, IntPtr.Zero);
        }

        public void Delete()
        {
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", @"%USERPROFILE%\Desktop", RegistryValueKind.ExpandString);
            User32.SetThreadDesktop(NormalDesktop);
            User32.SwitchDesktop(NormalDesktop);
            bool returnvalue = User32.CloseDesktop(HandleDesktop);
            //Dispose();
        }
        public void Show()
        {
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", Dir.FullName, RegistryValueKind.ExpandString);

            userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Bags\1\Desktop", RegistryKeyPermissionCheck.ReadWriteSubTree);

            foreach (var IconPositions in ItemPos)
            {
                userKey?.SetValue(IconPositions.Key, IconPositions.Value, RegistryValueKind.Binary);
            }

            User32.SetThreadDesktop(HandleDesktop);
            User32.SwitchDesktop(HandleDesktop);
            
        }

        public void CreateProcess(string name)
        {
            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            si.lpDesktop = Name;

            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

            // start the process.
            bool result = User32.CreateProcess(null, name, IntPtr.Zero, IntPtr.Zero, true, (int)WindowStylesEx.WS_EX_TRANSPARENT, IntPtr.Zero, null, ref si, ref pi);
        }

        public void Save()
        {
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\Shell\Bags\1\Desktop", RegistryKeyPermissionCheck.ReadWriteSubTree);
            ItemPos.Clear();
            foreach (var item in userKey.GetValueNames())
            {
                if (item.Contains("ItemPos"))
                {
                    ItemPos.Add(item, (byte[])userKey.GetValue(item));
                }
            }

            DesktopSerializer.Serialize(this);
        }

        public void Dispose()
        {
            Name = null;
            HandleDesktop = IntPtr.Zero;
            NormalDesktop = IntPtr.Zero;
        }
    }
}
