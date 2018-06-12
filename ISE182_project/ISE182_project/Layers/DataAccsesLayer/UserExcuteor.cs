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
    //This class excute user related queries
    class UserExcuteor
    {

        // execute the queary and return the users the were drawn frm server
        public ICollection<IUser> Excute(SqlCommand query)
        {
            int id;
            int Grpuo_id;
            string NickName;          
            string password;

            ICollection<IUser> output = new List<IUser>();

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

                    output.Add(user);
                }
            }
            catch
            {

            }
            finally
            {
                reader.Close();
            }

            return output;      
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
        
        //return if a user can register
        public bool canRegister(IUser user)
        {
            UserQueryCreator qc = new UserQueryCreator();
            Connect conn = new Connect();

            qc.addQuaryItem(user);
            qc.SetToLogisterQuery();
            SqlDataReader reader = conn.ExecuteReader(qc.getQuary());           

            return reader.Read(); //No current user with this name and group
        } 
    }
}
