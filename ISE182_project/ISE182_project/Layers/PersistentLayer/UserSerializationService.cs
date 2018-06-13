using ISE182_project.Layers.BusinessLogic;
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
    static class UserSerializationService
    {
        private const string USERS_LIST = "Users.bin"; // The file name to save the users

        //Serialze a list of users. return if it was done successfully
        public static bool serialize(ICollection<IUser> users)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return SerializationService.serialize(users, USERS_LIST);
        }

        //Deserialize all users from the disk
        public static ICollection<IUser> deserialize()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return SerializationService.deserialize(USERS_LIST) as ICollection<IUser>;
        }
    }
}

