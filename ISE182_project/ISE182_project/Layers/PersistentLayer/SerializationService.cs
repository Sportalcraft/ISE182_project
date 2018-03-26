using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    static class SerializationService
    {
        public static void Serialize(object toSerialize, string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Create);
            BinaryFormatter formater = new BinaryFormatter();
            formater.Serialize(stream, toSerialize);
            stream.Close();
        }

        public static object Deserialize(string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.Open);
            BinaryFormatter formater = new BinaryFormatter();
            object derialized = formater.Deserialize(stream);
            stream.Close();
            
            return derialized;
        }
    }
}
