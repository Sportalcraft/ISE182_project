using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    interface IUser
    {
        //return the nickname of the user
        string NickName { get; }

        //return the group ID of the user
        int Group_ID { get; }

        //Send a new message to the server
        void send(string msg, string URL);

        //logout the user
        void logout();
    }
}
