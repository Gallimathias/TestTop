using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTop.Core.WinAPI;

namespace TestTop.Forms
{
    public class Hotkey
    {
        private IntPtr Handle { get; set; }
        private int Id { get; set; }

        public Hotkey(IntPtr handle, int id, int modifier, Keys key )
        {
            User32.RegisterHotKey(handle,id,modifier,(int)key);
            this.Handle = handle;
            this.Id = id;
        }

        public void Unregister()
        {
            User32.UnregisterHotKey(Handle, Id);
            Handle = IntPtr.Zero;
        }
    }
}
