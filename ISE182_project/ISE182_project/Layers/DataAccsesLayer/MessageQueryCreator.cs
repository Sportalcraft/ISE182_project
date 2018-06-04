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
        private /*const*/ DateTime EMPTY_TIME = new DateTime(0,1,1); // the time were there is no time filter
        private const int MAX_MESSAGES = 200; // maximum items per quary

        //table coloms names
        private const string TABLE_NAME = "Messages"; // the name of the table
        private const string GUI_COL = "GUI";
        private const string GROUP_COL = "GroupID";
        private const string DATE_COL = "Data";
        private const string NICK_COL = "nickName";
        private const string BODY_COL = "Body";

        //Names of the parameters
        private const string GUI_PARM = "@GUI";
        private const string GROUP_PARM = "@GroupID";
        private const string DATE_PARM = "@Data";
        private const string NICK_PARM = "@nickName";
        private const string BODY_PARM = "@Body";

        private int _group; // the group to filter with
        private string _nickName; // the nick name to filter with  
        private DateTime _lastRecivedMessage; // Save the time of the last recived message    

        #endregion

        #region Constructors

        //A constructor
        public MessageQueryCreator() : base()
        {            
            clear();
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

            //Create query
            switch (Type)
            {
                case SELECT: quary = $"{SELECT} {TOP} {MAX_MESSAGES} * {FROM} {TABLE_NAME} {where()}"; break;
                case INSERT: quary = $"{INSERT} {TABLE_NAME} ({GUI_COL},{DATE_COL},{GROUP_COL},{NICK_COL},{BODY_COL}) {values()}"; break;
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
        }

        //get a where to the filtering options
        private string where()
        {
            string where = "";
            string timeCondition;

            if (_group != EMPTY_GROUP | _nickName != null) // tere is a where
                where += WHERE + " ";

            if (_group != EMPTY_GROUP)
            {
                where += GROUP_COL + " = " + GROUP_PARM;
                parameters.Add(new SqlParameter(GROUP_PARM, _group));

                if (_nickName != null)
                    where += " " +AND + " ";
            }

            if (_nickName != null)
            {
                where += NICK_COL + " = " + NICK_PARM;
                parameters.Add(new SqlParameter(NICK_PARM, _nickName));
            }

            timeCondition = $"{DATE_COL} = {_lastRecivedMessage}";

            if(!_lastRecivedMessage.Equals(EMPTY_TIME))
            {
                if (where.Equals(""))
                    where += $"{WHERE} {DATE_COL} = {timeCondition}";
                else
                    where += $"{AND} {timeCondition}";
            }

            return where;
        }

        // get the VALUES statements for INSERT quary
        private string values()
        {
           string values =  VALUES +$" ({GROUP_PARM},{DATE_PARM},{GROUP_PARM},{NICK_PARM},{BODY_PARM})";

            parameters.Add(new SqlParameter(GROUP_PARM, QuaryItem.GroupID));
            parameters.Add(new SqlParameter(DATE_PARM, QuaryItem.Date));
            parameters.Add(new SqlParameter(GROUP_PARM, QuaryItem.GroupID));
            parameters.Add(new SqlParameter(NICK_PARM, QuaryItem.UserName));
            parameters.Add(new SqlParameter(BODY_PARM, QuaryItem.MessageContent));

            return values;
        }

        //get the SET statement for UPDATE queries
        private string set()
        {
            string where = $"{SET} {BODY_COL} = {BODY_PARM} {WHERE} {GUI_COL} = { GUI_PARM}";

            parameters.Add(new SqlParameter(BODY_PARM, QuaryItem.MessageContent));
            parameters.Add(new SqlParameter(GUI_PARM, QuaryItem.Id));

            return where;
        }

        #endregion
    }
}
