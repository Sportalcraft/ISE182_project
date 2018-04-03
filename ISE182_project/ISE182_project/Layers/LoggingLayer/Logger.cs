using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace ISE182_project.Layers.LoggingLayer
{
    static class Logger
    {
        private static readonly ILog log = LogManager.GetLogger("SEprojectLogger"); // Holds the logger

        //Getter to the logger
        public static ILog Log
        {
            get { return log; }
        }

        //Put a referece at the begennig of the message - Developer :
        public static void Developer(string toLog, string level)
        {
            LogByLevel("Developer : " + toLog, level);
        } 

        //Put a referece at the begennig of the message - Maintenance :
        public static void Maintenance(string toLog, string level)
        {
            LogByLevel("Maintenance : " + toLog, level);
        }


        //--------------------------------------

        // logg a message accirdinf to a givven level
        private static void LogByLevel(string toLog, string level)
        {
            switch (level)
            {
                case "Debug": Log.Debug(toLog); break;
                case "Info":  Log.Info(toLog);  break;
                case "Warn":  Log.Warn(toLog);  break;
                case "Error": Log.Error(toLog); break;
                case "Fatal": Log.Fatal(toLog); break;

                default: Developer("Tried to log " + toLog + " in unknow level of " + level, "Info"); break;
            }
        }

        //Unused code. For now....
        private static ILog GetLogger([CallerFilePath]string filename = "")
        {
            return LogManager.GetLogger(filename);
        }
    }
}
