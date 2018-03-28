using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    static class ChatRoom
    {
        private const string url = @"http://ise172.ise.bgu.ac.il:80"; // The url addres of the server
        private static IUser loggedinUser;                                   // Current logged in user

        // logIn an existing user to the server
        public static void logIn(string nickname, int groupID)
        {
            throw new NotImplementedException();
        }

        // receive the last n messages
        public static void requestMessages(int number)
        {
            throw new NotImplementedException();
        }

        //  receive all the messages
        public static void requestAllMessages(int number)
        {
            throw new NotImplementedException();
        }
    }
}
