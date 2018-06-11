using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.DataAccsesLayer
{
    class MessageQueryCreator : QueryCreator<IMessage>
    {
        #region members

        private const int EMPTY_GROUP = -1; // the number were there is no group
        private /*const*/ DateTime EMPTY_TIME = new DateTime(1,1,1); // the time were there is no time filter
        private readonly int MAX_MESSAGES; // maximum items per quary

        //table coloms names
        private const string TABLE_NAME = "Messages"; // the name of the table
        private const string GUID_COL = "Guid";
        private const string User_ID_COL = "User_Id";
        private const string DATE_COL = "SendTime";
        private const string BODY_COL = "Body";

        //Names of the parameters
        private const string GUID_PARM = "@GUI";
        private const string USER_ID_PARM = "@UserID";
        private const string DATE_PARM = "@Data";
        private const string BODY_PARM = "@Body";

        //sizes of the parameters
        private const int GUID_SIZE = 68;
        //private const int USER_ID_SIZE = 32;
        //private const int DATE_SIZE = 30;
       private const int BODY_SIZE = 100;

        //From Users Table
        private const string USERS_TABLE = "Users"; // the name of the table
        private const string USERS_ID_COL = "Id";
        private const string GROUP_COL = "Group_Id";
        private const string NICK_COL = "Nickname";

        private const int NICK_SIZE = 8;

        //Users parameters
        private const string GROUP_PARM = "@GroupID";
        private const string NICK_PARM = "@NICk";

        private int _group; // the group to filter with
        private string _nickName; // the nick name to filter with  
        private DateTime _lastRecivedMessage; // Save the time of the last recived message    
        private int _userID; //The id of the user

        #endregion

        #region Constructors

        //A constructor
        public MessageQueryCreator(int MaxMessages) : base()
        {            
            clear();
            MAX_MESSAGES = MaxMessages;
        }

        #endregion

        #region Filtering

        // clear filter options
        public override void clearFilters()
        {
            base.clearFilters();
            clear();
        }

        // add a group filter
        public void addGroupFilter(int group)
        {
            if (group <= 0)
            {
                string error = "this group is illeagal!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            _group = group;
        }

        public void addUserID(int id)
        {
            if (id <= 0)
            {
                string error = "this id is illeagal!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            _userID = id;
        }

        // add a nick name filter
        public void addNicknameFilter(string nickName)
        {
            if (String.IsNullOrEmpty(nickName))
            {
                string error = "this name is illeagal!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            if (_group == EMPTY_GROUP)
            {
                string error = "in order to use the nick name filtering, you must first enter a group!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            _nickName = nickName;
        }

        // add a time filter
        public void addTimeFilter(DateTime from)
        {
            if (from > DateTime.Now)
            {
                string error = "this requested time is in the future!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            _lastRecivedMessage = from;
        }

        #endregion

        #region Abstract Imlamentation

        protected override string getQueryString()
        {
            string quary = "";

            if(Type.Equals(INSERT))
            {
                if(_userID == -1)
                {
                    string error = "you must enter the id of the user in order to send messages!";
                    Logger.Log.Error(Logger.Developer(error));

                    throw new InvalidOperationException(error);
                }
            }

            //Create query
            switch (Type)
            {
                case SELECT: quary = $"{SELECT} {TOP} {MAX_MESSAGES} {TABLE_NAME}.*, {USERS_TABLE}.{GROUP_COL}, {USERS_TABLE}.{NICK_COL} {FROM} {JOIN()} {where()} {ORDER_BY} {DATE_COL}"; break;
                case INSERT: quary = $"{INSERT} {TABLE_NAME} ({GUID_COL},{User_ID_COL},{DATE_COL},{BODY_COL}) {values()}"; break;
                case UPDATE: quary = $"{UPDATE} {TABLE_NAME} {set()}"; break;
            }

            return quary;
        }      

        #endregion

        #region Private Methods

        //reset slections
        private void clear()
        {
            _group = EMPTY_GROUP;
            _nickName = null;
            _lastRecivedMessage = EMPTY_TIME;
            _userID = -1;
        }

        //get a where to the filtering options
        private string where()
        {
            string where = whereNoTime();

            if(_group == EMPTY_GROUP & _nickName == null) // there is no filter to do
            {
                where += BulidTimeFilter(where); //No filters to make, draw only last messages
            }         

            return where;
        }

        //return the where statement without time filter
        private string whereNoTime()
        {
            string where = "";

            SqlParameter tParameter;

            if (_group != EMPTY_GROUP | _nickName != null) // tere is a where
                where += WHERE + " ";

            if (_group != EMPTY_GROUP)
            {
                where += $"{GROUP_COL} = {GROUP_PARM}";
                tParameter = new SqlParameter(GROUP_PARM, SqlDbType.Int);
                tParameter.Value = _group;
                parameters.Add(tParameter);

                if (_nickName != null)
                    where += $" {AND} ";
            }

            if (_nickName != null)
            {
                where += $"{NICK_COL} = {NICK_PARM}";
                tParameter = new SqlParameter(NICK_PARM, SqlDbType.Char, NICK_SIZE);
                tParameter.Value = _nickName;
                parameters.Add(tParameter);
            }

            return where;
        }

        private string BulidTimeFilter(string CurrentWhere)
        {
            string timeCondition = $"{DATE_COL} > {DATE_PARM}";
            string where = "";
            SqlParameter tParameter;

            if (!_lastRecivedMessage.Equals(EMPTY_TIME))
            {
                if (CurrentWhere.Equals(""))
                    where += $"{WHERE} {timeCondition}";
                else
                    where += $" {AND} {timeCondition}";

                tParameter = new SqlParameter(DATE_PARM, SqlDbType.DateTime);
                tParameter.Value = _lastRecivedMessage;
                parameters.Add(tParameter);
            }

            return where;
        }

        private string JOIN()
        {
            return $"{TABLE_NAME} {INNER_JOIN} {USERS_TABLE} {ON} {TABLE_NAME}.{User_ID_COL} = {USERS_TABLE}.{USERS_ID_COL}";
        }

        // get the VALUES statements for INSERT quary
        private string values()
        {
           string values =  VALUES +$" ({GUID_PARM},{USER_ID_PARM},{DATE_PARM},{BODY_PARM})";
            SqlParameter tParameter;

            tParameter = new SqlParameter(GUID_PARM, SqlDbType.UniqueIdentifier, GUID_SIZE);
            tParameter.Value = QuaryItem.Id;
            parameters.Add(tParameter);

            tParameter = new SqlParameter(USER_ID_PARM, SqlDbType.Int);
            tParameter.Value = _userID;
            parameters.Add(tParameter);

            tParameter = new SqlParameter(DATE_PARM, SqlDbType.DateTime);
            tParameter.Value = QuaryItem.Date;
            parameters.Add(tParameter);

            tParameter = new SqlParameter(BODY_PARM, SqlDbType.Text, BODY_SIZE);
            tParameter.Value = QuaryItem.MessageContent;
            parameters.Add(tParameter);

            return values;
        }

        //get the SET statement for UPDATE queries
        private string set()
        {
            string where = $"{SET} {BODY_COL} = {BODY_PARM} {WHERE} {GUID_COL} = { GUID_PARM}";
            SqlParameter tParameter;

            tParameter = new SqlParameter(BODY_PARM, SqlDbType.Text, BODY_SIZE);
            tParameter.Value = QuaryItem.MessageContent;
            parameters.Add(tParameter);

            tParameter = new SqlParameter(GUID_PARM, SqlDbType.UniqueIdentifier, GUID_SIZE);
            tParameter.Value = QuaryItem.Id;
            parameters.Add(tParameter);

            return where;
        }

        #endregion
    }
}
