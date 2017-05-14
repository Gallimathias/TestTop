using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoMaS;
using CoMaS.Attributes;

namespace TestTop.TestService.Commands
{
    [Command("/switchdesktop")]
    internal class SwitchCommand : Command<string[], string>
    {
        public SwitchCommand() : base()
        {
            NextFunction = Switch;
        }

        private string Switch(string[] arg)
        {
            RaiseFinishEvent(this, arg);
            return "";
        }
    }
}
