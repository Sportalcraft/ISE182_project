using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
   
    // This class impaments the IComparer iterface, 
    // and creating a ratio order to the messages acording to the message reciving time  
    class MessageComparatorByDate : IComparer
    {

        //Implamenting IComparer.
        // Compare two messages.
        // Negative number stand for x < y
        // 0               stand for x = y
        // Positive number stand for x > y
        public int Compare(object x, object y) 
        {
            IMessage msg1 = (IMessage)x;
            IMessage msg2 = (IMessage)y;

            return msg1.Date.CompareTo(msg2.Date); //Use the DateTime compareTo
        }
    }
}
