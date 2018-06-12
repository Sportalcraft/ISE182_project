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
    // This class implaments the IUser interface,
    // and represent a user in the chatroom
    class User : DisplayUser, IUser
    {
        private const int MIN_PASSWORD_LENGTH = 4; //Minimum password lenfth
        private const int MAX_PASSWORD_LENGTH = 8; //Maximum password lenfth

        private string _password; // the password of the user

        public User(string nickName, int GroupID /*, string password*/) : base(nickName, GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            string password = "123456";

            if (!isValidPassword(password))
            {
                string error = "The paswword is illegal";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            _password = password;
        }

        //getter to the Passwrd
        public string Password
        {
            get { return _password; }
        }

        #region functionalities

        //check passwod validity
        public bool isValidPassword(string Password)
        {
            return Password != null && //Not null
                Password.Length >= MIN_PASSWORD_LENGTH && // Not too short
                Password.Length <= MAX_PASSWORD_LENGTH; //Not too long
        }


        //Send a new message to the chatroom and save it
        public void send(string msg, int id)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!Message.isValid(msg))
            {
                string error = "The message the user requsted to send is illegal";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }

            IMessage message = new Message(this.Group_ID, this.NickName, msg);

            try
            {
                MessageService.Instence.send(message, id); //send mesage
            }
            catch
            {
                string error = "Error accured while sending a message!";
                Logger.Log.Fatal(Logger.Maintenance(error));

                throw;
            }
        }

        //logout from the server
        public void logout()
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));
            Logger.Log.Info(Logger.Maintenance("The user " + NickName + " (GroupID : " + Group_ID + ") loggedout."));
        }

        #endregion
    }
}
