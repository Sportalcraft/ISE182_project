using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MileStoneClient
{
    class Program
    { 
         static void Main(string[] args)
        {
            //Exciptions on english
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en-US");

            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

              //ChatRoom.register("Tal");
              ChatRoom.login("Tal");
              ChatRoom.send("Hello world!");



            Console.ReadKey();
        }
    }
}
