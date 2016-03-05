using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace SARButton
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll")]
        public static extern bool SwitchDesktop(IntPtr hDesktop);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetThreadDesktop(IntPtr hDesktop);

        public IntPtr DesktopHandle { get; set; }
        public MainForm(string[] args)
        {
            InitializeComponent();
            if(args.Count() != 0)
            {
                string temp = args[0].Remove(0,1);
                DesktopHandle = new IntPtr(Convert.ToInt32(temp));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (DesktopHandle != null)
            {
                SwitchDesktop(DesktopHandle);
                SetThreadDesktop(DesktopHandle);
                RegistryKey userKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", RegistryKeyPermissionCheck.ReadWriteSubTree);
                string value = (string)userKey?.GetValue("Desktop");
                userKey?.SetValue("Desktop", @"%USERPROFILE%\Desktop", RegistryValueKind.ExpandString);
            }
        }
    }
}
