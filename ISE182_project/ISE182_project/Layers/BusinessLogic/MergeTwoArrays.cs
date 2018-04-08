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
    //This class have one method to merge two arrylists into the first one, without duplication
    static class MergeTwoArrays
    {
        // Merge two ArrayLists to the first one without duplication
        public static void mergeIntoFirst(ArrayList array1, ArrayList array2)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if(array1==null | array2==null)
            {
                Logger.Log.Error(Logger.Maintenance("Can't merge a null array"));
                return;
            }

            if (array1 == array2)
                return; //if they are pointing to the same array, then we can exit.

            foreach (object obj in array2)
            {
                if (!array1.Contains(obj))
                    array1.Add(obj);
            }
        }
    }
}
