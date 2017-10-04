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
            DesktopManager.Desktops.FirstOrDefault(x => x.Name == arg[0]).DesktopHelper.RestoreIconPositions();
            //DesktopManager.CurrentDesktop.DesktopHelper.RestoreIconPositions();
            return "";
        }

        [Command("SaveDesktop")]
        public static string SaveDesktop(string[] arg)
        {
            DesktopManager.Desktops.FirstOrDefault(x => x.Name == arg[0]).Save();
            //DesktopManager.CurrentDesktop.Save();
            return "";
        }
        [Command("Switch")]
        public static string SwitchDesktop(string[] arg)
        {
            DesktopManager.Switch("test");
            return "";
        }
        [Command("SwitchBack")]
        public static string SwitchBack(string[] arg)
        {
            DesktopManager.SwitchBack();
            return "";
        }
        [Command("IAmAlive")]
        public static string Alive(string[] arg)
        {
            if (!MainService.Clients.Contains(arg[0]))
                MainService.Clients.Add(arg[0]);
            Console.WriteLine(arg[0]);
            return "";
        }
    }
}
