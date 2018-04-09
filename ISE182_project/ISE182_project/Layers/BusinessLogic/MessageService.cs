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
        private static ArrayList _ramMessages; // Store a coppy of the messages in the ram for quick acces

        //Getter and setter to the messages stored in the ram
        private static ArrayList RamMessages
        {
             set
            {
                if (_ramMessages == null) // There is no stored messages
                {
                    _ramMessages = value;
                }

                MergeTwoArrays.mergeIntoFirst(_ramMessages, value);  // merging to avoid duplication
                sort(_ramMessages);                                  // sorting
                MessageSerializationService.serialize(_ramMessages); // serialize the new list
            }

            get
            {
                if (_ramMessages == null) // Deserialize messages to the ram if not there alrady
                {
                    _ramMessages = MessageSerializationService.deserialize();
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

        //Edid message by guid and save to the RAM and disk
        public static void EditMessage(Guid ID, string newBody)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (ID == null) 
            {
                Logger.Log.Error(Logger.Maintenance("recived a null guid to look fo a certain message"));
                return;
            }

            if(!Message.isValid(newBody))
            {
                Logger.Log.Error(Logger.Maintenance("recived an illegal message body to edit"));
                return;
            }

            //A dummy message to use in the .equals
            IMessage dummy = new Message(ID, DateTime.Now, new User("Dummy"), "Dummy");

            foreach (Message msg in RamMessages)
            {
                if (msg.Equals(dummy))
                    msg.editBody(newBody);
            }

            UpdateDisk();
        }

        //return all the messages from a certain user
        public static ArrayList AllMessagesFromUser(IUser user)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ArrayList toreturn = new ArrayList();
            IUser temp;

            if (user == null)
            {
                Logger.Log.Error(Logger.Maintenance("recived a null user mto look for. returning empty list"));
                return toreturn;
            }

            foreach (IMessage msg in RamMessages)
            {
                temp = new User(msg.UserName, int.Parse(msg.GroupID)); // Message sender

                if (user.Equals(temp)) // if the same sender as the given one
                {
                    toreturn.Add(msg);
                }
            }

            return toreturn;
        }

        //return the last n saved messages
        public static ArrayList lastNmesages(int amount)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (amount < 0)
            {
                Logger.Log.Error(Logger.Maintenance("User requested a negative number of mesaages"));
                amount = 0;
            }

            if (amount > RamMessages.Count)
            {
                Logger.Log.Warn(Logger.Maintenance("User requested more messages then there is to show"));               
                amount = RamMessages.Count;
            }

            ArrayList toReturn = new ArrayList();

            for (int i = RamMessages.Count - amount; i < RamMessages.Count; i++)
            {
                toReturn.Add(RamMessages[i]);
            }

            return toReturn;
        }

        //Get the last 20 messages stored in RAM
        public static ArrayList last20Mesages()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return lastNmesages(20);
        }

        //retrive and save the last 10 meseges from server.
        public static void SaveLast10FromServer(string url)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            List<IMessage> retrived;

            if(url ==null || url.Equals(""))
            {
                Logger.Log.Error(Logger.Maintenance("Recived an illegal url"));
                return;
            }

            try
            {
                retrived = Communication.Instance.GetTenMessages(url);
            }
            catch
            {
                Logger.Log.Fatal(Logger.Maintenance("Server was not found!"));
                return;
            }

            ArrayList temp = new ArrayList();

            foreach(IMessage msg in retrived)
            {
                temp.Add(new Message(msg)); // We need to translate the retured message object to our message to avid problems
            }

            RamMessages = temp;
        }


        //-----------------------------------------------------------------

        #region private methods

        //Sort a message List by the time
        private static void sort(ArrayList messages)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            messages.Sort(new MessageComparatorByDate());
        }

        //Update the stored in disk messages after changing ram
        private static void UpdateDisk()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            RamMessages = new ArrayList(); //So the set atribulte will activate to ask to save to disk
        }

        //Setting the ram, if null
        private static void SetRAM()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            ICollection temp = RamMessages; //So the get atribulte will activate to ask to draw from disk
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
