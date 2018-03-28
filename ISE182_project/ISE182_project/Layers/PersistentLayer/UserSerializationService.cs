using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{

    //This class enable serializtion and deserializtion of users to the disk
    class UserSerializationService
    {
        private const string USERS_LIST = "Users.bin"; // The file name to save the users

        //Serialze a list of users
        public static void serialize(ArrayList users)
        {
            SerializationService.serialize(users, USERS_LIST);
        }

        //Deserialize all users from the disk
        public static ArrayList deserialize()
        {
            return (ArrayList)SerializationService.deserialize(USERS_LIST);
        }
    }
}
