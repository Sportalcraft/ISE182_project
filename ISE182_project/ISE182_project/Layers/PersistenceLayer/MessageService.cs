using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistenceLayer
{
    static class MessageService
    {
        private const string MESSAGE_LIST = "Messages.bin"; //The file name to save the mesages



        public static void SerializeMesages(ArrayList messages) //Cheak if legal
        {
            if (File.Exists(MESSAGE_LIST))
            {
                reSerializeMesages(messages);
            }
            else
            {
                Sort(messages);
                Serialize(messages);
            }        
        }

        public static ArrayList DeserializeAllMesages() 
        {
            ArrayList temp =  (ArrayList)SerializationService.Deserialize(MESSAGE_LIST);
            Sort(temp);
            return temp;
        }

        public static ArrayList DerializeLast20Mesages(int amount)
        {
            return DerializeMesages(20);
        }

        //-----------------------------------------------------------------

        #region private methods

        //return the last n Serialized messages
        private static ArrayList DerializeMesages(int amount)
        {
            ArrayList messages = DeserializeAllMesages();
            ArrayList toReturn = new ArrayList();

            foreach (IMessage msg in messages)
            {
                toReturn.Add(msg);
            }

            Sort(toReturn);
            return toReturn;
        }

        //Serialze a list
        private static void Serialize(ArrayList messages)
        {
            SerializationService.Serialize(messages, MESSAGE_LIST);
        }

        // Serialize when an old Serializion exost 
        private static void reSerializeMesages(ArrayList messages)
        {
            ArrayList oldMessages = DeserializeAllMesages();
            MergeIntoFirst(oldMessages, messages);
            Sort(oldMessages);
            Serialize(messages);
        }

        // Merge to lists of messages 
        private static void MergeIntoFirst(ArrayList msgs1, ArrayList msgs2)
        {
           foreach(IMessage msg in msgs2)
           {
                if (!Exist(msgs1, msg.Id))
                    msgs1.Add(msg);
           }
        }

        //check if an old message already exists
        private static bool Exist(ArrayList messages, Guid guid)
        {
            foreach (IMessage msg in messages)
            {
                if (msg.Id.Equals(guid))
                    return true;
            }
            return false;
        }


        //Sort a message List by the time
        private static void Sort(ArrayList messages)
        {
            messages.Sort(new MessageComparator());
        }

        #endregion

    }
}
