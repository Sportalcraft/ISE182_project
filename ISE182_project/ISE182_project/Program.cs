using ISE182_project;
using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PersistentLayer;
using ISE182_project.Layers.PresentationLayer;
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

            //Handle all the ecxeption that was no cocaught
            //AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;


            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ChatRoom.start(ChatRoom.Place.University);
            CLI c = new CLI();
            c.initialize();
            //Add code here
           
            Console.ReadKey();
        }


        //This methond handle all the unhendled exceptions
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ConsoleColor colorBefore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            //Log a fatal mesaage
            Logger.Log.Fatal(Logger.Maintenance("something went wrong from unknown reason"), (Exception)e.ExceptionObject);

            Console.ForegroundColor = colorBefore;

            Console.WriteLine("Press any key to exist");
            Console.ReadKey();
            ChatRoom.exit(); //exit the program
        }
    }
}
