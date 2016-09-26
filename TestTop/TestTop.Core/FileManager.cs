using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TestTop.Core
{
    public static class FileManager
    {
        public static DirectoryInfo ProgramPath { get; private set; }
        public static DirectoryInfo CurrentDesktopPath { get; private set; }
        public static Settings Init { get; private set; }

        public static bool IsInitialized { get; set; }

        public static void Initialize()
        {
            ProgramPath = new DirectoryInfo(Environment.CurrentDirectory);
        }


        public static void SerializeSettings(Settings settings, string path) =>
            File.WriteAllText(path, JsonConvert.SerializeObject(settings));


        public static Settings DeserializeSettings(string path) =>
            JsonConvert.DeserializeObject<Settings>(File.ReadAllText(path));



    }
}
