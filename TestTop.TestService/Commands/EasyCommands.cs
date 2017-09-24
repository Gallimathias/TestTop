using CommandManagementSystem.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTop.TestService.Commands
{
    public static class EasyCommands
    {
        static EasyCommands() => DesktopManager.DoStuff();

        [Command("RestoreIcons")]
        public static string RestoreIcons(string[] arg)
        {
            DesktopManager.CurrentDesktop.DesktopHelper.RestoreIconPositions();
            return "";
        }

        [Command("SaveDesktop")]
        public static string SaveDesktop(string[] arg)
        {
            DesktopManager.CurrentDesktop.Save();
            return "";
        }
    }
}
