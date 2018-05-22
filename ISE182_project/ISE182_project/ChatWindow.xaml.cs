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

namespace ISE182_project
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private ObservableObject bindObject;
        public ChatWindow(ObservableObject fromMainWindows)
        {
            InitializeComponent();
            this.bindObject = fromMainWindows;
            this.DataContext = bindObject;
            bindObject.Username = "Username: " + ChatRoom.LoggedinUser.NickName;
            bindObject.GroupID = "GroupID: " + ChatRoom.LoggedinUser.Group_ID.ToString();
            Printer(ChatRoom.request20Messages());
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.send(bindObject.MessageContent);
            bindObject.MessageContent = "";
        }
        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.logout();
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
        }
        private void Printer<T>(ICollection<T> list)
        {
            foreach (object o in list)
            {
                bindObject.Messages.Add(o.ToString());
            }
        }
    }
}
