using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.PersistentLayer
{
    class MessageComparator : IComparer
    {
        public int Compare(object x, object y)
        {
            IMessage msg1 = (IMessage)x;
            IMessage msg2 = (IMessage)y;

            return msg1.Date.CompareTo(msg2.Date);
        }
    }
}
