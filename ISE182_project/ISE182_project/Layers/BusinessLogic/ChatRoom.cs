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
        private const string URL = @"http://ise172.ise.bgu.ac.il:80"; // The url addres of the server
        private static IUser _loggedinUser;                           // Current logged in user

        //Geter and setter to the current user
        private static IUser LoggedinUser
        {
            get { return _loggedinUser; }
            set { _loggedinUser = value; }
        }

        //retrive and sace the last meseges from server
        public static void SaveLast10FromServer()
        {
            MessageService.SaveLast10FromServer(URL);
        }

        // register a user to the server
        public static void register(string nickname)
        {
            UserService.register(new User(nickname));
        }

        // logIn an existing user to the server
        public static void login(string nickname)
        {
            IUser user = new User(nickname);

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
            LoggedinUser = null; //Change the loggedin user to null
        }

        // Sebd new message to te server
        public static IMessage send(string body)
        {
            return Communication.Instance.Send(URL, LoggedinUser.Goup_ID.ToString(), LoggedinUser.NickName, body);
        }

        // receive the last n messages
        public static ArrayList requestMessages(int number)
        {
            return MessageService.lastNmesages(number);
        }

        //  receive all the messages
        public static ArrayList requestAllMessages(int number)
        {
            return MessageService.RamMessages;
        }
    }
}
