using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.CommunicationLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Threading;

namespace ISE182_project.Layers.PresentationLayer.GUI.Windows
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ObservableObject bindObject; //The Binding object
        private DispatcherTimer dispatcherTimer; //The timer

        //A Constructor
        public ChatWindow(ObservableObject fromMainWindows)
        {
            InitializeComponent();
            this.bindObject = fromMainWindows;
            this.DataContext = bindObject;
            bindObject.Username = "Username: " + ChatRoom.LoggedUser.NickName;
            bindObject.GroupID = "GroupID: " + ChatRoom.LoggedUser.Group_ID.ToString();
            bindObject.FilterNone = true;
            bindObject.SortAscending = true;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;         // add event
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);  // set time between ticks
            dispatcherTimer.Start();
            UpdateScreen();
        }

        #region Events

        //Timer Tick Event
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ChatRoom.DrawLastMessages();
            UpdateScreen();
        }

        //Send button click event
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            send(bindObject.MessageContent);
        }

        //Logout button click event
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChatRoom.logout();
                dispatcherTimer.Stop();
                MainWindow main = new MainWindow();
                main.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                bindObject.ErrorText = ex.Message;
                Error ePage = new Error(bindObject);
                ePage.Show();
            }
        }

        //Sort direction changed event
        private void Sort_RadioButton_Click(object sender, RoutedEventArgs e)
        {
            sort();
        }

        //The sort option changed event
        private void SortOptionns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sort();
        }

        //add filter optins
        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            if (bindObject.FilterNone)
                ChatRoom.resetFilters();
        }

        //Apply button click
        private void Apply_Click(object sender, RoutedEventArgs e)
        {

            filter();

            //reset data
            bindObject.UsernameBox = "";
            bindObject.GroupidBox = "";

            UpdateScreen();
        }

        //Enter clicked event
        private void TextBox_KeyDownSendMessage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox box = sender as TextBox;
                send(box.Text);
                box.Text = "";
            }
        }

        #endregion

        #region Private Methods

        //Update the messages displayed on screen
        private void UpdateScreen()
        {
            ICollection<IMessage> list = ChatRoom.getMessages();
            bindObject.Messages.Clear();

            foreach (IMessage m in list)
                bindObject.Messages.Add(m);
        }

        //Send a new message
        private void send(string message)
        {
            try
            {
                ChatRoom.send(message);
                bindObject.MessageContent = "";
                UpdateScreen();

            }
            catch (Exception ex)
            {
                bindObject.ErrorText = ex.Message;
                Error ePage = new Error(bindObject);
                ePage.Show();
            }
        }

        //Sort the messages
        private void sort()
        {
            ChatRoom.Sort option;

            switch (bindObject.SortOption)
            {
                case 0: option = ChatRoom.Sort.Time; break;
                case 1: option = ChatRoom.Sort.Nickname; break;
                case 2: option = ChatRoom.Sort.GroupNickTime; break;
                default: option = ChatRoom.Sort.Time; break;
            }

            ChatRoom.sort(option, bindObject.SortDescending); //sorting
            UpdateScreen(); //Update view
        }

        private void filter()
        {
            if (bindObject.FilterUser)
                ChatRoom.filterByUser(bindObject.FilterNameString, int.Parse(bindObject.FilterGroupString));
            
            if (bindObject.FilterGroupid)
                ChatRoom.filterByGroup(int.Parse(bindObject.FilterGroupString));

        }

        #endregion
    }
}