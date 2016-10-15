using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using TestTop.Core.JsonFiles;

namespace TestTop.Core
{
    public static class FileManager
    {
        public static void SerializeMainIni(MainIni mainIni, string path) =>
            File.WriteAllText(path, JsonConvert.SerializeObject(mainIni));
        
        public static MainIni DeserializeMainIni(string path) =>
            JsonConvert.DeserializeObject<MainIni>(File.ReadAllText(path));

        public static void SerializeDesktopIni(DesktopIni desktopIni, string path) =>
            File.WriteAllText(path, JsonConvert.SerializeObject(desktopIni));
        
        public static DesktopIni DeserializeDesktopIni(string path) =>
            JsonConvert.DeserializeObject<DesktopIni>(File.ReadAllText(path));


    }
}
