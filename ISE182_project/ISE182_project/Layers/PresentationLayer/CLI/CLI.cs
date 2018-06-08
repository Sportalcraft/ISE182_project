using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;

namespace ISE182_project.Layers.PresentationLayer.CLI
{
    class CLI
    {
        // CLI implemented as a singleton
        private static CLI instance;

        // CLI implemented as a singleton
        private CLI()
        {
            ChatRoom.start(ChatRoom.Place.University); //HERE
        }
        public static CLI Instance
        {
            get
            {
                if (instance == null)
                    instance = new CLI();
                return instance;
            }
        }
        // initialize(): used in order to initialize the CLI, introducing the user to the chat room instructions and allows him to interect with the relevant menu
        public void initialize()
        {
            Console.WriteLine("Hello, welcome to our ChatRoom!");
            menuNotification();
            while (true)
            { // This loop allows moving between menus without calling them multiple times in different methods
                while (!ChatRoom.isLoggedIn())
                    entranceManager();
                while (ChatRoom.isLoggedIn())
                    selectionMenu();
            }
        }
        // This menu handels a client which isn't logged-in in the chat room
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
                else if (selected.Key == ConsoleKey.C)
                    exitFunction();
                else
                    menuNotification();
            }
        }
        // This menu handels a client has logged-in
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
        // Trying to retrieve last 10 messages from server, reponds accordingly if the attempt was successful or not
        private void retrieveMessages()
        {
            try
            {
                ChatRoom.DrawLastMessages();
                boldingText("The 10 last messages were retrieved", ConsoleColor.Cyan);
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }
        }
        // Trying to display last 20 messages from server, reponds accordingly if the attempt was successful or not
        private void display20Messages()
        {
            try
            {
                boldingText("20 Last Messages:", ConsoleColor.Cyan);
                Printer(ChatRoom.request20Messages());
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }
        }
        // Trying to display all retrieved messages of a certain user, reponds accordingly if the attempt was successful or not
        private void displayUserMessages()
        {
            bool parametersReceived = false;
            int groupID = -1;
            string username = "";
            while (username.Equals(""))
            {
                Console.Write("Please enter the user's username in order to see all of his messages: ");
                username = Console.ReadLine();
            }
            // Checking groupID's validety - the relevant function in the logic-layer recieves int, therefore we must check it here.
            while (!parametersReceived)
            {
                Console.Write("Please enter the user's groupID in order to see all of his messages: ");
                string groupIDstring = Console.ReadLine();
                try // Only if the user's input was an int, the while loop will end
                {
                    groupID = int.Parse(groupIDstring);
                    parametersReceived = true;
                }
                catch
                {
                }
            }
            try
            {
                ICollection<IMessage> list = ChatRoom.requestMessagesfromUser(username, groupID); // An array of username's messages
                if (list.Count == 0)
                {
                    boldingText(username + " have no messages", ConsoleColor.Cyan);
                }
                else
                {
                    boldingText(username + "'s messages are:", ConsoleColor.Cyan);
                    Printer(list);
                }
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }

        }
        // This function allows a user to send a new message, only if under 150chars.
        private void writeMessage()
        {
            Console.Write("Please insert your text (No more than 150 characters: ");
            string body = Console.ReadLine();
            try
            {
                ChatRoom.send(body);
                boldingText("Message was successfully sent!", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }
        }
        // Handles login-out
        private void logoutFunction()
        {
            try
            {
                ChatRoom.logout();
                boldingText("You have successfully logged out", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }


        }
        // A message that pops-up when a user is pressing irrelevant keys (Instructions for menus)
        private void menuNotification()
        {
            boldingText("While facing a menu, press the requested key in order to select the option you desire.", ConsoleColor.Red);
        }
        private void boldingText(string text, ConsoleColor color) // Displayes the text in the requested color
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        // Handles login-in
        private void login()
        {
            Console.Write("In order to login, please enter your username and press <Enter>: ");
            string username = Console.ReadLine();
            try
            {
                ChatRoom.login(username,32);
                boldingText("You have successfully logged-in", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }
        }
        // Handles registration
        private void register()
        {
            Console.Write("In order to register, please select a username and press <Enter>: ");
            string username = Console.ReadLine();
            try
            {
                ChatRoom.register(username,32);
                boldingText("You have successfully registered, pls login now", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                boldingText(e.Message, ConsoleColor.Red);
            }
        }
        // Handles exit request
        private void exitFunction()
        {
            ChatRoom.exit();
        }
        // An easy way to print the relevant lists received from tha ChatRoom class
        private void Printer<T>(ICollection<T> list)
        {
            foreach (object o in list)
            {
                Console.WriteLine(o.ToString());
                Console.WriteLine("\\==================================\\");
            }
        }
    }
}
