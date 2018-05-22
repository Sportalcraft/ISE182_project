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
        private string username;
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                    this.username = value;
                    OnPropertyChanged();
            }
        }
        private string groupID;
        public string GroupID
        {
            get
            {
                return this.groupID;
            }
            set
            {
                this.groupID = value;
                OnPropertyChanged();
            }
        }
        private string usernameBox;
        public string UsernameBox
        {
            get
            {
                return this.usernameBox;
            }
            set
            {
                this.usernameBox = value;
                OnPropertyChanged();
            }
        }
        private string groupidBox;
        public string GroupidBox
        {
            get
            {
                return this.groupidBox;
            }
            set
            {
                this.groupidBox = value;
                OnPropertyChanged();
            }
        }
        private bool filterUsername;
        public bool FilterUsername
        {
            get
            {
                return this.filterUsername;
            }
            set
            {
                    this.filterUsername = value;
                    OnPropertyChanged();
            }
        }
        private bool filterGroupid;
        public bool FilterGroupid
        {
            get
            {
                return this.filterGroupid;
            }
            set
            {
                    this.filterGroupid = value;
                    OnPropertyChanged();
            }
        }
        private bool filterNone;
        public bool FilterNone
        {
            get
            {
                return this.filterNone;
            }
            set
            {
                    this.filterNone = value;
                    OnPropertyChanged();
            }
        }
        private bool sortAscending;
        public bool SortAscending
        {
            get
            {
                return this.sortAscending;
            }
            set
            {
                    this.sortAscending = value;
                    OnPropertyChanged();
            }
        }
        private bool sortDescending;
        public bool SortDescending
        {
            get
            {
                return this.sortDescending;
            }
            set
            {
                    this.sortDescending = value;
                    OnPropertyChanged();
            }
        }
    }
}
