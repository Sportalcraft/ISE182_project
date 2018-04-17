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

        //Getter and setter to the messages stored in the ram
        private ICollection<IMessage> RamMessages
        {
            set
            {
                if(value == null)
                {
                    string error = "recived null value for messages";
                    Logger.Log.Error(Logger.Maintenance(error));

                    throw new ArgumentNullException(error);
                }

                ICollection<IMessage> temp = sort(value, Sort.Time, false); // sorting
                RamData = temp;
            }

            get { return RamData; }
        }

        #region Filter

        //recive all the messages from a certain user
        public ICollection<IMessage> FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return RamMessages.Where(msg => user.Equals(new User(msg.UserName, int.Parse(msg.GroupID)))).ToList();
        }

        //recive all the messages from a certain group
        public ICollection<IMessage> FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return RamMessages.Where(msg => int.Parse(msg.GroupID) == groupID).ToList();
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
                    case Sort.GroupNickTime: return messages.OrderByDescending(msg => msg.UserName).ThenByDescending(msg => msg.UserName).ThenByDescending(msg => msg.Date).ToList();
                }
            }

            switch (SortBy)
            {
                case Sort.Time: return messages.OrderBy(msg => msg.Date).ToList();
                case Sort.Nickname: return messages.OrderBy(msg => msg.UserName).ToList();
                case Sort.GroupNickTime: return messages.OrderBy(msg => msg.UserName).ThenBy(msg => msg.UserName).ThenBy(msg => msg.Date).ToList();
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

            if (amount > RamMessages.Count)
            {
                Logger.Log.Warn(Logger.Maintenance("User requested more messages then there is to show"));
                amount = RamMessages.Count;
            }

            return RamMessages.Reverse().Take(amount).Reverse().ToList();
        }

        //retrive and save the last 10 meseges from server.
        public void SaveLast10FromServer(string url)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            List<IMessage> retrived;

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

            ICollection<IMessage> temp = new List<IMessage>();

            foreach (IMessage msg in retrived)
            {
                temp.Add(new Message(msg)); // We need to translate the retured message object to our message to avid problems
            }

            RamMessages = temp;
        }

        //-----------------------------------------------------------------

        #region override methods

        // deserialize messages
        protected override ICollection<IMessage> deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return MessageSerializationService.deserialize();
        }

        // serialize messages
        protected override bool serialize(ICollection<IMessage> _ramData)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            return MessageSerializationService.serialize(RamMessages);
        }

        #endregion

        #region Unused code

        //Edid message by guid and save to the RAM and disk
        private void EditMessage(Guid ID, string newBody)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            //A dummy message to use in the .equals
            IMessage dummy = new Message(ID, DateTime.Now, new User("Dummy"), "Dummy");

            foreach (Message msg in RamMessages)
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

            RamMessages.Add(msg);
            UpdateDisk();
        }

        #endregion

    }
}
