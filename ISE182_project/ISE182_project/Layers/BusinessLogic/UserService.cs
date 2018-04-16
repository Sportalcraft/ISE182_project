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
        private static ICollection<IUser> _ramUsers; // Store a coppy of the users in the ram for quick acces

        //Getter and setter to the users stored in the ram
        private static ICollection<IUser> RamUsers
        {
            set
            {
                if (_ramUsers == null) //there is ni stored messages
                {
                    string error = "recived a null user for registration";
                    Logger.Log.Error(Logger.Maintenance(error));
                    _ramUsers = new List<IUser>();
                    //throw new ArgumentNullException(error);
                }

                MergeTwoCollections.mergeIntoFirst(_ramUsers, value);      // Merging to avoid duplication

                if (!UserSerializationService.serialize(_ramUsers))   // Serialize the new list
                {
                    string error = "faild to serialize users";
                    Logger.Log.Fatal(Logger.Maintenance(error));

                    throw new IOException(error);
                }
            }

            get
            {
                if (_ramUsers == null)
                    _ramUsers = UserSerializationService.deserialize<IUser>(); // Deserialize users to the ram if not there alrady

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
                string error = "recived a null user for registration";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            if (!canRegister(user)) //cheak if this nicknake is taken
            {
                string error = "client tried to register with an already existing user";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
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
                string error = "recived a null user for registration";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return !RamUsers.Contains(user);
        }

        //cheak if a user can login
        public static bool canLogIn(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null)
            {
                string error = "recived a null user for login";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return RamUsers.Contains(user);
        }


        //-----------------------------------------------------------

        #region private methods

        //Update the stored in disk users after changing ram
        private static void UpdateDisk()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            RamUsers = new List<IUser>(); //So the set atribulte will activate to ask to save to disk
        }

        //Setting the ram, if null
        private static void SetRAM()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ICollection<IUser> temp = RamUsers; //So the get atribulte will activate to ask to draw from disk
        }

        #endregion
    }
}
