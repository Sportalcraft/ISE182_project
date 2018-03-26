using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistenceLayer
{
    static class MessageHandler
    {
        private const string MESSAGE_LIST = "Messages.bin"; //The file name to save the mesages



        public static void SerializeMesages(LinkedList<IMessage> messages) //Cheak if legal
        {
            if(File.Exists(MESSAGE_LIST))
            {
                reSerializeMesages(messages);
            }
            else
            {
                SerializationService.Serialize(messages, MESSAGE_LIST);
            }        
        }

        public static LinkedList<IMessage> DeserializeMesages() 
        {
            return  (LinkedList<IMessage>)SerializationService.Deserialize(MESSAGE_LIST);
        }

        public static LinkedList<IMessage> DerializeMesages(int amount)
        {
            LinkedList<IMessage> messages = DeserializeMesages();
            LinkedList<IMessage> toReturn = new LinkedList<IMessage>();

            foreach (IMessage msg in messages)
            {
                if (amount == 0)
                    return toReturn;

                toReturn.AddLast(msg);
            }

            return toReturn;
        }


        //-----------------------------------------------------------------

        // Serialize when an old Serializion exost 
        private static void reSerializeMesages(LinkedList<IMessage> messages)
        {
            LinkedList<IMessage> oldMessages = DeserializeMesages();
            MergeIntoFirst(oldMessages, messages);
            SerializationService.Serialize(oldMessages, MESSAGE_LIST);
        }

        // Merge to lists of messages 
        private static void MergeIntoFirst(LinkedList<IMessage> msgs1, LinkedList<IMessage> msgs2)
        {
           foreach(IMessage msg in msgs2)
           {
                if (!Exist(msgs1, msg.Id))
                    msgs1.AddLast(msg);
           }
        }

        //check if an old message already exists
        private static bool Exist(LinkedList<IMessage> messages, Guid guid)
        {
            foreach (IMessage msg in messages)
            {
                if (msg.Id.Equals(guid))
                    return true;
            }
            return false;
        }

    }
}
