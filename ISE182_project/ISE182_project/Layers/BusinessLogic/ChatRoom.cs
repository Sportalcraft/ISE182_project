using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{

    //This class hanel the chatroom
    static class ChatRoom
    {
        //private const string URL = @"http://ise172.ise.bgu.ac.il:80"; // The url addres of the server
        private const string URL = @"http://localhost/"; // The url addres of the server at home
        private static IUser _loggedinUser;                           // Current logged in user



        //Geter and setter to the current user
        private static IUser LoggedinUser
        {
            get { return _loggedinUser; }
            set { _loggedinUser = value; }
        }

        #region User

        // register a user to the server
        public static void register(string nickname)
        {
            UserService.register(new User(nickname));
        }

        // logIn an existing user to the server
        public static void login(string nickname)
        {
            IUser user = new User(nickname);

            if (LoggedinUser != null) //Already logged In
                throw new ArgumentException("Alrady loggedin!");

            if (!UserService.canLogIn(user)) //Was regusterd
                throw new ArgumentException("Must register first!");

            LoggedinUser = user;
        }

        // logout the user from the server
        public static void logout()
        {
            if(LoggedinUser == null) //There is no logged in userer
                throw new ArgumentNullException("Must login first to logout!");

            LoggedinUser.logout();
            LoggedinUser = null; // Change the loggedin user to null
        }

        #endregion

        #region Message

        // Send new message to te server
        public static void send(string body)
        {
            if(LoggedinUser == null) // There is no loged in iser
                throw new ArgumentNullException("Must login first to send messages!");

            if (!Message.isValid(body)) // The message is valid
                throw new ArgumentException("The Message is not valid!");

            LoggedinUser.send(body, URL);
            SaveLast10FromServer(); //reciving the last sent 10 messages
        }

        //retrive and sace the last meseges from server
        public static void SaveLast10FromServer()
        {
            MessageService.SaveLast10FromServer(URL);
        }

        // Receive the last n messages
        public static ArrayList requestMessages(int number)
        {
            return MessageService.lastNmesages(number);
        }

        // Receive all the messages
        public static ArrayList requestAllMessages()
        {
            return MessageService.RamMessages;
        }

        #endregion
    }
}
