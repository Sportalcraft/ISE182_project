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
        private bool userLoggedIn;
        private ConsoleKeyInfo selected;
        // Will most likely have a ChatRoom property in order to call the required Methods
        //private static List<ConsoleKey> validKeys = { ConsoleKey.A, };

        public CLI()
        {
            this.userLoggedIn = false;
            Console.WriteLine("Hello, welcome to our ChatRoom!");
            menuNotification();
        }
        public void initialize()
        {
            while (this.userLoggedIn != true)
                entranceManager();
            while (this.userLoggedIn == true)
                selectionMenu();
        }
        private void entranceManager()
        {
            Console.WriteLine("In order to use our services, please login first. If you don't have a user, please register first:");
            Console.WriteLine("a) Login");
            Console.WriteLine("b) Register");
            boldingText("c) Exit ChatRoom");
            while (this.userLoggedIn != true)
            {
                this.selected = Console.ReadKey();
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
            Console.WriteLine("a) Retrive 10 massages from the server");
            Console.WriteLine("b) Display massages");
            Console.WriteLine("c) Write a new massage");
            Console.WriteLine("d) Logout - allows exiting from the ChatRoom");
            this.selected = Console.ReadKey();
            if (selected.Key == ConsoleKey.A)
                retriveMassages();
            else if (selected.Key == ConsoleKey.B)
                displayMassages();
            else if (selected.Key == ConsoleKey.C)
                writeMassage();
            else if (selected.Key == ConsoleKey.D)
                logoutFunction();
            else
                menuNotification();
        }
        //private bool checkKeyValidity(ConsoleKey key)
        //{

        //}
        private void retriveMassages()
        {
            throw new NotImplementedException();
        }
        private void displayMassages()
        {
            throw new NotImplementedException();
        }
        private void writeMassage()
        {
            throw new NotImplementedException();
        }
        private void logoutFunction()
        {
            throw new NotImplementedException();
        }
        private void menuNotification()
        {
            Console.WriteLine("While facing a menu, press the requested key in order to select the option you desire.");
        }
        private void boldingText(string text) // Displayes the text in red color
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        private void login()
        {
            Console.Write("In order to login, please enter your username and press <Enter>: ");
            string username = Console.ReadLine();
            Console.Write("Please enter your password and press <Enter>: ");
            string password = Console.ReadLine();
            // Calling login function
            this.userLoggedIn = true; // this.userLoggedIn = bool returned from login function
        }
        private void register()
        {
            Console.Write("In order to register, please select a username and press <Enter>: ");
            string username = Console.ReadLine();
            Console.Write("Please enter a password and press <Enter>: ");
            string password = Console.ReadLine();
            // Calling register function
            this.userLoggedIn = true; // this.userLoggedIn = bool returned from register function
        }
        private void exitFunction()
        {
            // Inserting all new masseges into server might occure here, if needed
            Environment.Exit(0);
        }
    }
}
