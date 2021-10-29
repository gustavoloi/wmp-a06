/*
* FILE          : Logger.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : This class contains the logger functions, which are used to log the actions of the service to the event viewer or a log file, since the service itself has no UI.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace Server_Service
{
    public static class Logger
    {
        /* FUNCTION     : Log(string message)
         * DESCRIPTION  : This method logs a message into the log function of windows, and can be seen with more details in the Computer Management screen.    
         * PARAMETERS   :  string message   -   The message that is going to be logged
         *
         */
        public static void Log(string message)
        {
            EventLog serviceEventLog = new EventLog();
            if (!EventLog.SourceExists("MySource")) //Check if there is an event log already there
            {
                EventLog.CreateEventSource("MySource", "MyEventLog");
            }

            serviceEventLog.Source = "MyEventSource";
            serviceEventLog.Log = "MyEventLog";
            serviceEventLog.WriteEntry(message);
        }
        /* FUNCTION     : Log(string message)
         * DESCRIPTION  : Sends logs over to the Event Viewer   
         * PARAMETERS   : string logMessage   -   The message that is going to be logged
         *
         */
        public static void LogTxt(string logMessage)
        {
            try
            {
                WriteToFile(logMessage);
            }
            catch (Exception e) //Catch and display the exception
            {
                WriteToFile(e.ToString());
            }
        }
        /* FUNCTION     : WriteToFile
         * DESCRIPTION  :  Writes to the log text file in the same directory as the executable. If the file does not exist, then it will create it.  
         * PARAMETERS   :  string messageToWrite   -   The message that is going to be logged
         *
         */
        public static void WriteToFile(string messageToWrite)
        {
            string localPath = AppDomain.CurrentDomain.BaseDirectory + "\\log.txt";
            string currentDate = DateTime.Now.ToString("yyy/MM/dd HH:mm:ss");
            using (StreamWriter file = new StreamWriter(localPath, true)) // Setting up the stream writer
            {
                file.WriteLine(currentDate + " => " + messageToWrite); // Append to the file
            }
        }
    }
}
