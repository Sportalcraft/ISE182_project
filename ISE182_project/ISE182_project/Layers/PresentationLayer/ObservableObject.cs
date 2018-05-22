using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ISE182_project.Layers.PresentationLayer
{
    public class ObservableObject : INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableObject()
        {
            Messages.CollectionChanged += Messages_CollectionChanged;
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
