using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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

            using (Stream stream = File.Create(Path.Combine(desktop.Dir.Parent.FullName, "options.dt")))
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

            FileManager.SerializeDesktopIni(desktop.ToIni(),
                Path.Combine(desktop.Dir.Parent.FullName, $@"{desktop.Name}.ini"));
        }

        public static void DeSerializer(Desktop desktop)
        {
            List<DesktopIcon> items = new List<DesktopIcon>();

            foreach (var item in desktop.DesktopHelper.Icons)
                items.Add(item);

            using (Stream stream = File.OpenRead(Path.Combine(desktop.Dir.Parent.FullName, "options.dt")))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string key = reader.ReadString();
                        var icon = items.FirstOrDefault(v => v.Name == key);
                        if (icon == null)
                            reader.BaseStream.Position += 8;
                        
                        desktop.DesktopHelper.SetIconPosition(icon, 
                            new Point(reader.ReadInt32(), reader.ReadInt32()));
                    }
                }
            }
        }
    }
}
