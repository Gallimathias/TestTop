using System;
using System.Windows.Forms;
using TestTop.Core.WinAPI;

namespace TestTop.UI
{
    public class Hotkey
    {
        private IntPtr Handle { get; set; }
        private int Id { get; set; }

        public Hotkey(IntPtr handle, int id, int modifier, Keys key )
        {
            User32.RegisterHotKey(handle,id,modifier,(int)key);
            Handle = handle;
            Id = id;
        }

        public void Unregister()
        {
            User32.UnregisterHotKey(Handle, Id);
            Handle = IntPtr.Zero;
        }
    }
}
