using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{

    //This class enable serializtion and deserializtion to the disk of any kind
    public static class SerializationService
    {

        //Serialize an object into afile in  the given file name. return if it was done successfully
        public static bool serialize(object toSerialize, string fileName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            Stream stream = File.Open(fileName, FileMode.OpenOrCreate);
            BinaryFormatter formater = new BinaryFormatter();
            bool succeed = true;

            try
            {
                formater.Serialize(stream, toSerialize); //Serializing
            }
            catch (Exception e)
            {
                string temp = e.Message;

                Logger.Log.Error(Logger.Developer("Failed to serialize to " + fileName));
                succeed = false;
            }
            finally
            {
                stream.Close();
            }

            return succeed ;
        }

        //deserialze an object from the given file name
        public static object deserialize(string fileName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            Stream stream = File.Open(fileName, FileMode.OpenOrCreate);
            BinaryFormatter formater = new BinaryFormatter();
            object derialized;

            try
            {
                derialized = formater.Deserialize(stream); //Deserializing 
            }
            catch
            {
                Logger.Log.Error(Logger.Developer("Failed to deserialized from " + fileName + " returning null."));
                derialized = null;
            }
            finally
            {
                stream.Close();
            }
            
            return derialized;
        }
    }
}

