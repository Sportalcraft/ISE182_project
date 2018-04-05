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

            //Handle all the ecxeption that was no cocaught
            //AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;


            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            //ChatRoom.register("Tal");
            ChatRoom.login("Tal");
            //ChatRoom.send("Hello world!1");
            //ChatRoom.send("Hello world!2");
            //ChatRoom.send("Hello world!3");
            //ChatRoom.send("Hello world!4");
            //ChatRoom.send("Hello world!5");

            ChatRoom.logout();

            //ChatRoom.register("Me");
            //ChatRoom.login("Me");
            //ChatRoom.send("Hey!1");
            //ChatRoom.send("Hey!2");
            //ChatRoom.send("Hey!3");
            //ChatRoom.send("Hey!4");
            //ChatRoom.send("Hey!5");

            ICollection messages = ChatRoom.requestAllMessagesfromUser("Me", 32);

            foreach (IMessage msg in messages)
            {
                Console.WriteLine(msg);
                Console.WriteLine();
            }

            Console.ReadKey();
        }

        //This methond handle all the unhendled exceptions
        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e)
        {
            ConsoleColor colorBefore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            //Log a fatal mesaage
            Logger.Log.Fatal(Logger.Maintenance("something went wrong from unknown reason"), (Exception)e.ExceptionObject);

            Console.ForegroundColor = colorBefore;

            Console.WriteLine("Press any key to exist");
            Console.ReadKey();
            ChatRoom.exist(); //exit the program
        }
    }
}
