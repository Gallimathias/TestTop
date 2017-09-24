using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var process = new Process();
            if (args.Length > 0)
                process.StartInfo = new ProcessStartInfo(args[0]);
            else
                process.StartInfo = new ProcessStartInfo(@"..\..\..\TestTop.TestService\bin\Debug\TestTop.TestService.exe");
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

        }
    }
}
