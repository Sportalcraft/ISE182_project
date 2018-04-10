using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{

    //This class hanel the chatroom
    static class ChatRoom
    {
        private const string HOME_URL = @"http://localhost/";             // The url addres of the server at home
        private const string BGU_URL = @"http://ise172.ise.bgu.ac.il:80"; // The url addres of the server
        private static Place _location;                                   // The location of the client
        private static IUser _loggedinUser;                               // Current logged in user

        #region General

        //The location of the client
        public enum Place
        {
            Home,
            University
        }

        //Getter to the URL
        private static string URL
        {
            get
            {
                switch (_location)
                {
                    case Place.Home: return HOME_URL;
                    case Place.University: return BGU_URL;
                    default:
                        {                      
                            string error = "recived unknown enum value.";
                            Logger.Log.Fatal(Logger.Developer(error));

                            throw new ArgumentOutOfRangeException(error);
                        }
                }
 
            }
        }

        //Geter and setter to the current user
        private static IUser LoggedinUser
        {
            get { return _loggedinUser; }
            set { _loggedinUser = value; }
        }

        //Initiating the ram's saves from disk
        public static void start(Place location)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            UserService.start();    // Initiating mesaages on ram
            MessageService.start(); // Initiating users on ram
            _location = location;   // Set the location
        }

        //exit the program
        public static void exit()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (LoggedinUser != null)
                logout(); //logout first

            Logger.Log.Info(Logger.Maintenance("The client closed the program"));
            Environment.Exit(1); //Exiting
        }

        #endregion

        #region User

        // return if tere is an loggedin user
        public static bool isLoggedIn()
        {
            return LoggedinUser != null;
        }

        // register a user to the server
        public static void register(string nickname)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (isLoggedIn()) //Already logged In
            {
                string error = "A user tried to register while loggedin to: " + LoggedinUser;
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            UserService.register(new User(nickname)); //register
        }

        // logIn an existing user to the server
        public static void login(string nickname)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            IUser user = new User(nickname);

            if (isLoggedIn()) //Already logged In
            {
                string error = "A user tried to login while loggedin to: " + LoggedinUser;
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            if (!UserService.canLogIn(user)) //Was regusterd
            {
                string error = "A user tried to login to a not register account";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            LoggedinUser = user; //log in
            Logger.Log.Info(Logger.Maintenance("The user " + user + " loggedin"));
        }

        // logout the user from the server
        public static void logout()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!isLoggedIn()) //There is no logged in userer
            {
                string error = "A user tried to logout without being logedin to a user";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            LoggedinUser.logout();
            LoggedinUser = null; // Change the loggedin user to null
        }

        #endregion

        #region Message

        // Send new message to te server
        public static void send(string body)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            LoggedinUser.send(body, URL); // Sending
            SaveLast10FromServer();       // reciving the last sent 10 messages
        }

        //retrive and sace the last meseges from server
        public static void SaveLast10FromServer()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            MessageService.SaveLast10FromServer(URL);
        }

        // Receive the last 20 messages
        public static ArrayList request20Messages()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return requestMessages(20);
        }

        // Receive all the messages
        public static ArrayList requestAllMessagesfromUser(string nickName, int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.AllMessagesFromUser(new User(nickName, GroupID));
        }


        // ----------------------------------------------------------

        // Receive the last n messages
        private static ArrayList requestMessages(int number)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.lastNmesages(number);
        }

        #endregion
    }
}
