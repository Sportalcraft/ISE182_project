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
    static class MessageService
    {
        private static ICollection<IMessage> _ramMessages; // Store a coppy of the messages in the ram for quick acces

        //Getter and setter to the messages stored in the ram
        private static ICollection<IMessage> RamMessages
        {
             set
            {
                if (_ramMessages == null) // There is no stored messages
                {
                    string error = "the messages on the ram has not been initialize!";
                    Logger.Log.Fatal(Logger.Maintenance(error));

                    throw new NullReferenceException(error);
                }

                MergeTwoCollections.mergeIntoFirst(_ramMessages, value);// merging to avoid duplication
                sort(_ramMessages,Sort.Time, false);               // sorting

                if (!MessageSerializationService.serialize(_ramMessages)) // serialize the new list
                {
                    string error = "faild to serialize messages";
                    Logger.Log.Warn(Logger.Maintenance(error));
                    _ramMessages = new List<IMessage>();
                    //throw new IOException(error);
                }
            }

            get
            {
                if (_ramMessages == null) // Deserialize messages to the ram if not there alrady
                {
                    _ramMessages = MessageSerializationService.deserialize<IMessage>();
                }

                return _ramMessages;
            }
        }

        //Initiating the ram's saves from messages stored in the disk
        public static void start()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            SetRAM();
        }

        #region Filter

        //recive all the messages from a certain user
        public static ICollection<IMessage> FilterByUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return (ICollection<IMessage>)RamMessages.Where(msg => user.Equals(new User(msg.UserName, int.Parse(msg.GroupID))));
        }

        //recive all the messages from a certain group
        public static ICollection<IMessage> FilterByGroup(int groupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return (ICollection<IMessage>)RamMessages.Where(msg => int.Parse(msg.GroupID) == groupID);
        }

        #endregion

        #region Sort

        //Sort a message List by the time
        public static void sort(ICollection<IMessage> messages, Sort SortBy, bool descending)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if(descending)
            {
                switch (SortBy)
                {
                    case Sort.Time: messages.OrderByDescending(msg => msg.Date); return;
                    case Sort.Nickname: messages.OrderByDescending(msg => msg.UserName); return;
                    case Sort.GroupNickTime: messages.OrderByDescending(msg => msg.UserName).ThenByDescending(msg => msg.UserName).ThenByDescending(msg => msg.Date); return;
                }
            }

            switch(SortBy)
            {
                case Sort.Time: messages.OrderBy(msg => msg.Date); return;
                case Sort.Nickname: messages.OrderBy(msg => msg.UserName); return;
                case Sort.GroupNickTime: messages.OrderBy(msg => msg.UserName).ThenBy(msg => msg.UserName).ThenBy(msg => msg.Date); return;
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

        //Edid message by guid and save to the RAM and disk
        public static void EditMessage(Guid ID, string newBody)
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

        //return the last n saved messages
        public static ICollection<IMessage> lastNmesages(int amount)
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

            return (ICollection<IMessage>)RamMessages.Reverse().Take(amount).Reverse();
        }

        //retrive and save the last 10 meseges from server.
        public static void SaveLast10FromServer(string url)
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

            foreach(IMessage msg in retrived)
            {
                temp.Add(new Message(msg)); // We need to translate the retured message object to our message to avid problems
            }

            RamMessages = temp;
        }


        //-----------------------------------------------------------------

        #region private methods

        //Update the stored in disk messages after changing ram
        private static void UpdateDisk()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            RamMessages = new LinkedList<IMessage>(); //So the set atribulte will activate to ask to save to disk
        }

        //Setting the ram, if null
        private static void SetRAM()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ICollection<IMessage> temp = RamMessages; //So the get atribulte will activate to ask to draw from disk
        }

        //Unused - save a single message to the RAM
        private static void SaveMessage(IMessage msg)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            RamMessages.Add(msg);
            UpdateDisk();
        }
        #endregion

    }
}
