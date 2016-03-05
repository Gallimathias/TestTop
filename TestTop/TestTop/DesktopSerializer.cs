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
                    writer.Write(desktop.ItemPos.Count);

                    foreach (var item in desktop.ItemPos)
                    {
                        writer.Write(item.Key);
                        writer.Write(item.Value.Length);
                        writer.Write(item.Value, 0, item.Value.Length);
                    }
                }
            }
        }

        public static void DeSerializer(Desktop desktop)
        {
            using (Stream stream = File.Open(desktop.Dir.Parent.FullName + "\\options.dt", FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    int count = reader.ReadInt32();
                    for (int i = 0; i < count; i++)
                    {
                        string key = reader.ReadString();
                        int length = reader.ReadInt32();
                        byte[] values = reader.ReadBytes(length);

                        desktop.ItemPos.Add(key, values);
                    }
                }
            }
        }
    }
}
