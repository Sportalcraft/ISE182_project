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
    class UserExcuteor
    {

        // execute the queary and add it's items into te given collection
        public void ExcuteAndAddTo(SqlCommand query, ICollection<IUser> toAdd)
        {
            int id;
            int Grpuo_id;
            string NickName;          
            string password;

            if (toAdd == null)
            {
                string error = "collection is null";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            Connect conn = new Connect();
            SqlDataReader reader = conn.ExecuteReader(query);

            try
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(1);
                    Grpuo_id = reader.GetInt32(2);
                    NickName = reader.GetString(3);
                    password = reader.GetString(4);

                    IUser user = new User(/*id,*/ NickName, Grpuo_id/*, password */);

                    if (!toAdd.Contains(user))
                        toAdd.Add(user);
                }
            }
            catch
            {

            }
            finally
            {
                reader.Close();
            }          
        }

        //check if a user can log in, and if si return it's id, return -1 if cann't log in
        public int Loginable(IUser user)
        {
            UserQueryCreator qc = new UserQueryCreator();
            Connect conn = new Connect();
            string DSpassword;
            int id;

            qc.addQuaryItem(user);
            qc.SetToLogisterQuery();
            SqlDataReader reader = conn.ExecuteReader(qc.getQuary());

            if (!reader.Read())
                return -1; // Not registered

            id = reader.GetInt32(0);
            DSpassword = reader.GetString(1).Trim();

            if (DSpassword.Equals(user.Password))
                return id;

            return -1;
        }

        public bool canRegister(IUser user)
        {
            UserQueryCreator qc = new UserQueryCreator();
            Connect conn = new Connect();

            qc.addQuaryItem(user);
            qc.SetToLogisterQuery();
            SqlDataReader reader = conn.ExecuteReader(qc.getQuary());           

            return reader.Read(); //No current usser with this name and group
        } 
    }
}
