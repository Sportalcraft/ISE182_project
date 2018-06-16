using ISE182_project.Layers.BusinessLogic;
using ISE182_project.Layers.LoggingLayer;
using ISE182_project.Layers.PresentationLayer.GUI.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ISE182_project
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() : base()
        {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        //global exeption handler
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string ErrorText = e.Exception.Message;
            Logger.Log.Fatal(Logger.Maintenance("Unhandled Exception accured : \n" + ErrorText));

            Error ePage = new Error(ErrorText);
            ePage.Show();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            ChatRoom.start();
        }
    }
}
