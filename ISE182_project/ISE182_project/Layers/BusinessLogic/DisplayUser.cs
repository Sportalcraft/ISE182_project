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
    // This class represent a user in the chatroom with the properties and methods that can be publish to the users 
    public class DisplayUser
    {

        private int _groupID;            // The Group ID of the user
        private string _nickName;        // The nickName chosen by the user

        #region Getters & Setters

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

        #endregion

        #region Ctors

        //A constractor of User class
        public DisplayUser(string nickName, int GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (nickName == null || nickName.Equals(""))
            {
                string error = "The client tried to use illegal nickname";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }
            
            if (GroupID < 0)
            {
                string error = "User constractor recived illefal groupID";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            _nickName = nickName;
            _groupID = GroupID;
        }

        #endregion

        #region ToString & Equals

        // Cheack if two users are equals.
        // Two useres are equals if they both have the same group ID and the same nicknake
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

        #endregion
    }
}
