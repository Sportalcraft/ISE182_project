using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISE182_project.Layers.BusinessLogic;

namespace ISE182_project.Layers.PresentationLayer
{
    class CLI
    {
        // Will most likely have a ChatRoom property in order to call the required Methods
        //private static List<ConsoleKey> validKeys = { ConsoleKey.A, };

        public CLI()
        {
            Console.WriteLine("Hello, welcome to our ChatRoom!");
            menuNotification();
        }
        public void initialize()
        {
            while (true) {
            while (!ChatRoom.isLoggedIn())
                entranceManager();
            while (ChatRoom.isLoggedIn())
                selectionMenu();
            }
        }
        private void entranceManager()
        {
            Console.WriteLine("In order to use our services, please login first. If you don't have a user, please register first:");
            Console.WriteLine("a) Login");
            Console.WriteLine("b) Register");
            boldingText("c) Exit ChatRoom", ConsoleColor.Red);
            while (!ChatRoom.isLoggedIn())
            {
                ConsoleKeyInfo selected = Console.ReadKey();
                Console.WriteLine();
                if (selected.Key == ConsoleKey.A)
                    login();
                else if (selected.Key == ConsoleKey.B)
                    register();
                else if (selected.Key == ConsoleKey.C) // Placement needs to be changed
                    exitFunction();
                else
                    menuNotification();
            }
        }
        private void selectionMenu()
        {
            Console.WriteLine("Please select your desired option:");
            Console.WriteLine("a) Retrieve 10 messages from the server");
            Console.WriteLine("b) Display 20 messages");
            Console.WriteLine("c) Display user's messages");
            Console.WriteLine("d) Write a new message");
            Console.WriteLine("e) Logout - allows exiting from the ChatRoom");
            ConsoleKeyInfo selected = Console.ReadKey();
            Console.WriteLine();
            if (selected.Key == ConsoleKey.A)
                retrieveMessages();
            else if (selected.Key == ConsoleKey.B)
                display20Messages();
            else if (selected.Key == ConsoleKey.C)
                displayUserMessages();
            else if (selected.Key == ConsoleKey.D)
                writeMessage();
            else if (selected.Key == ConsoleKey.E)
                logoutFunction();
            else
                menuNotification();
        }
        //private bool checkKeyValidity(ConsoleKey key)
        //{

        //}
        private void retrieveMessages()
        {
            ChatRoom.SaveLast10FromServer();
            Console.WriteLine("The 10 last messages were retrieved");
        }
        private void display20Messages()
        {
            boldingText("20 Last Messages:", ConsoleColor.Cyan);
            ChatRoom.request20Messages().ToString();
        }
        private void displayUserMessages()
        {
            bool parametersReceived = false;
            int groupID = -1;
            Console.Write("Please enter the user's username in order to see all of his messages: ");
            string username = Console.ReadLine();
            while (!parametersReceived)
            {
                Console.Write("Please enter the user's groupID in order to see all of his messages: ");
                string groupIDstring = Console.ReadLine();
                try
                {
                        groupID = int.Parse(groupIDstring);
                        parametersReceived = true;
                }
                catch
                {
                }
            }
            boldingText(username + "'s messages are:", ConsoleColor.Cyan);
            ChatRoom.requestAllMessagesfromUser(username, groupID).ToString();

        }
        private void writeMessage()
        {
            Console.Write("Please insert your text: ");
            string body = Console.ReadLine();
            ChatRoom.send(body);
        }
        private void logoutFunction()
        {
            ChatRoom.logout();
            boldingText("You have successfully logged out", ConsoleColor.Green);
        }
        private void menuNotification()
        {
            Console.WriteLine("While facing a menu, press the requested key in order to select the option you desire.");
        }
        private void boldingText(string text, ConsoleColor color) // Displayes the text in red color
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private void login()
        {
            Console.Write("In order to login, please enter your username and press <Enter>: ");
            string username = Console.ReadLine();
            try {
            ChatRoom.login(username); // Calling login function
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void register()
        {
            Console.Write("In order to register, please select a username and press <Enter>: ");
            string username = Console.ReadLine();
            ChatRoom.register(username); // Calling register function
        }
        private void exitFunction()
        {
            // Inserting all new masseges into server might occure here, if needed
            Environment.Exit(0);
        }
    }
}
