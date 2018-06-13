using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using ISE182_project.Layers.LoggingLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ISE182_project.Layers.PresentationLayer.GUI.Windows
{
    /// <summary>
    /// Interaction logic for EditMenu.xaml
    /// </summary>
    public partial class EditMenu : Window
    {
        private ObservableObject _bindObject; //Binding object
        private IMessage _msg; //The the message

        //A constructor
        public EditMenu(IMessage msg,  ObservableObject obs)
        {
            InitializeComponent();
            _bindObject = obs;
            _msg = msg;
            DataContext = _bindObject;

            _bindObject.OldContent = _msg.MessageContent;
        }

        #region Events

        //Applay changes event
        private void Aplay_Click(object sender, RoutedEventArgs e)
        {
            edit();
        }

        //Apllay changes By Enter
        private void TextBox_Enter_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                TextBox box = sender as TextBox;
                _bindObject.EditMessageContent = box.Text;

                edit();
            }
        }

        #endregion

        #region Private Metods

        //display error message
        private void Error(string error)
        {
            _bindObject.ErrorText = error;
            Logger.Log.Error(Logger.Maintenance(error));

            Error ePage = new Error(_bindObject);
            ePage.Show();
        }

        //Edit the message
        private void edit()
        {
            if (!_msg.UserName.Equals(ChatRoom.LoggedUser.NickName) | !_msg.GroupID.Equals(ChatRoom.LoggedUser.Group_ID.ToString()))
            {
                Error("You can edit only your own messges!");
                return;
            }

            try
            {
                ChatRoom.EditMessage(_msg.Id, _bindObject.EditMessageContent);

                Hide(); // succesfully edited message
            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }
        }

        #endregion
    }
}
