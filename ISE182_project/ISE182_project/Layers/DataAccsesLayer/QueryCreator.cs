using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.DataAccsesLayer
{
    // This class creates the queris to use in the data base
    abstract class QueryCreator<T>
    {
        #region Members

        //SQL statements - so the compiler can check mispell errors (like SEkECT)
        protected const string SELECT = "SELECT";
        protected const string INSERT = "INSERT INTO";
        protected const string UPDATE = "UPDATE";
        protected const string FROM = "FROM";
        protected const string TOP = "TOP";
        protected const string WHERE = "WHERE";
        protected const string AND = "AND";
        protected const string SET = "SET";
        protected const string VALUES = "VALUES";
        protected const string INNER_JOIN = "INNER JOIN";
        protected const string ON = "ON";
        protected const string ORDER_BY = "ORDER BY";
        protected const string DESC = "DESC";

        private string _type;                           // SELECT, INSERT or UPDATE
        private T _item;                                // The item to add or edit
        protected ICollection<SqlParameter> parameters; // the parametes of this query

        #endregion

        #region constructors

        // A constructor     
        public QueryCreator()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            parameters = new List<SqlParameter>();
            clear();
        }

        #endregion

        #region Type Setters

        //set this quart tyoe to select
       public void SETtoSELECT()
       {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            AddType(SELECT);
       }

        // set this quary type to insert, and get te item to insert
        public void SETtoINSERT(T item)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            addQuaryItem(item);
            AddType(INSERT);
        }

        // set this quary type to update, and get te item to update 
        public void SETtoUPDATE(T item)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            addQuaryItem(item);
            AddType(UPDATE);
        }

        #endregion

        #region functionalities

        // clear al the filters
        public virtual void clearFilters()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            clear();
        }

        // get the quary
        public SqlCommand getQuary()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            SqlCommand command = null;
            string quary;
            Connect con = new Connect();

            if (Type == null)
            {
                string error = "you must choose a type first!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new InvalidOperationException(error);
            }

            quary = getQueryString();
            command = con.getCommand(quary);

            //Add Parameters
            foreach (SqlParameter parmeter in parameters)
                command.Parameters.Add(parmeter);

            parameters.Clear();

            //command.Prepare(); // preapering fo execution
            return command;
        }

        // add an item to the query
        public void addQuaryItem(T item)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (item == null)
            {
                string error = "this item is illeagal!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            _item = item;
        }

        #endregion

        #region Properties

        // gwr the type of the quary
        protected string Type
        {
            get { return _type; }
        }

        // get the otem of the quary
        protected T QuaryItem
        {
            get { return _item; }
        }

        #endregion

        #region Private Methods

        // reset quary
        private void clear()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            _type = null;
            _item = default(T);
            parameters.Clear();
        }

        // add a type to the quary
        private void AddType(string type)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (type == null || !(type.Equals(SELECT) | type.Equals(INSERT) | type.Equals(UPDATE)))
            {
                string error = "this type is illeagal!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            _type = type;
        }       

        #endregion

        //Get the string of the query
        protected abstract string getQueryString();

    }
}
