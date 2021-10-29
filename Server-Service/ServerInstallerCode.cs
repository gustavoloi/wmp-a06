using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace Server_Service
{
    [RunInstaller(true)]
    public partial class ServerInstallerCode : System.Configuration.Install.Installer
    {
        public ServerInstallerCode()
        {
            InitializeComponent();
        }
    }
}
