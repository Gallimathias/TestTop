using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Configuration;
using TestTop.Core.WinAPI;
using TestTop.Core;

namespace TestTop
{
    public partial class MainForm : Form
    {
        public List<Desktop> Desktops { get; set; }
        public IntPtr MainDesktopHandle { get; private set; }

        private Desktop startDesktop;
        private static List<Hotkey> hotkeys;

        public MainForm()
        {
            InitializeComponent();
            hotkeys = new List<Hotkey>();
            Desktops = new List<Desktop>();
            
            MainDesktopHandle = User32.GetDesktopWindow();
            startDesktop = new Desktop("Default", MainDesktopHandle, CreateGraphics(), MainDesktopHandle);

            File.WriteAllText(@"D:\Desktops\mainhandle.txt", MainDesktopHandle.ToString());
            
            GetDesktops();
            comboBox.Items.AddRange(Desktops.ToArray());
            hotkeys.Add(new Hotkey(Handle, hotkeys.Count, (int)KeyModifier.MOD_CONTROL, Keys.O));
            
            
        }

        private void desktopButton_Click(object sender, EventArgs e)
        {
            startDesktop.Save();

            if (string.IsNullOrWhiteSpace(comboBox.Text))
                return;

            var desk = Desktops.FirstOrDefault(d => d.Name == comboBox.Text);

            if (desk == null)
            {
                desk = new Desktop(comboBox.Text, MainDesktopHandle, CreateGraphics());
                Desktops.Add(desk);
                GetDesktops();
            }

            desk.Show();
            desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            //desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            desktopControl1.Add(desk.Name, desk.Image);

           
        }

        public void GetDesktops()
        {
            IntPtr windowStation = User32.GetProcessWindowStation();
            bool result = User32.EnumDesktops(windowStation, DesktopEnumProc, IntPtr.Zero);
        }

        private bool DesktopEnumProc(string lpszDesktop, IntPtr lParam)
        {
            Desktops.Add(new Desktop(lpszDesktop, MainDesktopHandle, CreateGraphics()));;
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)KeyModifier.WM_HOTKEY)
            {
                switch ((int)m.WParam)
                {
                    case 0: switchBack(); break;
                    case 1: MessageBox.Show(String.Format("Hotkey {0} erhalten.", m.WParam.ToString())); break;
                    case 2: MessageBox.Show(String.Format("Hotkey {0} erhalten.", m.WParam.ToString())); break;
                }
            }
            base.WndProc(ref m);
        }

        public void switchBack()
        {
            User32.SwitchDesktop(MainDesktopHandle);
            User32.SetThreadDesktop(MainDesktopHandle);
            RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
            string value = (string)userKey?.GetValue("Desktop");
            userKey?.SetValue("Desktop", @"%USERPROFILE%\Desktop", RegistryValueKind.ExpandString);
        }

        public void Save()
        {
            //***RegistryKey key = Registry.CurrentUser.OpenSubKey();
        }

        private void SaveButton_Click(object sender, EventArgs e) { }

        private void MainForm_Load(object sender, EventArgs e) { }
    }
}