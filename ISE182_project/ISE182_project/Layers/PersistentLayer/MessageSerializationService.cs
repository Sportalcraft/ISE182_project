using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    //This class enable serializtion and deserializtion of messages to the disk
    class MessageSerializationService
    {
        private const string MESSAGE_LIST = "Messages.bin"; // The file name to save the mesages

        // serialize a *sorted* list of messages
        public static void serialize(ArrayList messages)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            SerializationService.serialize(messages, MESSAGE_LIST);
        }

        //Deserialize all messages from the disk
        public static ArrayList deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ArrayList temp = (ArrayList)SerializationService.deserialize(MESSAGE_LIST);

            if (temp == null)
            {
                Logger.Log.Warn(Logger.Developer("deserialized null messefes list from " + MESSAGE_LIST + ", returning an empty Arraylist"));
                return new ArrayList();
            }

            return temp;
        }
    }
}
