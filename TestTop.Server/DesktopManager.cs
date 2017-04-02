using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTop.Core;

namespace TestTop.Server
{
    public static class DesktopManager
    {
        public static List<Desktop> Desktops;

        public static List<Desktop> GetDesktops() => Desktops;

        public static void AddDesktop(Desktop desktop)
        {
            Desktops.Add(desktop);
        }

        public static void UpdateDesktop(Desktop desktop)
        {

        }
    }
}
