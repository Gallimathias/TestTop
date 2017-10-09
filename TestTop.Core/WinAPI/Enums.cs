using System;

namespace TestTop.Core.WinAPI
{

    #region DesktopAcessMask

    [Flags]
    public enum DesktopAcessMask : uint
    {
        DESKTOP_NONE = 0,
        DESKTOP_READOBJECTS = 0x0001,
        DESKTOP_CREATEWINDOW = 0x0002,
        DESKTOP_CREATEMENU = 0x0004,
        DESKTOP_HOOKCONTROL = 0x0008,
        DESKTOP_JOURNALRECORD = 0x0010,
        DESKTOP_JOURNALPLAYBACK = 0x0020,
        DESKTOP_ENUMERATE = 0x0040,
        DESKTOP_WRITEOBJECTS = 0x0080,
        DESKTOP_SWITCHDESKTOP = 0x0100,

        GENERIC_ALL = (DESKTOP_READOBJECTS | DESKTOP_CREATEWINDOW | DESKTOP_CREATEMENU |
                        DESKTOP_HOOKCONTROL | DESKTOP_JOURNALRECORD | DESKTOP_JOURNALPLAYBACK |
                        DESKTOP_ENUMERATE | DESKTOP_WRITEOBJECTS | DESKTOP_SWITCHDESKTOP
                        )
    }

    #endregion

    #region GetClassLongPtr

    public enum GetClassLongPtr : int
    {
        GCL_MENUNAME = (-8),
        GCL_HBRBACKGROUND = (-10),
        GCL_HCURSOR = (-12),
        GCL_HICON = (-14),
        GCL_HMODULE = (-16),
        GCL_CBWNDEXTRA = (-18),
        GCL_CBCLSEXTRA = (-20),
        GCL_WNDPROC = (-24),
        GCL_STYLE = (-26),
        GCW_ATOM = (-32)
    }

    #endregion

    #region GetWindow

    public enum GetWindowCmd : uint
    {
        GW_HWNDFIRST = 0,
        GW_HWNDLAST = 1,
        GW_HWNDNEXT = 2,
        GW_HWNDPREV = 3,
        GW_OWNER = 4,
        GW_CHILD = 5,
        GW_ENABLEDPOPUP = 6
    }

    #endregion

    #region GetWindowLong

    public enum GWL : int
    {
        GWL_WNDPROC = (-4),
        GWL_HINSTANCE = (-6),
        GWL_HWNDPARENT = (-8),
        GWL_STYLE = (-16),
        GWL_EXSTYLE = (-20),
        GWL_USERDATA = (-21),
        GWL_ID = (-12)
    }

    #endregion

    #region HookType

    public enum HookType : int
    {
        WH_JOURNALRECORD = 0,
        WH_JOURNALPLAYBACK = 1,
        WH_KEYBOARD = 2,
        WH_GETMESSAGE = 3,
        WH_CALLWNDPROC = 4,
        WH_CBT = 5,
        WH_SYSMSGFILTER = 6,
        WH_MOUSE = 7,
        WH_HARDWARE = 8,
        WH_DEBUG = 9,
        WH_SHELL = 10,
        WH_FOREGROUNDIDLE = 11,
        WH_CALLWNDPROCRET = 12,
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    #endregion

    #region KeyModifier
    [Flags]
    public enum KeyModifier : int
    {
        MOD_CONTROL = 0x0002,
        MOD_SHIFT = 0x0004,
        WM_HOTKEY = 0x0312
    }
    #endregion KeyModifier

    #region MenuItemInfoMask

    [Flags]
    public enum MenuItemInfoMask : uint
    {
        MIIM_STATE = 0x00000001,
        MIIM_ID = 0x00000002,
        MIIM_SUBMENU = 0x00000004,
        MIIM_CHECKMARKS = 0x00000008,
        MIIM_TYPE = 0x00000010,
        MIIM_DATA = 0x00000020,
        MIIM_STRING = 0x00000040,
        MIIM_BITMAP = 0x00000080,
        MIIM_FTYPE = 0x00000100
    }

    #endregion

    #region MenuItemState

    [Flags]
    public enum MenuItemState : long
    {
        MFS_GRAYED = 0x00000003L,
        MFS_DISABLED = MFS_GRAYED,
        MFS_CHECKED = 0x00000008L,
        MFS_HILITE = 0x00000080L,
        MFS_ENABLED = 0x00000000L,
        MFS_UNCHECKED = 0x00000000L,
        MFS_UNHILITE = 0x00000000L,
        MFS_DEFAULT = 0x00001000L
    }

    #endregion

    #region MenuItemType

    [Flags]
    public enum MenuItemType : long
    {
        MFT_STRING = 0x00000000L,
        MFT_BITMAP = 0x00000004L,
        MFT_MENUBARBREAK = 0x00000020L,
        MFT_MENUBREAK = 0x00000040L,
        MFT_OWNERDRAW = 0x00000100L,
        MFT_RADIOCHECK = 0x00000200L,
        MFT_SEPARATOR = 0x00000800L,
        MFT_RIGHTORDER = 0x00002000L,
        MFT_RIGHTJUSTIFY = 0x00004000L
    }

    #endregion

    #region SendMessageTimeoutFlags

    [Flags]
    public enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL = 0x0000,
        SMTO_BLOCK = 0x0001,
        SMTO_ABORTIFHUNG = 0x0002,
        SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
    }

    #endregion

    #region WM

    public enum WM : uint
    {
        WM_PAINT = 0x000F
    }

    #endregion

    #region WindowShowStyle

    public enum WindowShowStyle : uint
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        ShowMaximized = 3,
        Maximize = 3,
        ShowNormalNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActivate = 7,
        ShowNoActivate = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimized = 11
    }

    #endregion

    #region WindowStyles

    [Flags]
    public enum WindowStyles : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
        WS_CAPTION = WS_BORDER | WS_DLGFRAME,
        WS_TILED = WS_OVERLAPPED,
        WS_ICONIC = WS_MINIMIZE,
        WS_SIZEBOX = WS_THICKFRAME,
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_CHILDWINDOW = WS_CHILD,
    }

    #endregion

    #region WindowStylesEx

    [Flags]
    public enum WindowStylesEx : uint
    {
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE),
        WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST),
        WS_EX_LAYERED = 0x00080000,
        WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }

    #endregion

    #region WindowsMessage
    [Flags]
    public enum WindowsMessage : uint
    {
        LVM_FIRST = 0x1000,
        LVM_GETITEMCOUNT = 0x1000 + 4,
        LVM_GETITEMW = 0x1000 + 75,
        LVM_GETITEMPOSITION = 0x1000 + 16,
        LVM_SETITEMPOSITION = 0x1000 + 15,
        PROCESS_VM_OPERATION = 0x0008,
        PROCESS_VM_READ = 0x0010,
        PROCESS_VM_WRITE = 0x0020,
        MEM_COMMIT = 0x1000,
        MEM_RELEASE = 0x8000,
        MEM_RESERVE = 0x2000,
        PAGE_READWRITE = 4,
        LVIF_TEXT = 0x0001,

    }
    #endregion WindowsMessage
    
}
