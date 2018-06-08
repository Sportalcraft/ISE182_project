using ISE182_project.Layers.DataAccsesLayer;
using ISE182_project.Layers.LoggingLayer;
//using ISE182_project.Layers.PersistentLayer;
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
        private UserQueryCreator query; // the query generator

        #region singletone

        //private ctor
        private UserService()
        {
            query = new UserQueryCreator();
        }

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

        #region functionalities

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

            if (!canRegister(user)) //cheak if this user is taken
            {
                string error = "client tried to register with an already existing user";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            RamData.Add(user);
            query.clearFilters();
            query.SETtoINSERT(user);
            Execute();
        }

        #endregion

        #region Validation

        //cheak if a user can register
        public bool canRegister(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            UserExcuteor ue = new UserExcuteor();

            if (user == null)
            {
                string error = "recived a null user for registration";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return !ue.canRegister(user);
        }

        //cheak if a user can login
        public bool canLogIn(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            UserExcuteor ue = new UserExcuteor();

            if (user == null)
            {
                string error = "recived a null user for login";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            return ue.canLogIn(user);
        }

        #endregion

        //-----------------------------------------------------------

        #region overrding methods

        protected override void reciveData()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            query.clearFilters();
            query.SETtoSELECT();
            Execute();
        }

        protected override bool AddToDS(IUser item)
        {
            try
            {
                query.clearFilters();
                query.SETtoINSERT(item);
                Execute();
                return true;
            }
            catch
            {
                string error = "Failed to add data to DS";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new Exception(error);
            }

        }


        protected override ICollection<IUser> DefaultSort(ICollection<IUser> Data)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return Data; // No defult sorting mechanisem, For now....
        }

        #endregion

        #region Private Methods

        //Execute the query
        private void Execute()
        {
            UserExcuteor excuteor = new UserExcuteor();
            excuteor.ExcuteAndAddTo(query.getQuary(), RamData);
            query.clearFilters();
        }

        #endregion
    }
}
