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

    //This class enable serializtion and deserializtion of users to the disk
    class UserSerializationService
    {
        private const string USERS_LIST = "Users.bin"; // The file name to save the users

        //Serialze a list of users
        public static void serialize(ArrayList users)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            SerializationService.serialize(users, USERS_LIST);
        }

        //Deserialize all users from the disk
        public static ArrayList deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ArrayList temp = (ArrayList)SerializationService.deserialize(USERS_LIST);

            if (temp == null)
            {
                Logger.Log.Warn(Logger.Developer("deserialized null users list from " + USERS_LIST + ", returning an empty Arraylist"));
                return new ArrayList();
            }

            return temp;
        }
    }
}
