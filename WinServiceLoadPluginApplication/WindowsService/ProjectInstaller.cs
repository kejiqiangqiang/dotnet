using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;
using WCFDuplexClientBaseApplication;

namespace WindowsService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            //read from config
            string serviceName = ServiceConfig.ServiceName;
            this.serviceInstaller1.ServiceName = serviceName;
            this.serviceInstaller1.DisplayName = serviceName;
            string serviceDependedOn = ServiceConfig.ServiceDependedOns;
            if (!string.IsNullOrEmpty(serviceDependedOn))
            {
                string[] serviceDependedOns = serviceDependedOn.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                if (serviceDependedOns != null && serviceDependedOns.Length>0)
                {
                    this.serviceInstaller1.ServicesDependedOn = serviceDependedOns;
                }
            }
            
        }
    }
}
