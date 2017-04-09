using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Linq;
using System.Configuration;
using TestTop.Core.WinAPI;
using TestTop.Core;

namespace TestTop.Forms
{
    public partial class MainForm : Form
    {
        public List<Desktop> Desktops { get; set; }
        public IntPtr MainDesktopHandle { get; private set; }
        public Desktop CurrentDesktop { get; private set; }

        private Desktop startDesktop;
        private static List<Hotkey> hotkeys;

        public MainForm()
        {
            InitializeComponent();
            hotkeys = new List<Hotkey>();
            Desktops = new List<Desktop>();

            MainDesktopHandle = User32.GetDesktopWindow();
            startDesktop = new Desktop("Default", MainDesktopHandle, CreateGraphics(), MainDesktopHandle);
            CurrentDesktop = startDesktop;
            File.WriteAllText(@"C:\Users\Public\Desktops\mainhandle.txt", MainDesktopHandle.ToString()); //TODO No hardcoded thinks

            GetDesktops();
            comboBox.Items.AddRange(Desktops.ToArray());
            hotkeys.Add(new Hotkey(Handle, hotkeys.Count, (int)KeyModifier.MOD_CONTROL, Keys.O));
            
        }

        private void DesktopButton_Click(object sender, EventArgs e)
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

            desktopControl1.Add(CurrentDesktop.Name, CurrentDesktop.TakeScreenshot());
            desk.Show();
            desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            //desk.CreateProcess(Path.Combine(Environment.GetEnvironmentVariable("windir"), @"explorer.exe"));
            CurrentDesktop = desk;

        }

        public void GetDesktops()
        {
            IntPtr windowStation = User32.GetProcessWindowStation();
            bool result = User32.EnumDesktops(windowStation, DesktopEnumProc, IntPtr.Zero);
        }

        private bool DesktopEnumProc(string lpszDesktop, IntPtr lParam)
        {
            Desktops.Add(new Desktop(lpszDesktop, MainDesktopHandle, CreateGraphics())); ;
            return true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)KeyModifier.WM_HOTKEY)
            {
                switch ((int)m.WParam)
                {
                    case 0: SwitchBack(); break;
                    case 1: MessageBox.Show(string.Format("Hotkey {0} erhalten.", m.WParam.ToString())); break;
                    case 2: MessageBox.Show(string.Format("Hotkey {0} erhalten.", m.WParam.ToString())); break;
                }
            }
            base.WndProc(ref m);
        }

        public void SwitchBack()// => Desktops.First(y => y.Name == "Default").Show();
        {
            comboBox.Text = "Default";
            DesktopButton_Click(null, null);
        }

        private void SaveButton_Click(object sender, EventArgs e) { }
        
    }
}