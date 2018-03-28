using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{

    //This class enable serializtion and deserializtion to the disk of any kind
    static class SerializationService
    {

        //Serialize an object into afile in  the given file name
        public static void serialize(object toSerialize, string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter formater = new BinaryFormatter();
            formater.Serialize(stream, toSerialize); //Serializing
            stream.Close();
        }

        //deserialze an object from the given file name
        public static object deserialize(string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter formater = new BinaryFormatter();
            object derialized = formater.Deserialize(stream); //Deserializing 
            stream.Close();
            
            return derialized;
        }
    }
}
