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
        #region Members

        private const int MAX_MESSAGES = 200; // maximum items per quary

        private MessageQueryCreator query; // the query generator
        private DateTime _lastMessageTime; // the time of the last message

        private ICollection<IMessage> _ramData; // Store a coppy of the data in the ram for quick acces       
        private ICollection<IMessage> _lastFilteredList; //Save te filtered items     

        private ChatRoom.Sort _sortBy; //Save the last sort option
        private bool _descending; //Save the last sort direction

        private string _lastFilter; //Save the last filter option
        private string _lastFilterNick; //Save the last filter nickname
        private int _lastFilterGroup; //Save the last filter gtoup

        #endregion

        #region singletone

        //private ctor
        private MessageService()
        {
            query = new MessageQueryCreator(MAX_MESSAGES);
            _ramData = new List<IMessage>();
            _lastFilteredList = new List<IMessage>();
            _lastFilter = ""; 
            _lastFilterNick = ""; 
            _lastFilterGroup = -1; 

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

        //Get the filtered messages
        public ICollection<IMessage> getMessages()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            LastFilter(); //Filter again by the last filter
            LastSort(); //Sort before sending
            return _lastFilteredList;
        }

        //Getter and setter to the data stored in the ram
        private ICollection<IMessage> RamData
        {
            get { return _ramData; }
            set
            {
                if(value != null)
                    _ramData = value;
            }
        }

        //add a message to the ram
        private void add(IMessage item)
        {
            if(!RamData.Contains(item))
                RamData.Add(item);
            if (!_lastFilteredList.Contains(item))
                _lastFilteredList.Add(item);

            if (_lastFilteredList.Count > MAX_MESSAGES)
            {
                if(_descending)
                    _lastFilteredList.Remove(_lastFilteredList.Last());
                else
                    _lastFilteredList.Remove(_lastFilteredList.First());
            }
        }

        //Send a message and save into the ram
        public void send(IMessage item, int UserID)
        {
            add(item);
            AddToDS(item, UserID);
        }

        #region Filter

        //recive all the messages from a certain user
        public void FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(user.Group_ID);
            query.addNicknameFilter(user.NickName);

            _lastFilteredList = getFilterdMessages();

            _lastFilter = "ByUser";
            _lastFilterGroup = user.Group_ID;
            _lastFilterNick = user.NickName;
        }

        //recive all the messages from a certain group
        public void FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            prepareGroupFilter(groupID);
            _lastFilteredList = getFilterdMessages();

            _lastFilter = "ByGroup";
            _lastFilterGroup = groupID;
            _lastFilterNick = "";
        }

        //reset filters
        public void resetFilters()
        {
            _lastFilteredList = RamData;
            LastSort();
            _lastFilteredList = _lastFilteredList.Reverse().Take(MAX_MESSAGES).Reverse().ToList();

            _lastFilter = "None";
            _lastFilterGroup = -1;
            _lastFilterNick = "";
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
            else
            {
                switch (SortBy)
                {
                    case ChatRoom.Sort.Time: temp = _lastFilteredList.OrderBy(msg => msg.Date).ToList(); break;
                    case ChatRoom.Sort.Nickname: temp = _lastFilteredList.OrderBy(msg => msg.UserName).ToList(); break;
                    case ChatRoom.Sort.GroupNickTime: temp = _lastFilteredList.OrderBy(msg => int.Parse(msg.GroupID)).ThenBy(msg => msg.UserName).ThenBy(msg => msg.Date).ToList(); break;
                }
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

                if (_lastFilteredList.Count > 0)
                {
                    if(_descending)
                        _lastMessageTime = getMessages().First().Date;
                    else
                        _lastMessageTime = getMessages().Last().Date;
                }
            }
            catch
            {
                string error = "An Error Acured while drawing messages!";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw;
            } 
        }

        //Edid message by guid and save to the RAM and disk, and chrck if the user can edit the message
        public void EditMessage(Guid ID, string newBody, IUser editor)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if(editor == null)
            {
                string errorMessage = "recived null user!";
                Logger.Log.Error(Logger.Maintenance(errorMessage));

                throw new ArgumentNullException(errorMessage);
            }


            //A dummy message to use in the .equals
            IMessage dummy = new Message(ID, DateTime.Now, "Dummy", 0, "Dummy");

            foreach (Message msg in _lastFilteredList)
            {
                if (msg.Equals(dummy))
                {
                    if(!msg.GroupID.Equals(editor.Group_ID.ToString()) | !msg.UserName.Equals(editor.NickName)) //written by other user
                    {
                        string errorMessage = "you can edit only your own messges!!";
                        Logger.Log.Error(Logger.Maintenance(errorMessage));

                        throw new InvalidOperationException(errorMessage);
                    }

                    try
                    {
                        msg.editBody(newBody);
                    }
                    catch
                    {
                        string errormessage = "could not edit your message, please check the length of your message!";
                        Logger.Log.Error(Logger.Maintenance(errormessage));

                        throw new ArgumentOutOfRangeException(errormessage);
                    }

                    query.clearFilters();
                    query.SETtoUPDATE(msg);
                    Connect con = new Connect();
                    con.ExecuteNonQuery(query.getQuary());
                    return;
                }
            }

            //message is not exists

            string error = "Could not found a message with the ruqusted guid of " + ID;
            Logger.Log.Error(Logger.Maintenance(error));

            throw new KeyNotFoundException(error);
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

            _lastFilteredList = RamData;
            sort(ChatRoom.Sort.Time, false); //Sortg by Time at start

            if (_lastFilteredList.Count > 0)
                _lastMessageTime = _lastFilteredList.Last().Date;
            else
                _lastMessageTime = new DateTime(1, 1, 1);
        }

        #endregion

        #region Private Methods

        //Clear old filterrs and add filter by the group and by the time
        private void prepareGroupFilter(int group)
        {
            query.clearFilters();
            query.SETtoSELECT();
            query.addGroupFilter(group);
            query.addTimeFilter(_lastMessageTime);
        }

        //Execute queries
        private void Execute()
        {
            MessageExcuteor me = new MessageExcuteor();
            ICollection<IMessage> temp = me.Excute(query.getQuary());

            foreach (IMessage msg in temp)
            {
                if(msg.Date <= DateTime.Now.ToUniversalTime())
                     add(msg);
            }
        }

        //get mthe messages return by a filter
        private ICollection<IMessage> getFilterdMessages()
        {
            MessageExcuteor me = new MessageExcuteor();
            return me.Excute(query.getQuary());
        }

        //sort the messages by the last requested sort
        private void LastSort()
        {
            sort(_sortBy, _descending); // resort
        }

        //filter the messages by the last requested filter
        private void LastFilter()
        {
            switch(_lastFilter)
            {
                case "ByUser": FilterByUser(new User(_lastFilterNick, _lastFilterGroup)); break;
                case "ByGroup": FilterByGroup(_lastFilterGroup); break;
                default: break;
            }
        }

        #endregion

    }
}
