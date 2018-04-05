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
            get { return _sender.Group_ID.ToString(); }
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

            set
            {
                if (!isValid(value))
                    throw new ArgumentException("the Message is not valid!");

                _body = value;
            }
        }

        //Getter to the sender nuckname
        public string UserName
        {
            get { return _sender.NickName; }
        }

        //Getter to the sender nuckname
        private IUser Sender
        {
            get { return _sender; }
        }

        //A constractor of message class
        public Message(Guid g_id, DateTime receivingTime, IUser sender, string body)
        {
            if (g_id == null | receivingTime == null | sender == null | body == null)
                throw new ArgumentException("Can't recive a null as am argument!");

            if(!isValid(body))
                throw new ArgumentException("Message is too long!");

            _g_id = g_id;
            _receivingTime = receivingTime;
            _sender = sender;
            _body = body;
        }

        //A copy constractor
        public Message(IMessage msg) : this(msg.Id, msg.Date, new User(msg.UserName, int.Parse(msg.GroupID)), msg.MessageContent) { }

        //Edit the message's body
        public void editBody(string newBody)
        {
            if (!isValid(newBody))
                throw new ArgumentException("the Message is not valid!");

            MessageContent = newBody;
            MessageService.EditMessage(this);
        }

        //Cheak if the body is valid
        public static bool isValid(string body)
        {
            return body.Length < 150;
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

        //return a string tat represent the message
        public override string ToString()
        {
            return "Message : \nGuid: " + Id + "\nRecivingTime : " + Date + 
                "\nSender : " + Sender + "\nMessage Body : " + MessageContent;
        }
    }
}
