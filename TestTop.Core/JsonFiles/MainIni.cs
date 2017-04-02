using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TestTop.Core.JsonFiles
{
    public class MainIni
    {
        [JsonProperty(PropertyName ="desktops")]
        public List<string> Desktops { get; set; }
        [JsonProperty(PropertyName = "desktopHotkeys")]
        public Dictionary<string,string> DesktopHotkeys { get; set; }
        
    }
}
