using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
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
            //ChatRoom.register("Tal");
            ChatRoom.login("Tal");

            Console.ReadKey();
        }
    }
}
