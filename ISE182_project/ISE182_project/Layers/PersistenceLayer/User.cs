using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistenceLayer
{
    class User
    {
        private int _groupID;     // The group ID from the registration sheet
        private string _nickName; // The nickName chosen by the user

        public int GroupID
        {
            get { return _groupID; }
            //set { _groupID = value; }
        }

        public string NickName
        {
            get { return _nickName; }
           // set { _nickName = value; }
        }

        public IMessage send(string msg)
        {
            throw new NotImplementedException();
        }

        public void logout()
        {
            throw new NotImplementedException();
        }
    }
}
