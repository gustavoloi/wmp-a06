/*
* FILE          : Program.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : Simple test harness for the logs(both the event log and the txt log file)
*
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server_Service;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Testing the log");
            Logger.LogTxt("Creating as file");
        }
    }
}
