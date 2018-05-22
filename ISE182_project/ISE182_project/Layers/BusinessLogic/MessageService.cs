using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PersistentLayer;
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
     class MessageService : GeneralHandler<IMessage>
    {
         #region singletone

        //private ctor
        private MessageService() { }

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

        #region Filter

        //recive all the messages from a certain user
        public ICollection<IMessage> FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return FilterByUser(user, RamData);
        }

        //recive all the messages from a certain group
        public ICollection<IMessage> FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return FilterByGroup(groupID, RamData);
        }

        //recive all the messages from a certain user and from a certain colection of messages
        public ICollection<IMessage> FilterByUser(IUser user, ICollection<IMessage> toFilter)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return toFilter.Where(msg => user.Equals(new User(msg.UserName, int.Parse(msg.GroupID)))).ToList();
        }

        //recive all the messages from a certain group and from a certain colection of messages
        public ICollection<IMessage> FilterByGroup(int groupID, ICollection<IMessage> toFilter)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return toFilter.Where(msg => int.Parse(msg.GroupID) == groupID).ToList();
        }

        #endregion

        #region Sort

        //Sort a message List by the time
        public ICollection<IMessage> sort(ICollection<IMessage> messages, Sort SortBy, bool descending)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (descending)
            {
                switch (SortBy)
                {
                    case Sort.Time: return messages.OrderByDescending(msg => msg.Date).ToList();
                    case Sort.Nickname: return messages.OrderByDescending(msg => msg.UserName).ToList();
                    case Sort.GroupNickTime: return messages.OrderByDescending(msg => msg.GroupID).ThenByDescending(msg => msg.UserName).ThenByDescending(msg => msg.Date).ToList();
                }
            }

            switch (SortBy)
            {
                case Sort.Time: return messages.OrderBy(msg => msg.Date).ToList();
                case Sort.Nickname: return messages.OrderBy(msg => msg.UserName).ToList();
                case Sort.GroupNickTime: return messages.OrderBy(msg => msg.GroupID).ThenBy(msg => msg.UserName).ThenBy(msg => msg.Date).ToList();
            }

            string error = "The sorting methid failed";
            Logger.Log.Fatal(Logger.Maintenance(error));

            throw new InvalidOperationException(error);
        }

        //Sort options
        public enum Sort
        {
            Time,
            Nickname,
            GroupNickTime
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

        //retrive and save the last 10 meseges from server. and return the new nessages that were not originaly in the ram
        public void SaveLast10FromServer(string url)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            List<IMessage> retrived, newData = new List<IMessage>(); ;

            if (url == null || url.Equals(""))
            {
                string error = "Recived an illegal url";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            try
            {
                retrived = Communication.Instance.GetTenMessages(url);
            }
            catch
            {
                string error = "Server was not found!";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw;
            } 

            foreach (IMessage msg in retrived)
            {
                try
                {
                    newData.Add(new Message(msg));  // We need to translate the retured message object to our message to avid problems
                }
                catch
                {

                }
            }

            RamData = newData;
        }

        #endregion

        //-----------------------------------------------------------------

        #region override methods

        // deserialize messages
        protected override ICollection<IMessage> deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return MessageSerializationService.deserialize();
        }

        // serialize messages
        protected override bool serialize(ICollection<IMessage> Data)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return MessageSerializationService.serialize(RamData);
        }

        //Sorting by time
        protected override ICollection<IMessage> DefaultSort(ICollection<IMessage> Data)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return sort(Data, Sort.Time, false);
        }

        #endregion

        #region Unused code

        //Edid message by guid and save to the RAM and disk
        private void EditMessage(Guid ID, string newBody)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            //A dummy message to use in the .equals
            IMessage dummy = new Message(ID, DateTime.Now, new User("Dummy",0), "Dummy");

            foreach (Message msg in RamData)
            {
                if (msg.Equals(dummy))
                {
                    msg.editBody(newBody);
                    UpdateDisk();
                    return;
                }
            }

            string error = "Could not found a message with the ruqusted guid of " + ID;
            Logger.Log.Error(Logger.Maintenance(error));

            throw new KeyNotFoundException(error);
        }

        // save a single message to the RAM
        private void SaveMessage(IMessage msg)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            RamData.Add(msg);
            UpdateDisk();
        }

        #endregion

    }
}
