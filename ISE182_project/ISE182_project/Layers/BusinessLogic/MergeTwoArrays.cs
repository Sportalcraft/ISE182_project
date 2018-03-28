using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            foreach (object obj in array2)
            {
                if (!array1.Contains(obj))
                    array1.Add(obj);
            }
        }
    }
}
