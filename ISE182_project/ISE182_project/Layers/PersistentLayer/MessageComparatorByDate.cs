using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    class MessageComparatorByDate : IComparer
    {
        //This class impaments the IComparer iterface, 
        //and making a ratio order acrding to the message time  

        public int Compare(object x, object y) //Implamenting IComparer
        {
            IMessage msg1 = (IMessage)x;
            IMessage msg2 = (IMessage)y;

            return msg1.Date.CompareTo(msg2.Date);
        }
    }
}
