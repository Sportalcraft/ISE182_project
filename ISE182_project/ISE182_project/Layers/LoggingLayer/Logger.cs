using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]


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

        #region referece adders

        //can be used to put a referece at the begennig of the message - Developer :
        public static string Developer(string logMessage)
        {
            return "Developer : " + logMessage;
        }

        //can be used to put a referece at the begennig of the message - Maintenance :
        public static string Maintenance(string logMessage)
        {
            return "Maintenance : " + logMessage;
        }

        //can be used to put a referece at the begennig of the message - Server :
        public static string Server(string logMessage)
        {
            return "Server : " + logMessage;
        }

        //can be used to put a state whem a method was entered for debugging
        public static string MethodStart(MethodBase method)
        {
            return MethodStart(method.ToString(), method.DeclaringType.Name);
            // Or : return MethodStart(method.Name, method.DeclaringType.Name);
        }

        #endregion

        //--------------------------------------   

        #region private metods

        //can be used to put a statwe with nethis was entered for debugging
        private static string MethodStart(string methodName, string className)
        {
            return Developer("The method " + methodName + " in class " + className + " was started ");
        }

        #endregion

        #region Unused Code

        //Unused code. For now....
        private static ILog GetLogger([CallerFilePath]string filename = "")
        {
            return LogManager.GetLogger(filename);
        }

        #endregion
    }
}
