using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    class UserService
    {
        private const string USERS_LIST = "Users.bin"; // The file name to save the users
        private static ArrayList _ramUsers;            // Store a coppy of the users in the ram for quick acces

        //Getter and setter to the users stored in the ram
        public static ArrayList RamUsers
        {
            private set { _ramUsers = value; }
            get
            {
                if (_ramUsers == null)
                    deserializeAllUsers();

                return _ramUsers;
            }
        }

        //Add a new user to the users list
        public static void register(IUser user)
        {
            if (!canRegister(user))
                throw new IllegalArgumentException();

            RamUsers.Add(user);
        }

        //cheak if a user can register
        public static bool canRegister(IUser user)
        {
            return !RamUsers.Contains(user);
        }

        #region private methods

        //Deserialize all users from the disk and save to ram
        private static ArrayList deserializeAllUsers()
        {
            ArrayList temp = (ArrayList)SerializationService.Deserialize(USERS_LIST);
            RamUsers = temp;
            return temp;
        }

        // Merge two lists of messages to the first one
        private static void mergeIntoFirst(ArrayList users1, ArrayList users2)
        {
            foreach (IUser user in users2)
            {
                if (!users1.Contains(user))
                    users1.Add(user);
            }
        }

        //Serialze a list of users
        private static void serialize(ArrayList users)
        {
            SerializationService.Serialize(users, USERS_LIST);
        }

        //Serialize a list of users
        private static void serializeUsers(ArrayList users)
        {
            if (File.Exists(USERS_LIST))
            {
                mergeIntoFirst(RamUsers, users);
                users = RamUsers;
            }

            serialize(users);
            RamUsers = users;
        }

        #endregion
    }
}
