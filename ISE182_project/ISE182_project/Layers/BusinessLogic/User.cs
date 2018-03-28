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
        public int Goup_ID
        {
            get { return GROUP_ID; }
        }

        //The constractor of User class
        public User(string nickName)
        {
            _nickName = nickName;
        }

        //Send a new message to the chatroom
        public IMessage send(string msg)
        {
            return ChatRoom.send(msg);
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
    }
}
