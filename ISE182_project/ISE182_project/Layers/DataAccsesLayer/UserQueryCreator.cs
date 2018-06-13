using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.DataAccsesLayer
{
    //Create user related Queries
    class UserQueryCreator : QueryCreator<IUser>
    {

        #region members

        //table coloms names
        private const string TABLE_NAME = "Users"; // the name of the table
        private const string ID_COL = "id";
        private const string GROUP_COL = "Group_Id";
        private const string NICK_COL = "Nickname";
        private const string PASSWORD_COL = "Password";

        //Parametes names
        private const string ID_PARM = "@id";
        private const string GROUP_PARM = "@GroupID";
        private const string NICK_PARM = "@NICk";
        private const string PASSWORD_PARM = "@pass";

        //parametes sizes
        private const int NICK_SIZE = 8;
        private const int PASWORD_SIZE = 64;

        private bool _logister; // This to check if the the query is to check if the user can register of login

        #endregion

        #region Constructors

        //A constructor
        public UserQueryCreator() : base()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
        }

        #endregion

        #region Abstract Imlamentation

        //reset filters
        public override void clearFilters()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            base.clearFilters();
            clear();
        }

        //get te querry command string
        protected override string getQueryString()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            string quary = "";

            if(_logister)
                return canLogisterQuery();

            //Create Quary
            switch (Type)
            {
                case SELECT: quary = $"{SELECT}  * {FROM} {TABLE_NAME}"; break;
                case INSERT: quary = $"{INSERT} {TABLE_NAME} ({GROUP_COL},{NICK_COL},{PASSWORD_COL}) {Values()}"; break;
                case UPDATE:
                    {
                        string error = "you are not allowed to change users!";
                        Logger.Log.Error(Logger.Maintenance(error));

                        throw new InvalidOperationException(error);
                    }
            }

            return quary;
        }

        #endregion

        #region functionalities
        
        //set this query to check regiserable \ loginable
        public  void SetToLogisterQuery()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            SETtoSELECT();
            _logister = true;
        }

        #endregion

        #region Private Methods

        // get the query to check regiserable \ loginable
        private string canLogisterQuery()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return $"{SELECT} {ID_COL},{PASSWORD_COL} {FROM} {TABLE_NAME} {Where()}";
        }

        //Get the VALUE part of the query
        private string Values()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            string query =  $"{VALUES} ({ GROUP_PARM},{NICK_PARM},{PASSWORD_PARM})";
            SqlParameter temp;

            temp = new SqlParameter(GROUP_PARM, SqlDbType.Int);
            temp.Value = QuaryItem.Group_ID;
            parameters.Add(temp);

            temp = new SqlParameter(NICK_PARM, SqlDbType.Char, NICK_SIZE);
            temp.Value = QuaryItem.NickName;
            parameters.Add(temp);

            temp = new SqlParameter(PASSWORD_PARM, SqlDbType.Char, PASWORD_SIZE);
            temp.Value = QuaryItem.Password;
            parameters.Add(temp);

            return query;
        }

        //Get the WHERE part of the query
        private string Where()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            string query = $"{WHERE} {GROUP_COL} = {GROUP_PARM} {AND} {NICK_COL} = {NICK_PARM}";
            SqlParameter temp;

            temp = new SqlParameter(GROUP_PARM, SqlDbType.Int);
            temp.Value = QuaryItem.Group_ID;
            parameters.Add(temp);

            temp = new SqlParameter(NICK_PARM, SqlDbType.Char, NICK_SIZE);
            temp.Value = QuaryItem.NickName;
            parameters.Add(temp);

            return query;
        }

        //clear inputs
        public void clear()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            _logister = false;
        }

        #endregion

    }
}
