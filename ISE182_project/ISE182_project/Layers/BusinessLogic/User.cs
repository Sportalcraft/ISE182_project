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
        private const int MIN_USERNAME_LENGTH = 1; //Minimum username lenfth
        private const int MAX_USERNAME_LENGTH = 8; //Maximum username lenfth

        #region Constractors

        public User(string nickName, int GroupID) : base(nickName, GroupID)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            if (!isValidUserName(nickName))
            {
                string error = "The username is illegal";
                Logger.Log.Error(Logger.Maintenance(error));

                throw new ArgumentException(error);
            }
        }

        #endregion

        #region Validation

        //check passwod validity
        public static bool isValidPassword(string Password)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return Password != null && //Not null
                Password.Length >= MIN_PASSWORD_LENGTH && // Not too short
                Password.Length <= MAX_PASSWORD_LENGTH; //Not too long
        }

        //check user name validity
        public static bool isValidUserName(string userName)
        {
            Logger.Log.Debug(Logger.MethodStart(MethodBase.GetCurrentMethod()));

            return userName != null && //Not null
                userName.Length >= MIN_USERNAME_LENGTH && // Not too short
                userName.Length <= MAX_USERNAME_LENGTH; //Not too long
        }


        #endregion

        #region functionalities

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
