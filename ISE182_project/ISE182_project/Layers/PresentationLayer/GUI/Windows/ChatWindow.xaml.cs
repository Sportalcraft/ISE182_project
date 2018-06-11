﻿using ISE182_project.Layers.BusinessLogic;
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
        private ObservableObject bindObject;
        private DispatcherTimer dispatcherTimer;
        private bool sortChanged;
        private bool filterApplied;
        private bool reloadChat;

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

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ChatRoom.DrawLastMessages();
            UpdateScreen();
        }


        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ChatRoom.send(bindObject.MessageContent);
                UpdateScreen();
                bindObject.MessageContent = "";
            }
            catch (Exception ex)
            {
                bindObject.ErrorText = ex.Message;
                Error ePage = new Error(bindObject);
                ePage.Show();
            }
        }
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
        private void Printer(ICollection<IMessage> list)
        {
            foreach (IMessage msg in list)
            {
                bindObject.Messages.Add(msg);
            }
        }

        //Update the messages displayed on screen
        private void UpdateScreen()
        {
            ICollection<IMessage> list = ChatRoom.getMessages();
            bindObject.Messages.Clear();

            foreach (IMessage m in list)
                bindObject.Messages.Add(m);           
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
            if (bindObject.FilterNone)
                reloadChat = true;
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if (bindObject.FilterNone)
            {
                UpdateScreen();
            }
            else
            {
                if (bindObject.FilterGroupString == "" | bindObject.FilterGroupString == null)
                    return;
                if (bindObject.FilterUsername)
                    if (bindObject.FilterNameString == "" | bindObject.FilterNameString == null)
                        return;
                filterApplied = true;
                UpdateScreen();
            }
        }

        private void TextBox_KeyDownSendMessage(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox box = sender as TextBox;

                try
                {
                    ChatRoom.send(box.Text);
                    box.Text = "";
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
        }
    }
}