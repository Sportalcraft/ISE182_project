using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    [Serializable]
    // This class Implaments the Imessage interface
    // and represen a message in the chatroom
    class Message : IMessage
    {
        private Guid _g_id;              // The unique idetifier of the message
        private DateTime _receivingTime; // The time the server received the message
        private IUser _sender;           // The sender user
        private string _body;            // The message’s content

        //Getter to the reciving time
        public DateTime Date
        {
            get { return _receivingTime; }
        }

        //Getter to the group ID
        public string GroupID
        {
            get { return _sender.Goup_ID.ToString(); }
        }

        //Getter to the guid
        public Guid Id
        {
            get { return _g_id; }
        }

        //Getter to the content of the message
        public string MessageContent
        {
            get { return _body; }
            private set { _body = value; }
        }

        //Getter to the sender nuckname
        public string UserName
        {
            get { return _sender.NickName; }
        }

        //The constractor method
        public Message(Guid guid, DateTime receivingTime, IUser sender, string body)
        {
            if (!isValid(body)) //check if the message is valid
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

        // Cheack if two messages are equals.
        // Two mesages are equals if they both have the same guid
        public override bool Equals(object obj)
        {
            if (!(obj is IMessage))
                return false;

            IMessage other = (IMessage)obj;

            return Id.Equals(other.Id);
        }

        //Cheak if the body is valid
        private bool isValid(string body)
        {
            return body.Length < 150;
        }
    }
}
