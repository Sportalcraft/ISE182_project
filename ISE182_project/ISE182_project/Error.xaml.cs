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
using ISE182_project.Layers.PresentationLayer;

namespace ISE182_project
{
    /// <summary>
    /// Interaction logic for Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        private ObservableObject bindObject;
        public Error(ObservableObject obs)
        {
            this.bindObject = obs;
            this.DataContext = bindObject;
            InitializeComponent();
        }
    }
}
