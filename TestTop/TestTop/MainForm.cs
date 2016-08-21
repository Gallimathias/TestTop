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

            MainDesktopHandle = User32.GetThreadDesktop(User32.GetCurrentThreadId());
            startDesktop = new Desktop("Default", MainDesktopHandle, CreateGraphics(), MainDesktopHandle);

            File.WriteAllText(@"D:\Desktops\mainhandle.txt", MainDesktopHandle.ToString());
            
            GetDesktops();
            comboBox.Items.AddRange(Desktops.ToArray());
            hotkeys.Add(new Hotkey(Handle, hotkeys.Count, (int)KeyModifier.MOD_CONTROL, Keys.O));

            FillList();
            
        }

        private async void FillList()
        {
            //Desktops = await HelperMethods.GetAsync<List<Desktop>>("/desktops/");
        }

        private void desktopButton_Click(object sender, EventArgs e)
        {
            Desktop tempDesk = null;
            startDesktop.Save();
            foreach (Desktop Desk in Desktops)
            {
                if (Desk.Name == comboBox.Text)
                {
                    Desktops[Desktops.Count - 1].CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"SARButton.exe -" + MainDesktopHandle.ToString()));
                    Desk.Show();
                    //userControl11.Add(Desk.Image);

                }
                else
                {
                    if (Desktops.Where(d => d.Name.Equals(comboBox.Text)).Count() > 0)
                        continue;
                    tempDesk = new Desktop(comboBox.Text, MainDesktopHandle, this.CreateGraphics());

                }
            }
            if (tempDesk != null)
            {
                Desktops.Add(tempDesk);
                GetDesktops();
                Desktops[Desktops.Count - 1].CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
                Desktops[Desktops.Count - 1].CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"SARButton.exe -" + MainDesktopHandle.ToString()));
                Desktops[Desktops.Count - 1].Show();
            }
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
            //User32.SwitchDesktop(MainDesktopHandle);
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