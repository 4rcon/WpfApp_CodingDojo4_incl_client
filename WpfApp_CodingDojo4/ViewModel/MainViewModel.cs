using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApp_CodingDojo4.Classes;
using WpfApp_CodingDojo4.ViewModel;

namespace WpfApp_CodingDojo4.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private Server server;
        private const int port = 10200;
        private const string ipadress = "127.0.0.1";
        private bool isConnected = false;
        public ObservableCollection<string> Messages { get; set; }
        public ObservableCollection<string> Users { get; set; }
        public ObservableCollection<string> LogMessages { get; set; }


        #region Start Server Button
        private RelayCommand _startConCmd;
        public RelayCommand StartConnCommand
        {
            get
            {
                return _startConCmd
                    ?? (_startConCmd = new RelayCommand(
                    () =>
                    {
                        server = new Server(ipadress, port, DisplayNewMessages);
                        server.StartAccepting();
                        isConnected = true;
                    },
                    () =>
                    {
                        if (isConnected = false)
                            return true;
                        return false;
                    }));
            }
        }
        #endregion

        #region Stop Server Button
        private RelayCommand _stopConCmd;
        public RelayCommand StopConnCommand
        {
            get
            {
                return _startConCmd
                       ?? (_startConCmd = new RelayCommand(
                           () =>
                           {
                               server.StopAccepting();
                               isConnected = false;
                           },
                           () =>
                           {
                               if (isConnected = true)
                                   return true;
                               return false;
                           }));
            }
        }
        #endregion



        private void DisplayNewMessages(string message)
        {
            //todo Write GUI Updater to display new messages
        }

        public MainViewModel()
        {
            Messages = new ObservableCollection<string>();
            Users = new ObservableCollection<string>();
            LogMessages = new ObservableCollection<string>();


        }

    }
}