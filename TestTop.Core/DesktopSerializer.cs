using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TestTop.Core
{
    static class DesktopSerializer
    {

        public static void Serialize(Desktop desktop)
        {
            desktop.DesktopHelper.UpdateIcons();
            using (Stream stream = File.Create(desktop.Dir.Parent.FullName + "\\options.dt"))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(desktop.DesktopHelper.Icons.Count);
                    foreach (var item in desktop.DesktopHelper.Icons)
                    {
                        writer.Write(item.Name);
                        writer.Write(item.Location.X);
                        writer.Write(item.Location.Y);
                    }
                }
            }

            FileManager.SerializeDesktopIni(desktop.ToIni(), $@"{desktop.Dir.Parent.FullName}\{desktop.Name}.ini");
        }

        public static void DeSerializer(Desktop desktop)
        {
            List<DesktopIcon> items = new List<DesktopIcon>();
            foreach (var item in desktop.DesktopHelper.Icons)
                items.Add(item);
            using (Stream stream = File.OpenRead(desktop.Dir.Parent.FullName + "\\options.dt"))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string key = reader.ReadString();
                        int x = reader.ReadInt32();
                        int y = reader.ReadInt32();
                        var icon = items.FirstOrDefault(v => v.Name == key); 
                        if (icon != null)
                            desktop.DesktopHelper.SetIconPosition(icon, new System.Drawing.Point(x, y));
                    }
                }
            }
        }
    }
}
