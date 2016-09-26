using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core
{
    public class Settings
    {
        public List<string> Desktops { get; set; }

        public Settings()
        {
            Desktops = new List<string>();
        }

        public void AddDesktop(Desktop desktop)
        {
            Desktops.Add(desktop.Name);
        }

    }
}
