﻿using ISE182_project.Layers.LoggingLayer;
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

        //Serialze a list of users. return if it was done successfully
        public static bool serialize<T>(ICollection<T> users)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return SerializationService.serialize(users, USERS_LIST);
        }

        //Deserialize all users from the disk
        public static ICollection<T> deserialize<T>()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            ICollection<T> temp = (ICollection<T>)SerializationService.deserialize(USERS_LIST);

            if (temp == null)
            {
                string error = "deserialized null users list from " + USERS_LIST + ", returning an empty list";
                Logger.Log.Warn(Logger.Developer(error));

                throw new IOException(error);
            }

            return temp;
        }
    }
}
