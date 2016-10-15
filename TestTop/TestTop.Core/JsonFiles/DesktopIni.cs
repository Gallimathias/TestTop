using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core.JsonFiles
{
    public class DesktopIni
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "screenshot")]
        public string Screenshot { get; set; }
        [JsonProperty(PropertyName = "backgrounds")]
        public Dictionary<int, string> Backgrounds { get; set; }
    }
}
