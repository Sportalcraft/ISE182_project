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
    class UserService : GeneralHandler<IUser>
    {
        #region singletone

        //private ctor
        private UserService() { }

        private static UserService _instence; // the instence

        // instemce getter
        public static UserService Instence
        {
            get
            {
                if (_instence == null)
                    _instence = new UserService();

                return _instence;
            }
        }

        #endregion

        //Add a new user to the users list
        public void register(IUser user)
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

            RamData.Add(user);
            UpdateDisk();
        }

        //cheak if a user can register
        public bool canRegister(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null)
            {
                string error = "recived a null user for registration";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return !RamData.Contains(user);
        }

        //cheak if a user can login
        public bool canLogIn(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (user == null)
            {
                string error = "recived a null user for login";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return RamData.Contains(user);
        }


        //-----------------------------------------------------------

        #region overrding methods

        protected override ICollection<IUser> deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return UserSerializationService.deserialize();
        }

        protected override bool serialize(ICollection<IUser> _ramData)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return UserSerializationService.serialize(RamData);
        }

        protected override ICollection<IUser> DefaultSort(ICollection<IUser> Data)
        {
            return Data; // No defult sorting mechanisem, For now....
        }

        #endregion
    }
}
