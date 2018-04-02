using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{

    //This class manege the useres stored in RAM
    class UserService
    {
        private static ArrayList _ramUsers; // Store a coppy of the users in the ram for quick acces

        //Getter and setter to the users stored in the ram
        public static ArrayList RamUsers
        {
            private set
            {
                if (_ramUsers == null) //there is ni stored messages
                {
                    _ramUsers = value;
                }

                MergeTwoArrays.mergeIntoFirst(_ramUsers, value); // Merging to avoid duplication
                UserSerializationService.serialize(_ramUsers);   // Serialize the new list
            }

            get
            {
                if (_ramUsers == null)
                    _ramUsers = UserSerializationService.deserialize(); // Deserialize users to the ram if not there alrady

                return _ramUsers;
            }
        }

        //Add a new user to the users list
        public static void register(IUser user)
        {
            if (!canRegister(user)) //cheak if this nicknake is taken
                throw new ArgumentException("This user already taken!");

            RamUsers.Add(user);
            Update();
        }

        //cheak if a user can register
        public static bool canRegister(IUser user)
        {
            return !RamUsers.Contains(user);
        }

        //cheak if a user can login
        public static bool canLogIn(IUser user)
        {
            return RamUsers.Contains(user);
        }


        //-----------------------------------------------------------

        //Update the stored in disk users after changing ram
        private static void Update()
        {
            RamUsers = RamUsers; //So the set atribulte will activate to ask to save to disk
        }
    }
}
