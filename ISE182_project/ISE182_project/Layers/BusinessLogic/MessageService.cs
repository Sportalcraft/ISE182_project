using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.PersistentLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    //This class manege the messages stored in RAM
    static class MessageService
    {
        private static ArrayList _ramMessages; // Store a coppy of the messages in the ram for quick acces

        //Getter and setter to the messages stored in the ram
        public static ArrayList RamMessages
        {
             private set
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
                    _ramMessages = MessageSerializationService.deserialize();

                return _ramMessages;
            }
        }


        //save a single message to the RAM
        public static void SaveMessage(IMessage msg)
        {
            RamMessages.Add(msg);
            Update();
        }

        //Edid message and save to the RAM and disk
        public static void EditMessage(IMessage editedMessage)
        {
            foreach(Message msg in RamMessages)
            {
                if (msg.Equals(editedMessage))
                    msg.MessageContent = editedMessage.MessageContent;
            }

            Update();
        }

        //return the last n saved messages
        public static ArrayList lastNmesages(int amount)
        {
            ArrayList toReturn = new ArrayList(amount);

            for (int i = 0; i < amount & i < RamMessages.Count; i++)
            {
                toReturn.Add(RamMessages[i]);
            }

            return toReturn;
        }

        //Get the last 20 messages stored in RAM
        public static ArrayList last20Mesages(int amount)
        {
            return lastNmesages(20);
        }

        //retrive and save the last 10 meseges from server
        public static void SaveLast10FromServer(string url)
        {
            RamMessages = new ArrayList(Communication.Instance.GetTenMessages(url));
        }


        //-----------------------------------------------------------------

        //Sort a message List by the time
        private static void sort(ArrayList messages)
        {
            messages.Sort(new MessageComparatorByDate());
        }

        //Update the stored in disk mesages after changing ram
        private static void Update()
        {
            RamMessages = RamMessages; //So the set atribulte will activate to ask to save to disk
        }

    }
}
