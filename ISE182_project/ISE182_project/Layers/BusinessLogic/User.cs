using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    [Serializable]
    // This class implaments the IUser interface,
    // and represent a user in the chatroom
    class User : IUser
    {
        private const int GROUP_ID = 32;  // The group ID from the registration sheet
        private string _nickName;         // The nickName chosen by the user

        //Getter to the nickname
        public string NickName
        {
            get { return _nickName; }
        }

        //getter to the group ID
        public int Group_ID
        {
            get { return GROUP_ID; }
        }

        //The constractor of User class
        public User(string nickName)
        {
            _nickName = nickName;
        }

        //A constractor of User class
        public User(string nickName, int GroupID)
        {
            if (Group_ID != GroupID)
                throw new ArgumentException("You cant register to another group!");

            _nickName = nickName;
        }

        //Send a new message to the chatroom and save it
        public void send(string msg, string URL)
        {
            try
            {
                Communication.Instance.Send(URL, Group_ID.ToString(), NickName, msg);
            }
            catch
            {
                throw new Exception("Server was not found!");
            }

            //IMessage Translated = new Message(message); //Traslated to our message' to be able to serialize
            //MessageService.SaveMessage(Translated);
        }

        //logout from the server
        public void logout() { }


        // Cheack if two users are equals.
        // Two useres are equaks if they both have the same group ID and the sane nicknake
        public override bool Equals(object obj)
        {
            if (!(obj is IUser))
                return false;

            IUser other = (IUser)obj;
            return NickName.Equals(other.NickName);
        }

        public override string ToString()
        {
            return "User :\nNickName : " + NickName + "\nGroup ID : " + Group_ID;
        }
    }
}
