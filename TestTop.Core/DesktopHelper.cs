using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using TestTop.Core.WinAPI;
using System.Windows.Forms;

namespace TestTop.Core
{
    public sealed class DesktopHelper
    {
        public enum DesktopWindow
        {
            ProgMan,
            SHELLDLL_DefViewParent,
            SHELLDLL_DefView,
            SysListView32
        }

        public List<DesktopIcon> Icons { get; private set; }

        public IntPtr DesktopHandle { get; internal set; }

        /// <summary>
        /// Erzeugt eine neue Instanz der DesktopHelper-Klasse.
        /// </summary>
        public DesktopHelper()
        {
            Icons = new List<DesktopIcon>();

            DesktopHandle = GetWorkerWWindow();

            if (DesktopHandle.Equals(IntPtr.Zero))
                throw new ArgumentNullException("DesktopHandle");
            else
                CreateDesktopIconList();
        }

        public static IntPtr GetDesktopWindow(DesktopWindow desktopWindow)
        {
            IntPtr _ProgMan = User32.GetShellWindow();
            IntPtr _SHELLDLL_DefViewParent = _ProgMan;
            IntPtr _SHELLDLL_DefView = User32.FindWindowEx(_ProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
            IntPtr _SysListView32 = User32.FindWindowEx(_SHELLDLL_DefView, IntPtr.Zero, "SysListView32", "FolderView");

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

        public static IntPtr GetWorkerWWindow()
        {
            IntPtr hDesktopListView = IntPtr.Zero;
            IntPtr hWorkerW = IntPtr.Zero;

            IntPtr hProgman = User32.FindWindow(("Progman"), "");
            IntPtr hDesktopWnd = GetDesktopWindow(DesktopWindow.SysListView32);

            if (hDesktopWnd != IntPtr.Zero)
                return hDesktopWnd;
            else
            {
                IntPtr hShellViewWin = User32.FindWindowEx(hProgman, IntPtr.Zero, ("SHELLDLL_DefView"), "");

                if (hShellViewWin != IntPtr.Zero)
                    hDesktopListView = User32.FindWindowEx(hShellViewWin, IntPtr.Zero, ("SysListView32"), "");
                else
                {
                    do
                    {
                        hWorkerW = User32.FindWindowEx(hDesktopWnd, hWorkerW, ("WorkerW"), "");
                        hShellViewWin = User32.FindWindowEx(hWorkerW, IntPtr.Zero, ("SHELLDLL_DefView"), "");
                        hShellViewWin = User32.FindWindowEx(hShellViewWin, IntPtr.Zero, ("SysListView32"), "FolderView");

                    } while (hShellViewWin == IntPtr.Zero && hWorkerW != IntPtr.Zero);

                    hDesktopListView = hShellViewWin;
                }

            }

            return hDesktopListView;
        }

        /// <summary>
        /// Erneuert die Iconliste.
        /// </summary>
        public void UpdateIcons() => CreateDesktopIconList();

        public void RestoreIconPositions()
        {
            DesktopIcon[] backupIcons = new DesktopIcon[Icons.Count];
            Icons.CopyTo(backupIcons);

            UpdateIcons();

            foreach (var icon in Icons)
            {
                var tmpIcon = backupIcons.FirstOrDefault(x => x.Name == icon.Name);

                if (tmpIcon != null)
                    SetIconPosition(icon, tmpIcon.Location.X, tmpIcon.Location.Y);
            }
        }

        /// <summary>
        /// Verändert die Position eines Icons auf dem Desktop
        /// </summary>
        /// <param name="icon">Das Icon, welches verschoben werden soll.</param>
        /// <param name="newLocation">Die neue Position des Icons.</param>
        public void SetIconPosition(DesktopIcon icon, Point newLocation)
        {
            if (User32.SendMessage(DesktopHandle, (uint)WindowsMessage.LVM_SETITEMPOSITION,
                (uint)icon.Index, newLocation.Y * 65536 + newLocation.X == 1) == 1)
            {
                icon.Location = newLocation;
            }
        }

        /// <summary>
        /// Verändert die Position eines Icons auf dem Desktop
        /// </summary>
        /// <param name="icon">Das Icon, welches verschoben werden soll.</param>
        /// <param name="x">X Position</param>
        /// <param name="y">Y Position</param>
        public void SetIconPosition(DesktopIcon icon, int x, int y) => SetIconPosition(icon, new Point(x, y));


        /// <summary>
        /// Ruft die Anzahl der Icons ab, die sich auf dem
        /// Desktop befinden.
        /// </summary>
        public uint IconCount => User32.SendMessage(DesktopHandle, (uint)WindowsMessage.LVM_GETITEMCOUNT, 0, false);

        /// <summary>
        /// Füllt die Liste m_Items mit den Icons vom Desktop.
        /// </summary>
        private void CreateDesktopIconList()
        {
            Icons.Clear();

            User32.GetWindowThreadProcessId(DesktopHandle, out uint vProcessId);

            IntPtr vProcess = Kernel32.OpenProcess(
                    (uint)WindowsMessage.PROCESS_VM_OPERATION |
                    (uint)WindowsMessage.PROCESS_VM_READ |
                    (uint)WindowsMessage.PROCESS_VM_WRITE, false, vProcessId);

            IntPtr vPointer = Kernel32.VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                    (uint)WindowsMessage.MEM_RESERVE |
                    (uint)WindowsMessage.MEM_COMMIT, (uint)WindowsMessage.PAGE_READWRITE);

            try
            {
                for (uint i = 0; i < IconCount; i++)
                {
                    byte[] vBuffer = new byte[256];

                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = (int)WindowsMessage.LVIF_TEXT;
                    vItem[0].iItem = (int)i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM)));

                    uint vNumberOfBytesRead = 0;

                    Kernel32.WriteProcessMemory(vProcess, vPointer,
                            Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                            Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);

                    User32.SendMessage(DesktopHandle, (uint)WindowsMessage.LVM_GETITEMW, i, vPointer.ToInt32() == 1);

                    Kernel32.ReadProcessMemory(vProcess,
                            (IntPtr)((int)vPointer + Marshal.SizeOf(typeof(LVITEM))),
                            Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                            vBuffer.Length, ref vNumberOfBytesRead);


                    string iconName = Encoding.Unicode.GetString(vBuffer, 0,
                            (int)vNumberOfBytesRead);
                    iconName = iconName.Substring(0, iconName.IndexOf('\0'));

                    User32.SendMessage(DesktopHandle, (uint)WindowsMessage.LVM_GETITEMPOSITION, i, vPointer.ToInt32() == 1);

                    Point[] vPoint = new Point[1];
                    Kernel32.ReadProcessMemory(vProcess, vPointer,
                            Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0),
                            Marshal.SizeOf(typeof(Point)), ref vNumberOfBytesRead);

                    Icons.Add(new DesktopIcon((int)i, iconName, vPoint[0]));
                }
            }
            finally
            {
                Kernel32.VirtualFreeEx(vProcess, vPointer, 0, (uint)WindowsMessage.MEM_RELEASE);
                Kernel32.CloseHandle(vProcess);
            }
        }
    }
}