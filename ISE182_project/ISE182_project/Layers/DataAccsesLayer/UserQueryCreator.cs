using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.DataAccsesLayer
{
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

        private bool _logister; // This to check if the the query is to check if the user can register of login

        #endregion

        #region Constructors

        //A constructor
        public UserQueryCreator() : base()
        {

        }

        #endregion

        #region Abstract Imlamentation

        public override void clearFilters()
        {
            base.clearFilters();
            clear();
        }

        protected override string getQueryString()
        {
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
        
        public  void SetToLogisterQuery()
        {
            SETtoSELECT();
            _logister = true;
        }

        private string canLogisterQuery()
        {
            return $"{SELECT} {ID_COL},{PASSWORD_COL} {FROM} {TABLE_NAME} {Where()}";
        }      

        #endregion

        #region Private Methods

        private string Values()
        {
            string query =  $"{VALUES} ({ GROUP_PARM},{NICK_PARM},{PASSWORD_PARM})";

           parameters.Add(new SqlParameter(GROUP_PARM, QuaryItem.Group_ID));
           parameters.Add(new SqlParameter(NICK_PARM, QuaryItem.NickName));
           parameters.Add(new SqlParameter(PASSWORD_PARM, QuaryItem.Password));

            return query;
        }

        private string Where()
        {
            string query = $"{WHERE} {GROUP_COL} = {GROUP_PARM} {AND} {NICK_COL} = {NICK_PARM}";

            parameters.Add(new SqlParameter(GROUP_PARM, QuaryItem.Group_ID));
            parameters.Add(new SqlParameter(NICK_PARM, QuaryItem.NickName));

            return query;
        }

        public void clear()
        {
            _logister = false;
        }

        #endregion

    }
}
