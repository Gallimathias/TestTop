using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestTop.Basic;

namespace TestTop
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
