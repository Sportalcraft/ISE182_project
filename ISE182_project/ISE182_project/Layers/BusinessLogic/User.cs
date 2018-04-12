using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    [Serializable]
    // This class implaments the IUser interface,
    // and represent a user in the chatroom
    class User : IUser
    {
        private const int GROUP_ID = 32; // Our group ID from the registration sheet

        private int _groupID;            // The Group ID of the user
        private string _nickName;        // The nickName chosen by the user


        //Getter to the nickname
        public string NickName
        {
            get { return _nickName; }
        }

        //getter to the group ID
        public int Group_ID
        {
            get { return _groupID; }
        }

        //The constractor of User class
        public User(string nickName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (nickName == null || nickName.Equals(""))
            {
                string error = "The client tried to use illegal nickname";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            _nickName = nickName;
            _groupID = GROUP_ID;
        }

        //A constractor of User class
        public User(string nickName, int GroupID) : this(nickName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (GroupID < 0)
            {
                string error = "User constractor recived illefal groupID";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            if (GroupID != GROUP_ID)
            {
                Logger.Log.Warn(Logger.Maintenance("An instance of a user of a different group was created : NickName - " + nickName + " (GrouoID  : " + GroupID + ")" ));
            }

            _groupID = GroupID;
        }

        //Send a new message to the chatroom and save it
        public void send(string msg, string URL)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!Message.isValid(msg))
            {
                string error = "The message the user requsted to send is illegal";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            if (URL == null || URL.Equals(""))
            {
                string error = "The url the user tried to send to is illegal";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            try
            {
                Communication.Instance.Send(URL, Group_ID.ToString(), NickName, msg);
            }
            catch
            {
                string error = "Server was not found!";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw;
            }
        }

        //logout from the server
        public void logout()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            Logger.Log.Info(Logger.Maintenance("The user " + NickName + " (GroupID : " + Group_ID + ") loggedout."));
        }

        // Cheack if two users are equals.
        // Two useres are equaks if they both have the same group ID and the same nicknake
        public override bool Equals(object obj)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!(obj is IUser))
                return false;

            IUser other = (IUser)obj;
            return NickName.Equals(other.NickName) & Group_ID.Equals(other.Group_ID);
        }

        //return a string that represent this user
        public override string ToString()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return "NickName : " + NickName + ", Group ID : " + Group_ID;
        }
    }
}
