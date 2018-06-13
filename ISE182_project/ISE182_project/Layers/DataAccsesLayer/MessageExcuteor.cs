using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
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
    //This class excute message related queries
    class MessageExcuteor
    {
        // execute the queary and return the messaged tht were drown from the server
        public ICollection<IMessage> Excute(SqlCommand query)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));


            Guid guid;
            int UserID;           
            DateTime receivingTime; 
            int groupID;
            string NickName;        
            string body;

            ICollection<IMessage> output = new List<IMessage>();

            Connect conn = new Connect();
            SqlDataReader reader = conn.ExecuteReader(query);

            while(reader.Read())
            {
                try
                {
                    guid = new Guid(reader.GetString(0).Trim());
                    UserID = reader.GetInt32(1);
                    receivingTime = reader.GetDateTime(2);
                    body = reader.GetString(3);
                    groupID = reader.GetInt32(4);
                    NickName = reader.GetString(5);

                    DisplayMessage msg = new DisplayMessage(guid, receivingTime, groupID, NickName, body);
                    output.Add(msg);
                }
                catch { }
            }

            return output;
        }

    }
}
