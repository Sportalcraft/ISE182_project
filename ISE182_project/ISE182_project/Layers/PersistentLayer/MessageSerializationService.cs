using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    //This class enable serializtion and deserializtion of messages to the disk
     static class MessageSerializationService
    {
        private const string MESSAGE_LIST = "Messages.bin"; // The file name to save the mesages

        // serialize a *sorted* list of messages. return if it was done successfully
        public static bool serialize(ICollection<IMessage> messages)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return SerializationService.serialize(messages, MESSAGE_LIST);
        }

        //Deserialize all messages from the disk
        public static ICollection<IMessage> deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return SerializationService.deserialize(MESSAGE_LIST) as ICollection<IMessage>;
        }
    }
}

