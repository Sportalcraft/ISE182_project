using ISE182_project.Layers.CommunicationLayer;
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

            UserService.Instence.start();    // Initiating mesaages on ram
            MessageService.Instence.start(); // Initiating users on ram
            _location = location;   // Set the location
        }

        //exit the program
        public static void exit()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (isLoggedIn())
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

            UserService.Instence.register(new User(nickname)); //register
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

            if (!UserService.Instence.canLogIn(user)) //Was regusterd
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

            if(!isLoggedIn())
            {
                string error = "You cant send a message without login first!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            LoggedinUser.send(body, URL); // Sending
            SaveLast10FromServer();       // reciving the last sent 10 messages
        }

        //retrive and sace the last meseges from server
        public static void SaveLast10FromServer()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            MessageService.Instence.SaveLast10FromServer(URL);
        }

        // Receive the last 20 messages
        public static ICollection<IMessage> request20Messages()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return requestMessages(20);
        }

        #region Sort

        //Sort a message List by the time
        public static ICollection<IMessage> sort(ICollection<IMessage> messages, MessageService.Sort SortBy, bool descending)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.sort(messages, SortBy, descending);

        }

        #endregion

        #region Filter

        // Receive all the messages from a certain user
        public static ICollection<IMessage> requestAllMessagesfromUser(string nickName, int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.FilterByUser(new User(nickName, GroupID));
        }

        // Receive all the messages from a certain user from a certain collection
        public static ICollection<IMessage> GetMessagesfromUser(ICollection<IMessage> messages, string nickName, int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.FilterByUser(new User(nickName, GroupID), messages);
        }

        // Receive all the messages from a certain group
        public static ICollection<IMessage> requestAllMessagesfromGroup(int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.FilterByGroup(GroupID);
        }

        // Receive all the messages from a certain group from a certain collection
        public static ICollection<IMessage> GetMessagesfromGroup(ICollection<IMessage> messages, int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.FilterByGroup(GroupID, messages);
        }

        #endregion

        // ----------------------------------------------------------

        // Receive the last n messages
        private static ICollection<IMessage> requestMessages(int number)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return MessageService.Instence.lastNmesages(number);
        }
        #endregion
    }
}
