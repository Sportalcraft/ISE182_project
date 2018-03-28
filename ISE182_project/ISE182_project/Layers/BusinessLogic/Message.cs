using ISE182_project.Layers.CommunicationLayer;
using MileStoneClient.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    class Message : IMessage
    {
        private Guid _g_id;              // The unique idetifier of the message
        private DateTime _receivingTime; // The time the server received the message
        private IUser _sender;           // The sender user
        private string _body;            // The message’s content


        public DateTime Date
        {
            get { return _receivingTime; }
        }

        public string GroupID
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Guid Id
        {
            get { return _g_id; }
        }

        public string MessageContent
        {
            get { return _body; }
            private set { _body = value; }
        }

        public string UserName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Message(Guid guid, DateTime receivingTime, IUser sender, string body)
        {
            if (!isValid(body))
                throw new ArgumentException("Message is too long!");

            _g_id = guid;
            _receivingTime = receivingTime;
            _sender = sender;
            _body = body;
        }

        //Edit the message's body
        public void editBody(string newBody)
        {
            MessageContent = newBody;
        }

        //Cheak if the body is valid
        private bool isValid(string body)
        {
            return body.Length < 150;
        }
    }
}
