using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{

    //This class manege the useres stored in RAM
    class UserService
    {
        private static ArrayList _ramUsers; // Store a coppy of the users in the ram for quick acces

        //Getter and setter to the users stored in the ram
        private static ArrayList RamUsers
        {
            set
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

        //Initiating the ram's saves from users stored in the disk
        public static void start()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            SetRAM();
        }

        //Add a new user to the users list
        public static void register(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null) 
            {
                Logger.Log.Error(Logger.Maintenance("recived a null user for registration"));
                return;
            }

            if (!canRegister(user)) //cheak if this nicknake is taken
            {
                Logger.Log.Error(Logger.Maintenance("client tried to register with an alreadt existing user"));
                return;
            }

            RamUsers.Add(user);
            UpdateDisk();
        }

        //cheak if a user can register
        public static bool canRegister(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null)
            {
                Logger.Log.Error(Logger.Maintenance("recived a null user for registration"));
                return false;
            }

            return !RamUsers.Contains(user);
        }

        //cheak if a user can login
        public static bool canLogIn(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null)
            {
                Logger.Log.Error(Logger.Maintenance("recived a null user for login"));
                return false;
            }

            return RamUsers.Contains(user);
        }


        //-----------------------------------------------------------

        #region private methods

        //Update the stored in disk users after changing ram
        private static void UpdateDisk()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            RamUsers = new ArrayList(); //So the set atribulte will activate to ask to save to disk
        }

        //Setting the ram, if null
        private static void SetRAM()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ICollection temp = RamUsers; //So the get atribulte will activate to ask to draw from disk
        }

        #endregion
    }
}
