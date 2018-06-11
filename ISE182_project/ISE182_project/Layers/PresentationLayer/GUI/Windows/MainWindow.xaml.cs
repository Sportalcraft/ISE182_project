using ISE182_project.Layers.BusinessLogic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableObject main = new ObservableObject();

        public MainWindow()
        {
            this.DataContext = main;     
            InitializeComponent();
        }

        private void userHandlerButton_Click(object sender, RoutedEventArgs e)
        {
            logister();
        }

        //Log in ore register
        private void logister()
        {
            try
            {
                int groupID = int.Parse(main.GroupidBox);

                if (!main.ToLogIn)
                    ChatRoom.register(main.UsernameBox, groupID);

                ChatRoom.login(main.UsernameBox, groupID);
                ChatWindow cw = new ChatWindow(this.main);
                cw.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                main.ErrorText = ex.Message;
                Error ePage = new Error(main);
                ePage.Show();
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.exit();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox box = sender as TextBox;
                main.GroupidBox = box.Text;
                logister();
            }
        }
    }
}
