using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    //This class have one method to merge two collections into the first one, without duplication
    static class MergeTwoCollections
    {

        // Merge two collections to the first one without duplication
        public static void mergeIntoFirst<T>(ICollection<T> list1, ICollection<T> list2)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if(list1==null | list2==null)
            {
                string error = "Can't merge null";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentNullException(error);
            }

            if (list1 == list2)
                return; //if they are pointing to the same collection, then we can exit.

            foreach (T item in list2)
            {
                if (!list1.Contains(item))
                    list1.Add(item);
            }
        }
    }
}
