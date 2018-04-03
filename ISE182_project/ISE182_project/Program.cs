using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;



namespace MileStoneClient
{
    class Program
    { 
         static void Main(string[] args)
        {
            //Exciptions on english
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            //ChatRoom.register("Tal");
            //ChatRoom.login("Tal");
            Logger.Log.Debug("DEBUG log");
            Logger.Log.Info("INFO log");
            Logger.Log.Warn("WARN log");
            Logger.Log.Error("ERROR log");
            Logger.Log.Fatal("FATAL log");

            Console.ReadKey();
        }
    }
}
