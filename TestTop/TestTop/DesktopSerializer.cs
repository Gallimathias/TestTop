using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestTop
{
    class DesktopSerializer
    {
        public static void Serialize(Desktop desktop)
        {
            
            using (Stream stream = File.Open(desktop.Dir.Parent.FullName + "\\options.dt", FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(desktop.DesktopHelper.Icons.Length);
                    foreach (var item in desktop.DesktopHelper.Icons)
                    {
                        writer.Write(item.Name);
                        writer.Write(item.Location.X);
                        writer.Write(item.Location.Y);
                    }
                }
            }
        }

        public static void DeSerializer(Desktop desktop)
        {
            Dictionary<string, DesktopIcon> items = new Dictionary<string, DesktopIcon>();
            foreach (var item in desktop.DesktopHelper.Icons)
                items.Add(item.Name, item);
            using (Stream stream = File.Open(desktop.Dir.Parent.FullName + "\\options.dt", FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string key = reader.ReadString();
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        DesktopIcon icon; 
                        if (items.TryGetValue(key, out icon))
                            desktop.DesktopHelper.SetIconPosition(icon, new System.Drawing.Point(x, y));
                    }
                }
            }
        }
    }
}
