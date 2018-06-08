using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.DataAccsesLayer
{
    class MessageExcuteor
    {
        // execute the queary and add it's items into te given collection
        public void ExcuteAndAddTo(SqlCommand query, ICollection<IMessage> toAdd)
        {
            Guid guid;              
            DateTime receivingTime; 
            int groupID;
            string NickName;        
            string body; 

            if (toAdd == null)
            {
                string error = "collection is null";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            Connect conn = new Connect();
            SqlDataReader reader = conn.ExecuteReader(query);

            while(reader.Read())
            {
                guid = reader.GetGuid(1);
                receivingTime = reader.GetDateTime(2);
                groupID = reader.GetInt32(3);
                NickName = reader.GetString(4);
                body = reader.GetString(5);

                IMessage msg = new Message(guid, receivingTime,groupID, NickName, body);

                if (!toAdd.Contains(msg))
                  toAdd.Add(msg);
            }
        }
    }
}
