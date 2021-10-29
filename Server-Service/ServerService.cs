/*
* FILE          : ServerService.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : This class handles the service part of the server. It calls the methods that are present in the ServerClass and Logger classes. When anything is one in this
*                 class, it is logged in a txt file on the same folder as the service.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Server_Service
{
    public partial class ServerService : ServiceBase
    {
        public ServerService()
        {
            InitializeComponent(); //Initialize the service only
        }

        /* FUNCTION     : OnStart
        * DESCRIPTION   : This method is called when the service starts. It also logs the action in the log file using the logger.      
        */
        protected override void OnStart(string[] args)
        {
            ServerClass.StartServerThread(); 
            string startLog = "Server Service Started";
            Logger.LogTxt(startLog);
        }

        /* FUNCTION     : OnStop
        * DESCRIPTION   : This method is called when the service stops. It also logs the action in the log file using the logger.
        */
        protected override void OnStop()
        {
            ServerClass.StopServerThread();
            string stopLog = "Server Service Stopped";
            Logger.LogTxt(stopLog);
        }
    }
}
