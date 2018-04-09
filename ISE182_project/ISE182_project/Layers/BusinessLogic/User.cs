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
                Logger.Log.Error(Logger.Maintenance("The client tried to register with illegal nickname"));
            }

            _nickName = nickName;
            _groupID = GROUP_ID;
        }

        //A constractor of User class
        public User(string nickName, int GroupID) : this(nickName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (GroupID != GROUP_ID)
            {
                Logger.Log.Warn(Logger.Maintenance("An instance of a user of a different group was created"));
            }

            _groupID = GroupID;
        }

        //Send a new message to the chatroom and save it
        public void send(string msg, string URL)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!Message.isValid(msg))
            {
                Logger.Log.Error(Logger.Maintenance("recived an illegal message to send"));
                return;
            }

            if (URL == null || URL.Equals(""))
            {
                Logger.Log.Error(Logger.Maintenance("recived an illegal url"));
                return;
            }

            try
            {
                Communication.Instance.Send(URL, Group_ID.ToString(), NickName, msg);
            }
            catch
            {
                Logger.Log.Fatal(Logger.Maintenance("Server was not found!"));
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
