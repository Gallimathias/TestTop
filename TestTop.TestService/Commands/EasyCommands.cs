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
        public static bool RestoreIcons(string[] arg)
        {
            DesktopManager.Desktops.FirstOrDefault(x => x.Name == arg[0]).DesktopHelper.RestoreIconPositions();
            //DesktopManager.CurrentDesktop.DesktopHelper.RestoreIconPositions();
            return true; //TODO: check if successfull
        }

        [Command("SaveDesktop")]
        public static bool SaveDesktop(string[] arg)
        {
            DesktopManager.Desktops.FirstOrDefault(x => x.Name == arg[0]).Save();
            //DesktopManager.CurrentDesktop.Save();
            return true; //TODO: check if successfull
        }

        [Command("Switch")]
        public static bool SwitchDesktop(string[] arg)
        {
            DesktopManager.Switch("test4");
            return true; //TODO: check if successfull
        }

        [Command("NewDesktopSwitch")]
        public static bool NewDesktopSwitch(string[] arg)
        {
            DesktopManager.NewDesktopSwitch(arg[0], arg[1]);
            return true; //TODO: check if successfull
        }

        [Command("SwitchBack")]
        public static bool SwitchBack(string[] arg)
        {
            DesktopManager.SwitchBack();
            return true; //TODO: check if successfull
        }

        [Command("IAmAlive")]
        public static bool Alive(string[] arg)
        {
            if (!MainService.Clients.Contains(arg[0]))
                MainService.Clients.Add(arg[0]);

            Console.WriteLine(arg[0]);
            return true; //TODO: check if successfull
        }
    }
}
