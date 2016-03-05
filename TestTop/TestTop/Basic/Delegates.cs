using System;
using System.Collections.Generic;
using System.Text;

namespace TestTop.Basic
{
    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    public delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    public delegate bool EnumDesktopProc(string lpszDesktop, IntPtr lParam);
}
