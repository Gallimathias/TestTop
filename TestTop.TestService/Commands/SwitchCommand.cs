using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.TestService.Commands
{
    [Command("/switchdesktop")]
    internal class SwitchCommand : Command<string[], string>
    {
        public SwitchCommand() : base()
        {

        }

        public override string Main(string[] arg)
        {
            return base.Main(arg);
        }
     
    }
}
