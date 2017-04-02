using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTop.Core.WinAPI;

namespace TestTop
{
    public class Hotkey
    {
        private IntPtr handle { get; set; }
        private int id { get; set; }

        public Hotkey(IntPtr handle, int id, int modifier, Keys key )
        {
            User32.RegisterHotKey(handle,id,modifier,(int)key);
            this.handle = handle;
            this.id = id;
        }

        public void Unregister()
        {
            User32.UnregisterHotKey(handle, id);
            handle = IntPtr.Zero;
        }
    }
}
