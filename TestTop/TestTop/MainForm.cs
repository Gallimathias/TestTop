using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Configuration;
using TestTop.Basic;

namespace TestTop
{
    public partial class MainForm : Form
    {
        public List<Desktop> Desktops { get; set; }
        private Desktop StarterDesktop { get; set; }
        public IntPtr MainDesktopHandle { get; private set; }
        private static List<string> _desktops = new List<string>();
        private static List<Hotkey> _hotkeys = new List<Hotkey>();

        const int MOD_CONTROL = 0x0002;
        const int MOD_SHIFT = 0x0004;
        const int WM_HOTKEY = 0x0312;

        public MainForm()
        {
            InitializeComponent();
            MainDesktopHandle = User32.GetThreadDesktop(User32.GetCurrentThreadId());
            StarterDesktop = new Desktop("Default", MainDesktopHandle);//TODO WARNUNG MUSS GEÄNDERT WERDEN
            Desktops = new List<Desktop>();
            GetDesktops();
            comboBox1.Items.AddRange(_desktops.ToArray());
            _hotkeys.Add(new Hotkey(Handle, _hotkeys.Count, MOD_CONTROL, Keys.O));
            foreach (string Desk in _desktops)
            {
                Desktops.Add(new Desktop(Desk, MainDesktopHandle));
            }
            comboBox1.AutoCompleteMode = AutoCompleteMode.Append;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void desktopButton_Click(object sender, EventArgs e)
        {
            Desktop tempDesk = null;
            StarterDesktop.Save();
            foreach (Desktop Desk in Desktops)
            {
                if (Desk.Name == comboBox1.Text)
                {
                    Desktops[Desktops.Count - 1].CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"SARButton.exe -" + MainDesktopHandle.ToString()));
                    Desk.Show();

                }
                else
                {
                    if (Desktops.Where(d => d.Name.Equals(comboBox1.Text)).Count() > 0)
                        continue;
                    tempDesk = new Desktop(comboBox1.Text, MainDesktopHandle);

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
            _desktops.Add(lpszDesktop);
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
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

        private void SaveButton_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
    }
}