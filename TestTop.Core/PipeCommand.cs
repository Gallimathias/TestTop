using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.Core
{
    public class PipeCommand
    {
        public string Command { get; set; }
        public string[] Args { get; set; }

        public PipeCommand(string command) => Command = command;
        public PipeCommand(string command, params string[] args) : this(command)=> Args = args;

    }
}
