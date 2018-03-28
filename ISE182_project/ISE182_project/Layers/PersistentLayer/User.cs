using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    class User : IUser
    {
        private const int GROUP_ID = 32;  // The group ID from the registration sheet
        private string _nickName;         // The nickName chosen by the user

        public string NickName
        {
            get { return _nickName; }
            // set { _nickName = value; }
        }

        public User(string nickName)
        {
            _nickName = nickName;
        }

        public IMessage send(string msg)
        {
            throw new NotImplementedException();
        }

        public void logout() { }

        public override bool Equals(object obj)
        {
            if (!(obj is IUser))
                return false;

            IUser other = (IUser)obj;
            return NickName.Equals(other.NickName);

        }
    }
}
