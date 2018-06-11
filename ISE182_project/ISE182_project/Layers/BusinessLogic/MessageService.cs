using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.DataAccsesLayer;
using ISE182_project.Layers.LoggingLayer;
//using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    //This class manege the messages stored in RAM
     class MessageService
    {
        private const int MAX_MESSAGES = 200; // maximum items per quary

        private MessageQueryCreator query; // the query generator
        private DateTime _lastMessageTime; // the time of the last message

        private ICollection<IMessage> _ramData; // Store a coppy of the data in the ram for quick acces       
        private ICollection<IMessage> _lastFilteredList; //Save te filtered items

        private ChatRoom.Sort _sortBy; //Save the last sort option
        private bool _descending; //Save the last sort direction

        #region singletone

        //private ctor
        private MessageService()
        {
            query = new MessageQueryCreator(MAX_MESSAGES);
            _ramData = new List<IMessage>();
            _lastFilteredList = new List<IMessage>();
        }

        private static MessageService _instence; // the instence

        // instemce getter
        public static MessageService Instence
        {
            get
            {
                if (_instence == null)
                    _instence = new MessageService();

                return _instence;
            }
        }

        #endregion

        #region functionalities

        //Getter and setter to the data stored in the ram
        public ICollection<IMessage> RamData
        {
            get { return _ramData; }
            private set
            {
                if(value != null)
                    _ramData = value;
            }
        }

        private void add(IMessage item)
        {

             RamData.Add(item);
            _lastFilteredList.Add(item);

            if (RamData.Count > MAX_MESSAGES)
            {
                RamData.Remove(RamData.First());
            }

            if (_lastFilteredList.Count > MAX_MESSAGES)
            {
                _lastFilteredList.Remove(_lastFilteredList.First());
            }
        }

        public void send(IMessage item, int UserID)
        {
            add(item);
            sort(_sortBy, _descending); // resort
            AddToDS(item, UserID);
        }

        //Get the filtered messages
        public ICollection<IMessage> getMessages()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return _lastFilteredList;
        }

        #region Filter

        //recive all the messages from a certain user
        public ICollection<IMessage> FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(user.Group_ID);
            query.addNicknameFilter(user.NickName);
            Execute();

            return RamData;
        }

        //recive all the messages from a certain group
        public ICollection<IMessage> FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(groupID);
            Execute();

            return RamData;
        }

        #endregion

        #region Sort

        //Sort a message List by the time
        public void sort(ChatRoom.Sort SortBy, bool descending)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ICollection<IMessage> temp = null;

            //Save last sort option
            _sortBy = SortBy;
            _descending = descending;

            if (descending)
            {
                switch (SortBy)
                {
                    case ChatRoom.Sort.Time: temp = _lastFilteredList.OrderByDescending(msg => msg.Date).ToList(); break;
                    case ChatRoom.Sort.Nickname: temp = _lastFilteredList.OrderByDescending(msg => msg.UserName).ToList(); break;
                    case ChatRoom.Sort.GroupNickTime: temp = _lastFilteredList.OrderByDescending(msg => msg.GroupID).ThenByDescending(msg => msg.UserName).ThenByDescending(msg => msg.Date).ToList(); break;
                }
            }

            switch (SortBy)
            {
                case ChatRoom.Sort.Time: temp = _lastFilteredList.OrderBy(msg => msg.Date).ToList(); break;
                case ChatRoom.Sort.Nickname: temp = _lastFilteredList.OrderBy(msg => msg.UserName).ToList(); break;
                case ChatRoom.Sort.GroupNickTime: temp = _lastFilteredList.OrderBy(msg => int.Parse(msg.GroupID)).ThenBy(msg => msg.UserName).ThenBy(msg => msg.Date).ToList(); break;
            }

            _lastFilteredList = temp;
        }

        

        #endregion


        //return the last n saved messages
        public ICollection<IMessage> lastNmesages(int amount)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (amount < 0)
            {
                string error = "User requested a negative number of mesaages";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentOutOfRangeException(error);
            }

            if (amount > RamData.Count)
            {
                Logger.Log.Warn(Logger.Maintenance("User requested more messages then there is to show"));
                amount = RamData.Count;
            }

            return RamData.Reverse().Take(amount).Reverse().ToList();
        }

        //retrive and save the new messages that were send after the last draw.
        public void DrawNewMessage()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));         

            query.clearFilters();
            query.SETtoSELECT();
            query.addTimeFilter(_lastMessageTime);

            try
            {
                Execute();

                if(RamData.Count > 0)
                 _lastMessageTime = RamData.Last().Date;
            }
            catch
            {
                string error = "An Error Acured while drawing messages!";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw;
            } 
        }

        #endregion

        //-----------------------------------------------------------------

        #region override methods

        // deserialize messages
        protected void reciveData()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            query.clearFilters();
            query.SETtoSELECT();
            Execute();
        }

        // serialize messages
        protected bool AddToDS(IMessage Data, int UserID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            query.SETtoINSERT(Data);
            query.addUserID(UserID);

            try
            {
                Connect con = new Connect();
                con.ExecuteNonQuery(query.getQuary());
                _lastMessageTime = Data.Date;
                return true;
            }
            catch(Exception e)
            {
                string error = "Failed do add data to the DS";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);;
            }
            
        }

        //init data
        public void start()
        {
            reciveData();

            if (RamData.Count > 0)
                _lastMessageTime = RamData.Last().Date;
            else
                _lastMessageTime = new DateTime(1, 1, 1);

            _lastFilteredList = RamData;
             sort(ChatRoom.Sort.Time, false); //Sortg by Time at start
            RamData = _lastFilteredList;

        }

        #endregion

        #region PrivateMethods

        //Edid message by guid and save to the RAM and disk
        private void EditMessage(Guid ID, string newBody)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            //A dummy message to use in the .equals
            IMessage dummy = new Message(ID, DateTime.Now,"Dummy",0, "Dummy");

            foreach (Message msg in RamData)
            {
                if (msg.Equals(dummy))
                {
                    msg.editBody(newBody);
                    query.clearFilters();
                    query.SETtoUPDATE(msg);
                    Connect con = new Connect();
                    con.ExecuteNonQuery(query.getQuary());
                    return;
                }
            }

            string error = "Could not found a message with the ruqusted guid of " + ID;
            Logger.Log.Error(Logger.Maintenance(error));

            throw new KeyNotFoundException(error);
        }

        //Clear old filterrs and add filter by the group and by the time
        private void prepareGroupFilter(int group)
        {
            query.clearFilters();
            query.addGroupFilter(group);
            query.addTimeFilter(_lastMessageTime);
        }

        private void Execute()
        {
            MessageExcuteor me = new MessageExcuteor();
            ICollection<IMessage> temp = me.Excute(query.getQuary());

            foreach (IMessage msg in temp)
                add(msg);
        }

        #endregion

    }
}
