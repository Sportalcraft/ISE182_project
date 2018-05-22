using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace ISE182_project.Layers.PresentationLayer
{
    public class ObservableObject : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableObject()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;

            DispatcherTimer dispatcherTimer = new DispatcherTimer(); 
            dispatcherTimer.Tick += dispatcherTimer_Tick;         // add event
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 2);  // set time between ticks
            //dispatcherTimer.Start();                              // start the timer        
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            messageContent = "Banana";
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("Messages");
        }

        private string messageContent = "";
        public string MessageContent
        {
            get
            {
                return messageContent;
            }
            set
            {
                messageContent = value;
                OnPropertyChanged("MessageContent");
            }
        }
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<string> Messages { get; } = new ObservableCollection<string>();

        private bool mainWindowLoginRadio;
        public bool MainWindowLoginRadio
        {
            get
            {
                return this.mainWindowLoginRadio;
            }
            set
            {
                if(!this.mainWindowLoginRadio)
                {
                    this.mainWindowLoginRadio = value;
                    OnPropertyChanged();
                }
            }
        }
        private string _Username;
        public string Username
        {
            get
            {
                return this._Username;
            }
            set
            {
                    this._Username = value;
                    OnPropertyChanged();
            }
        }
        private string _GroupID;
        public string GroupID
        {
            get
            {
                return this._GroupID;
            }
            set
            {
                this._GroupID = value;
                OnPropertyChanged();
            }
        }


    }
}
