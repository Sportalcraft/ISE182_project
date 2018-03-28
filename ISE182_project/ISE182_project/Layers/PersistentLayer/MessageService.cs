using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    static class MessageService
    {
        private const string MESSAGE_LIST = "Messages.bin"; // The file name to save the mesages
        private static ArrayList _ramMessages;              // Store a coppy of the messages in the ram for quick acces

        //Getter and setter to the messages stored in the ram
        public static ArrayList RamMessages
        {
            private set { _ramMessages = value; }
            get
            {
                if (_ramMessages == null)
                    deserializeAllMesages();

                return _ramMessages;
            }
        }


        public static void serializeMesages(ArrayList messages)
        {
            if (File.Exists(MESSAGE_LIST))
            {
                mergeIntoFirst(RamMessages, messages);
                messages = RamMessages;
            }

            sort(messages);
            serialize(messages);
            RamMessages = messages;
        }

        public static ArrayList last20Mesages(int amount)
        {
            return lastNmesages(20);
        }

        //-----------------------------------------------------------------

        #region private methods

        //Deserialize all messages from the disk and save to ram
        private static ArrayList deserializeAllMesages()
        {
            ArrayList temp = (ArrayList)SerializationService.Deserialize(MESSAGE_LIST);
            sort(temp);
            RamMessages = temp;
            return temp;
        }

        //return the last n Serialized messages
        private static ArrayList lastNmesages(int amount)
        {
            ArrayList toReturn = new ArrayList(amount);

            for (int i = 0; i < amount & i < RamMessages.Count; i++)
            {
                toReturn.Add(RamMessages[i]);
            }

            return toReturn;
        }

        //Serialze a list of messages
        private static void serialize(ArrayList messages)
        {
            SerializationService.Serialize(messages, MESSAGE_LIST);
        }

        //Sort a message List by the time
        private static void sort(ArrayList messages)
        {
            messages.Sort(new MessageComparatorByDate());
        }

        // Merge two lists of messages to the first one
        private static void mergeIntoFirst(ArrayList msgs1, ArrayList msgs2)
        {
            foreach (IMessage msg in msgs2)
            {
                if (!msgs1.Contains(msg))
                    msgs1.Add(msg);
            }
        }

        #endregion

    }
}
