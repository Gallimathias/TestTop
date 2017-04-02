using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.IO;
using System.Linq;
using System.Configuration;
using System.Drawing;
using TestTop.Core.WinAPI;
using System.Windows.Forms;
using TestTop.Core.JsonFiles;

namespace TestTop.Core
{
    public class Desktop : IDisposable
    {
        public string Name { get; set; }
        public Image Image { get; private set; }
        public DirectoryInfo Dir
        {
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
        public IntPtr HandleDesktop { get; private set; }
        public DesktopHelper DesktopHelper { get; set; }

        private IntPtr normalDesktop;
        private Graphics graphics;
        private DirectoryInfo dir;

        public Desktop(string name, IntPtr normalDesktop, Graphics graphics)
        {
            Name = name;
            this.graphics = graphics;
            this.normalDesktop = normalDesktop;

            DesktopHelper = new DesktopHelper();

            Dir = new DirectoryInfo(
                Path.Combine(ConfigurationManager.AppSettings.GetValues("savepath").First(),
                Name, Name));

            HandleDesktop = createNewDesktop();  //TODO: Problem
            
            if (!File.Exists(Dir.Parent.FullName + "\\options.dt"))
            {
                Dir.Create();
                Save();
            }
            else
            {
                DesktopSerializer.DeSerializer(this);
            }
            
            User32.OpenDesktop(Name, 0x0001, false, (long)DesktopAcessMask.GENERIC_ALL);
        }

        public Desktop(string name, IntPtr normalDesktop, Graphics graphics, IntPtr DesktopHandle) : this(name, normalDesktop, graphics)
        {
            HandleDesktop = DesktopHandle;
        }
        
        public void Delete()
        {
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", @"%USERPROFILE%\Desktop", RegistryValueKind.ExpandString);

            User32.SetThreadDesktop(normalDesktop);
            User32.SwitchDesktop(normalDesktop);
            User32.SetThreadDesktop(normalDesktop);            
            User32.CloseDesktop(HandleDesktop);
            
            //Dispose();
        }

        public void Show()
        {
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            //string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", Dir.FullName, RegistryValueKind.ExpandString);

            User32.SetThreadDesktop(HandleDesktop);
            User32.SwitchDesktop(HandleDesktop);
            User32.SetThreadDesktop(HandleDesktop);
        }

        public void CreateProcess(string name)
        {
            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            si.lpDesktop = Name;

            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();

            // start the process.
            User32.CreateProcess(null, name, IntPtr.Zero, IntPtr.Zero, true, (int)WindowStylesEx.WS_EX_TRANSPARENT,
                                 IntPtr.Zero, null, ref si, ref pi);
        }

        public void Save() => DesktopSerializer.Serialize(this);

        public void Dispose()
        {
            Name = null;
            HandleDesktop = IntPtr.Zero;
            normalDesktop = IntPtr.Zero;
        }

        public DesktopIni ToIni()
        {
            var ini = new DesktopIni();
            ini.Name = Name;
            ini.Screenshot = @""; //TODO Gute Zeile
            return ini;
        }

        public override string ToString() => Name;

        public Bitmap TakeScreenshot()
        {
            var width   = Screen.PrimaryScreen.Bounds.Width;
            var height  = Screen.PrimaryScreen.Bounds.Height;
            using (Bitmap bmpScreenCapture = new Bitmap(width, height))
            {
                using (Graphics g = Graphics.FromImage(bmpScreenCapture))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, //TODO Idee vom Stream 4.2.2017
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmpScreenCapture.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                bmpScreenCapture.Save(Dir.Parent.FullName + @"/screenshot.bmp");
                return new Bitmap(bmpScreenCapture, new Size(width / 4, height / 4));
            }
        }
        
        private IntPtr createNewDesktop() =>
          User32.CreateDesktop(Name, IntPtr.Zero, IntPtr.Zero, 0, (long)DesktopAcessMask.GENERIC_ALL, IntPtr.Zero);
    }
}
