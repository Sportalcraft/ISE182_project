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
        private MessageQueryCreator query; // on object to create the queries
        private DateTime _lastMessageTime; //The last recived message

        public MessageExcuteor(int maxMessages)
        {
            query = new MessageQueryCreator(maxMessages);
            _lastMessageTime = new DateTime(1, 1, 1);
        }

        #region Functionalites

        //Add time filter
        public void AddTimeFilter(DateTime time)
        {
            _lastMessageTime = time;
        }

        //Clear the filters
        public void clearFilters()
        {
            query.clearFilters();
        }

        //recive all the messages from a certain user
        public void FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(user.Group_ID);
            query.addNicknameFilter(user.NickName);
        }

        //recive all the messages from a certain group
        public void FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(groupID);
        }


        #endregion

        #region SQL commends

        // add message to the DB
        public int INSERT(IMessage Data, int UserID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            query.SETtoINSERT(Data);
            query.addUserID(UserID);

            try
            {
                Connect con = new Connect();
                int changed = con.ExecuteNonQuery(query.getQuary());
                _lastMessageTime = Data.Date;
                return changed;
            }
            catch (Exception e)
            {
                string error = "Failed do add data to the DS";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error); ;
            }
        }

        //Update a message
        public void UPDATE(IMessage msg)
        {
            query.clearFilters();
            query.SETtoUPDATE(msg);
            Connect con = new Connect();
            con.ExecuteNonQuery(query.getQuary());
        }

        //get all the last messages - for initialization
        public ICollection<IMessage> getALLLastMessages()
        {
            query.clearFilters();
            return getFilteredMessages();
    }

        //get filered messahes
        public ICollection<IMessage> getFilteredMessages()
        {
            query.SETtoSELECT();
            return Excute(query.getQuary());
        }

        #endregion

        #region Private Methods

        // execute the queary and return the messaged tht were drown from the server
        private ICollection<IMessage> Excute(SqlCommand queryCommend)
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
            SqlDataReader reader = conn.ExecuteReader(queryCommend);

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

        //Clear old filterrs and add filter by the group and by the time
        private void prepareGroupFilter(int group)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            query.clearFilters();
            query.SETtoSELECT();
            query.addGroupFilter(group);
            query.addTimeFilter(_lastMessageTime);
        }

        #endregion

    }
}
