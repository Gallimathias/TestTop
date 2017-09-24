using CommandManagementSystem;
using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.TestService
{
    [CommandManager("CommandManager", "TestTop.TestService.Commands")]
    public class CommandManager : CommandManager<string[], string>
    {
       
    }
}
