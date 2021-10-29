/*
* FILE          : Program.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : In this assignment, we are to create a service using the code we had made in A-05 which utilized a chat server hosting multiple clients to talk amongst
*                 themselves. The only part that is different is that the server must now act as a service instead of a console application, logs are sent to a text file where
*                 necessary and to the event viewer as well. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Server_Service
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }

   
}
