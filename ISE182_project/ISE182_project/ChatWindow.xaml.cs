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
using System.Collections.ObjectModel;

namespace ISE182_project
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ObservableObject bindObject;
        private DispatcherTimer dispatcherTimer;
        private ICollection<IMessage> last20;
        private bool sortChanged;
        private bool filterApplied;

        public ChatWindow(ObservableObject fromMainWindows)
        {
            InitializeComponent();
            this.last20 = new List<IMessage>();
            this.bindObject = fromMainWindows;
            this.DataContext = bindObject;
            bindObject.Username = "Username: " + ChatRoom.LoggedinUser.NickName;
            bindObject.GroupID = "GroupID: " + ChatRoom.LoggedinUser.Group_ID.ToString();
            bindObject.FilterNone = true;
            bindObject.SortAscending = true;

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
        private void Printer(ICollection<IMessage> list)
        {
            foreach (IMessage msg in list)
            {
                bindObject.Messages.Add(msg);
            }
        }

        private void UpdateScreen()
        {
            ICollection<IMessage> list = ChatRoom.request20Messages();
            foreach (IMessage m in this.last20)
                if (list.Contains(m))
                    list.Remove(m);
            MessageService.Sort option;
            if (bindObject.SortOption == 0)
                option = MessageService.Sort.Time;
            else if (bindObject.SortOption == 1)
                option = MessageService.Sort.Nickname;
            else
                option = MessageService.Sort.GroupNickTime;

            Printer(list);
            this.last20= ChatRoom.request20Messages();
            if (sortChanged)
            {
                ObservableCollection<IMessage> temp = new ObservableCollection<IMessage>();
                foreach (IMessage msg in bindObject.Messages)
                    temp.Add(msg);
                bindObject.Messages.Clear();
                Printer(ChatRoom.sort(temp, option, bindObject.SortDescending));
                sortChanged = false;
            }
            if (filterApplied)
            {
                ObservableCollection<IMessage> temp = new ObservableCollection<IMessage>();
                foreach (IMessage msg in bindObject.Messages)
                    temp.Add(msg);
                bindObject.Messages.Clear();
                if (bindObject.FilterUsername)
                    Printer(ChatRoom.requestMessagesfromUser(temp, bindObject.FilterNameString, int.Parse(bindObject.FilterGroupString)));
                else if (bindObject.FilterGroupid)
                    Printer(ChatRoom.requestMessagesfromGroup(temp, int.Parse(bindObject.FilterGroupString)));
                filterApplied = false;
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            sortChanged = true;
            UpdateScreen();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortChanged = true;
            UpdateScreen();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (bindObject.FilterNone)
            {
                filterApplied = false;
                UpdateScreen();
            }
            else
            {
                if (bindObject.FilterGroupString==""|bindObject.FilterGroupString==null)
                    return;
                if (bindObject.FilterUsername)
                    if (bindObject.FilterNameString == "" | bindObject.FilterNameString == null)
                        return;
                filterApplied = true;
                UpdateScreen();
            }
        }
        }
    }
