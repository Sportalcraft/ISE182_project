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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.PresentationLayer;
using ISE182_project.Layers;

namespace ISE182_project
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

            ChatRoom.start(ChatRoom.Place.University);
            InitializeComponent();
        }
        private void userHandlerButton_Click(object sender, RoutedEventArgs e)
        {
            int groupID = int.Parse(main.GroupidBox);
            if (loginRadio.IsChecked == true)
                ChatRoom.login(main.UsernameBox, groupID);
            else
                ChatRoom.register(main.UsernameBox, groupID);

            ChatWindow cw = new ChatWindow(this.main);
            cw.Show();
            this.Hide();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            ChatRoom.exit();
        }
    }
}
