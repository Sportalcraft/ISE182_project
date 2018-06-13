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
    //This class help to conect to the server
    class Connect
    {
        private static SqlConnection _conn; // the connection opject

        #region constructors

        //A constructor
        public Connect()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            _conn = new SqlConnection(ConectionString());
        }

        #endregion

        #region functionalities

        //get SqlCommand object
        public SqlCommand getCommand(string quary)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return new SqlCommand(quary, _conn);
        }

        //Execute a query that returns a table
        public SqlDataReader ExecuteReader(SqlCommand command)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            SqlDataReader data_reader = null;

            try
            {
                command.Connection.Open();
                data_reader = command.ExecuteReader();
            }
            catch(Exception e)
            {
                string error = "failed to execute query!";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new Exception(error);
            }
            finally
            {
                //data_reader.Close();
                command.Dispose();
            }

            return data_reader;
        }

        //Execute a query that does not returm a value, and return the amount of rows that were changed
        public int ExecuteNonQuery(SqlCommand command)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            int num_rows_changed = 0;

            try
            {
                command.Connection.Open();
                command.Prepare();
                num_rows_changed = command.ExecuteNonQuery();
            }
            catch(Exception e)
            {

            }
            finally
            {
                command.Dispose();
            }

            return num_rows_changed;
        }

       #endregion
        
        #region Private Methods

        // create the connectio string
        private static string ConectionString()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            string server_address = @"GLADOS\SQLEXPRESS"; //*/ "ise172.ise.bgu.ac.il,1433\\DB_LAB";
            string database_name = "MS3";
            string user_name = "publicUser";
            string password = "isANerd";

            string connetion_string = $"Data Source={server_address};Initial Catalog={database_name};Integrated Security=True";
            //string connetion_string = $"Server={server_address} ;Database={database_name};User ID={user_name};Password={password}";
            return connetion_string;
        }

        #endregion
    }
}
