using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ISE182_project.Layers.BusinessLogic
{
    [Serializable]
    // This class Implaments the Imessage interface
    // and represen a message in the chatroom
    class Message : DisplayMessage
    {
        #region Ctors

        //A constractor of message class
        public Message(Guid guid, DateTime receivingTime, string userName, int GroupID, string body):base(guid, receivingTime, userName, GroupID, body)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
        }

        //A copy constractor
        public Message(IMessage msg) : base(msg.Id, msg.Date, msg.UserName, int.Parse(msg.GroupID), msg.MessageContent)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
        }

        //A constractor of message class
        public Message(Guid guid, DateTime receivingTime, int group, string nickName, string body) : this(guid, receivingTime, nickName, group, body)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
        }

        //A constractor of message class
        public Message(int group, string nickName, string body) : base(Guid.NewGuid(), DateTime.Now.ToUniversalTime(), nickName, group, body)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
        }

        #endregion

        //Edit the message's body.
        public void editBody(string newBody)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            MessageContent = newBody; //edit the body
        }
    }
}
