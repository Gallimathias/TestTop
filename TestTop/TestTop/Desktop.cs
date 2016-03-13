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
        public Image Image { get; private set; }
        [XmlIgnore]
        private DirectoryInfo dir;
        public DirectoryInfo Dir {
            get
            {
                return dir;
            }
            set
            {
                if (value.Name == "Default")
                    dir = new DirectoryInfo(Path.Combine(value.Parent.FullName, "Desktop"));
                else
                    dir = value;
            }
        }
        [XmlIgnore]
        public IntPtr HandleDesktop { get; private set; }
        [XmlIgnore]
        private IntPtr NormalDesktop { get; set; }
        [XmlIgnore]
        public DesktopHelper DesktopHelper { get; set; }
        private Graphics graphics;

        public Desktop()
        {

        }
        public Desktop(string name, IntPtr normalDesktop,Graphics graphics)
        {
            Name = name;
            User32.OpenDesktop(Name, 0x0001, false, (long)DesktopAcessMask.GENERIC_ALL);
            NormalDesktop = normalDesktop;
            DesktopHelper = new DesktopHelper();
            Dir = new DirectoryInfo(
                Path.Combine(ConfigurationManager.AppSettings.GetValues("savepath").First(),
                Name, Name));
            HandleDesktop = createNewDesktop();
            this.graphics = graphics;
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
            var screens = Screen.AllScreens.ToList();
            
            using (Bitmap bmpScreenCapture = new Bitmap(2560, 1440))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);

                }
                    Image = (Image)new Bitmap(bmpScreenCapture, new Size(420, 270));
            }

            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", Dir.FullName, RegistryValueKind.ExpandString);

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
