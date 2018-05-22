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
using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.PresentationLayer;
using ISE182_project.Layers;
using System.Windows.Threading;
using ISE182_project.Layers.CommunicationLayer;

namespace ISE182_project
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ObservableObject bindObject;
        private DispatcherTimer dispatcherTimer;

        public ChatWindow(ObservableObject fromMainWindows)
        {
            InitializeComponent();
            this.bindObject = fromMainWindows;
            this.DataContext = bindObject;
            bindObject.Username = "Username: " + ChatRoom.LoggedUser.NickName;
            bindObject.GroupID = "GroupID: " + ChatRoom.LoggedUser.Group_ID.ToString();


            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;         // add event
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);  // set time between ticks
            dispatcherTimer.Start();

            UpdateScreen();       
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ChatRoom.SaveLast10FromServer();
            UpdateScreen();
        }


        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.send(bindObject.MessageContent);
            UpdateScreen();
            bindObject.MessageContent = "";
        }
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.logout();
            dispatcherTimer.Stop();
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }
        private void UpdateList(ICollection<IMessage> list)
        {
            string temp;

            foreach (IMessage msg in list)
            {
                temp = msg.ToString();

                if (!bindObject.Messages.Contains(temp))
                    bindObject.Messages.Add(temp + "\n");
            }
        }

        private void UpdateScreen()
        {
            UpdateList(ChatRoom.request20Messages());
        }
    }
}
