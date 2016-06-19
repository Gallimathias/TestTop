using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration.Install;
using System.Collections;
using System.ServiceProcess;

namespace TestTop.Server
{
    [RunInstaller(true)]
    public class TestInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller serviceProcessInstaller;
        
        public override void Install(IDictionary stateSaver)
        {
            serviceInstaller = new ServiceInstaller();
            serviceProcessInstaller = new ServiceProcessInstaller {Account = ServiceAccount.NetworkService };
            
            serviceInstaller.ServiceName = "TestTopServerService";
            serviceInstaller.StartType = ServiceStartMode.Manual;

            Installers.Add(serviceInstaller);
            Installers.Add(serviceProcessInstaller);


            base.Install(stateSaver);

        }
    }
}
