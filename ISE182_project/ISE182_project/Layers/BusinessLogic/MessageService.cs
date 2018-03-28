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
                MergeTwoArrays.mergeIntoFirst(_ramMessages, value);  // Merging to avoid duplication
                sort(_ramMessages);                                  // Sorting
                MessageSerializationService.serialize(_ramMessages); // Serialize the new list
            }
            get
            {
                if (_ramMessages == null)
                    _ramMessages = MessageSerializationService.deserialize(); // Deserialize messages to the ram if not there alrady

                return _ramMessages;
            }
        }

        //return the last n Serialized messages
        public static ArrayList lastNmesages(int amount)
        {
            ArrayList toReturn = new ArrayList(amount);

            for (int i = 0; i < amount & i < RamMessages.Count; i++)
            {
                toReturn.Add(RamMessages[i]);
            }

            return toReturn;
        }

        public static ArrayList last20Mesages(int amount)
        {
            return lastNmesages(20);
        }

        //-----------------------------------------------------------------

        //Sort a message List by the time
        private static void sort(ArrayList messages)
        {
            messages.Sort(new MessageComparatorByDate());
        }

    }
}
