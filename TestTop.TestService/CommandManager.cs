using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.TestService
{
    [CoMaS.Attributes.CommandManager("CommandManager", "TestTop.TestService.Commands")]
    public class CommandManager : CoMaS.CommandManager<string[], string>
    {

    }
}
