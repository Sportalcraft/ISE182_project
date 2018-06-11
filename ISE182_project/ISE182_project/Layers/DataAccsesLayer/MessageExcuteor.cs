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
        // execute the queary
        public ICollection<IMessage> Excute(SqlCommand query)
        {
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
                guid = new Guid(reader.GetString(0).Trim());
                UserID = reader.GetInt32(1);
                receivingTime = reader.GetDateTime(2);
                body = reader.GetString(3);
                groupID = reader.GetInt32(4);
                NickName = reader.GetString(5);

                IMessage msg = new Message(guid, receivingTime,groupID, NickName, body);
                output.Add(msg);
            }

            return output;
        }
    }
}
