using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using TestTop.Core.WinAPI;

namespace TestTop.Core
{
    public sealed class DesktopHelper
    {
        #region Extern

        [DllImport("kernel32.dll")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
                uint dwSize, uint flAllocationType, uint flProtect);

        [DllImport("kernel32.dll")]
        public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
             uint dwSize, uint dwFreeType);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
             IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
             IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess,
                bool bInheritHandle, uint dwProcessId);

        [DllImport("user32.DLL")]
        public static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

        [DllImport("user32.DLL")]
        public static extern IntPtr FindWindow(string lpszClass, string lpszWindow);

        [DllImport("user32.DLL")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent,
                IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd,
                out uint dwProcessId);

        #endregion

        #region Windows Messages

        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        private const uint LVM_GETITEMW = LVM_FIRST + 75;
        private const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
        private const uint LVM_SETITEMPOSITION = LVM_FIRST + 15;
        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RELEASE = 0x8000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_READWRITE = 4;
        private const int LVIF_TEXT = 0x0001;

        #endregion

        public List<DesktopIcon> Icons { get; set; }
        private IntPtr desktopHandle = IntPtr.Zero;

        /// <summary>
        /// Füllt die Liste m_Items mit den Icons vom Desktop.
        /// </summary>
        private void CreateDesktopIconList()
        {
            this.Icons.Clear();

            int iconCount = this.IconCount;

            GetWindowThreadProcessId(this.desktopHandle, out uint vProcessId);

            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                    PROCESS_VM_WRITE, false, vProcessId);

            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                    MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

            try
            {
                for (int i = 0; i < iconCount; i++)
                {
                    byte[] vBuffer = new byte[256];

                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));

                    uint vNumberOfBytesRead = 0;

                    WriteProcessMemory(vProcess, vPointer,
                            Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                            Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

                    SendMessage(this.desktopHandle, LVM_GETITEMW, i, vPointer.ToInt32());

                    ReadProcessMemory(vProcess,
                            (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                            Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                            vBuffer.Length, ref vNumberOfBytesRead);


                    string iconName = Encoding.Unicode.GetString(vBuffer, 0,
                            (int)vNumberOfBytesRead);
                    iconName = iconName.Substring(0, iconName.IndexOf('\0'));

                    SendMessage(this.desktopHandle, LVM_GETITEMPOSITION, i, vPointer.ToInt32());

                    Point[] vPoint = new Point[1];
                    ReadProcessMemory(vProcess, vPointer,
                            Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
                            Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);

                    this.Icons.Add(new DesktopIcon(
                        i, iconName, vPoint[0]));
                }
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
        }

        /// <summary>
        /// Erzeugt eine neue Instanz der DesktopHelper-Klasse.
        /// </summary>
        public DesktopHelper()
        {
            Icons = new List<DesktopIcon>();
            
            desktopHandle = GetDesktopWindow(DesktopWindow.SysListView32);

            // Handle des Desktops ermitteln. Wenn das fehlschlägt, eine
            // Exception werfen.
            //desktopHandle = FindWindow("WorkerW", "");
            //desktopHandle = FindWindowEx(desktopHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            //desktopHandle = FindWindowEx(desktopHandle, IntPtr.Zero, "SysListView32", "FolderView");
            //if (desktopHandle == IntPtr.Zero)
            //{
            //    desktopHandle = FindWindow("Progman", "Program Manager");
            //    desktopHandle = new IntPtr((desktopHandle.ToInt32() + 2));
            //    desktopHandle = FindWindowEx(desktopHandle, IntPtr.Zero, "SysListView32", "FolderView");
            //}

            //if (desktopHandle == IntPtr.Zero)
            //    desktopHandle = new IntPtr(0x0001018A);

                //desktopHandle = User32.GetDesktopWindow();

            if (desktopHandle.Equals(IntPtr.Zero))
                throw new ArgumentNullException("DesktopHandle");
            else
                CreateDesktopIconList();
        }

        public enum DesktopWindow
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }

        public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
        {
            IntPtr _ProgMan = User32.GetShellWindow();
            IntPtr _SHELLDLL_DefViewParent = _ProgMan;
            IntPtr _SHELLDLL_DefView = FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            IntPtr _SysListView32 = FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

            if (_SHELLDLL_DefView == IntPtr.Zero)
            {
                User32.EnumWindows((hwnd, lParam) =>
                {
                    StringBuilder ClassName = new StringBuilder(256);
                    User32.GetClassName(hwnd, ClassName, ClassName.Length);
                    if (ClassName.ToString() == "WorkerW")
                    {
                        IntPtr child = FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null);
                        if (child != IntPtr.Zero)
                        {
                            _SHELLDLL_DefViewParent = hwnd;
                            _SHELLDLL_DefView = child;
                            _SysListView32 = FindWindowEx(child, IntPtr.Zero, "SysListView32", "FolderView"); ;
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);
            }

            switch (desktopWindow)
            {
                case DesktopWindow.ProgMan:
                    return _ProgMan;
                case DesktopWindow.SHELLDLL_DefViewParent:
                    return _SHELLDLL_DefViewParent;
                case DesktopWindow.SHELLDLL_DefView:
                    return _SHELLDLL_DefView;
                case DesktopWindow.SysListView32:
                    return _SysListView32;
                default:
                    return IntPtr.Zero;
            }
        }

        /// <summary>
        /// Erneuert die Iconliste.
        /// </summary>
        public void UpdateIcons()
        {
            this.CreateDesktopIconList();
        }

        public void RestoreIconPositions()
        {
            DesktopIcon[] backupIcons = new DesktopIcon[Icons.Count];
            Icons.CopyTo(backupIcons);

            UpdateIcons();
            foreach (var item in Icons)
            {
                var icon = backupIcons.FirstOrDefault(x => x.Name == item.Name);
                if (icon != null)
                    SetIconPosition(item, icon.Location.X, icon.Location.Y);
            }
        }

        /// <summary>
        /// Verändert die Position eines Icons auf dem Desktop
        /// </summary>
        /// <param name="icon">Das Icon, welches verschoben werden soll.</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public void SetIconPosition(DesktopIcon icon, int x, int y)
        {
            this.SetIconPosition(icon, new Point(x, y));
        }

        /// <summary>
        /// Verändert die Position eines Icons auf dem Desktop
        /// </summary>
        /// <param name="icon">Das Icon, welches verschoben werden soll.</param>
        /// <param name="newLocation">Die neue Position des Icons.</param>
        public void SetIconPosition(DesktopIcon icon, Point newLocation)
        {
            int posX = newLocation.X;
            int posY = newLocation.Y;

            if (SendMessage(this.desktopHandle, LVM_SETITEMPOSITION,
                icon.Index, posY * 65536 + posX) == 1)
            {
                icon.Location = newLocation;
            }
        }
        

        /// <summary>
        /// Ruft die Anzahl der Icons ab, die sich auf dem
        /// Desktop befinden.
        /// </summary>
        public int IconCount
        {
            get
            {
                return SendMessage(this.desktopHandle, LVM_GETITEMCOUNT, 0, 0);
            }
        }
    }

    public class DesktopIcon
    {
        private int m_Index;
        private string m_Name;
        private Point m_Location;

        public DesktopIcon(int index, string name, Point location)
        {
            this.m_Index = index;
            this.m_Location = location;
            this.m_Name = name;
        }

        public int Index
        {
            get { return this.m_Index; }
        }

        public Point Location
        {
            get { return this.m_Location; }
            internal set { this.m_Location = value; }
        }

        public string Name
        {
            get { return this.m_Name; }
        }
    }

    internal struct LVITEM
    {
        public int mask;
        public int iItem;
        public int iSubItem;
        public int state;
        public int stateMask;
        public IntPtr pszText; // string
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
        public int iIndent;
        public int iGroupId;
        public int cColumns;
        public IntPtr puColumns;
    }
}