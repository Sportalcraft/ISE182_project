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
        private const string TABLE_NAME = "Useres"; // the name of the table
        private const string Group_COL = "GroupID";
        private const string NICK_COL = "nickName";

        //Parametes names
        private const string Group_PARM = "@GRoupID";
        private const string NICK_PARM = "@NICk";

        #endregion

        #region Constructors

        //A constructor
        public UserQueryCreator() : base()
        {

        }

        #endregion

        #region Abstract Imlamentation

        protected override string getQueryString()
        {
            string quary = "";

            //Create Quary
            switch (Type)
            {
                case SELECT: quary = $"{SELECT}  * {FROM} {TABLE_NAME}"; break;
                case INSERT: quary = $"{INSERT} {TABLE_NAME} ({Group_COL},{NICK_COL}) {Values()}"; break;
                case UPDATE:
                    {
                        string error = "you are not aloowd to change users!";
                        Logger.Log.Error(Logger.Maintenance(error));

                        throw new InvalidOperationException(error);
                    }
            }

            return quary;
        }

        #endregion

        #region Private Methods

        private string Values()
        {
           string query =  $"{ VALUES} ({ Group_PARM},{NICK_PARM})";

           parameters.Add(new SqlParameter(Group_PARM, QuaryItem.Group_ID));
           parameters.Add(new SqlParameter(NICK_PARM, QuaryItem.NickName));

            return query;
        }

        #endregion

    }
}
