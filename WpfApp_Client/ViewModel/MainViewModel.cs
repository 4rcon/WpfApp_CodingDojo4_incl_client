using System;
using System.Collections.ObjectModel;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApp_Client.Classes;

namespace WpfApp_Client.ViewModel
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
        private Handler client;

        #region boolean for Connection

        public const string IsConnectedPropertyName = "IsConnected";

        private bool _isConnected = false;

        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }

            set
            {
                if (_isConnected == value)
                {
                    return;
                }

                _isConnected = value;
                RaisePropertyChanged(IsConnectedPropertyName);
            }
        }
        #endregion

        #region ConnectBtnCmd

        private RelayCommand _connectButtonCommand;

        public RelayCommand ConnectBtnCmd
        {
            get
            {
                return _connectButtonCommand
                    ?? (_connectButtonCommand = new RelayCommand(
                    () =>
                    {
                        //todo: What to do when hitting "Connect" button.

                        IsConnected = true;
                        Connect();
                        return;
                    },
                    () =>
                    {
                        if (IsConnected == false && MyUsername != null && !MyUsername.Equals(String.Empty))
                        {
                            return true;
                        }
                        return false;
                    }));
            }
        }
        #endregion

        #region Username Property and Textfield
        public const string MyUsernamePropertyName = "MyUsername";

        private string _myUsername = String.Empty;

        public string MyUsername
        {
            get
            {
                return _myUsername;
            }

            set
            {
                if (_myUsername == value)
                {
                    return;
                }

                _myUsername = value;
                RaisePropertyChanged(MyUsernamePropertyName);
            }
        }
        #endregion

        #region Send Message Textfield
        public const string SendMsgPropPropertyName = "SendMsgProp";

        private string _sendMsgProperty = "";

        public string SendMsgProp
        {
            get
            {
                return _sendMsgProperty;
            }

            set
            {
                if (_sendMsgProperty == value)
                {
                    return;
                }

                _sendMsgProperty = value;
                RaisePropertyChanged(SendMsgPropPropertyName);
            }
        }
        #endregion
        
        #region Button f�rs verschicken

        private RelayCommand _sendMsgBtnCmd;

        public RelayCommand SendMessageBtnCmd
        {
            get
            {
                return _sendMsgBtnCmd
                    ?? (_sendMsgBtnCmd = new RelayCommand(
                    () =>
                    {
                        //todo: add what to do with the message

                        SendMsgProp = String.Empty;
                    },
                    () =>
                    {
                        if (IsConnected == true && SendMsgProp != null && !SendMsgProp.Equals(String.Empty))
                        {
                            return true;
                        }
                        return false;
                    }));
            }
        }
        #endregion

        public ObservableCollection<string> ChatlogObCol { get; set; } = new ObservableCollection<string>();
        


        public MainViewModel()
        {
            


            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
        }


        private void Connect()
        {
            MessageReceived("Connecting to Chat,...");
            Thread connectThread = new Thread(() =>
            {
                try
                {
                    client = new Handler(10200, new Action<string>(MessageReceived), new Action(Disconnect));
                }
                catch (Exception)
                {
                    MessageReceived("Cannot connect to Chat. Please try again.");
                }
            });
            connectThread.Start();
        }

        private void Disconnect()
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MessageReceived("Disconnected from Chat!");
                IsConnected = false;
                ConnectBtnCmd.RaiseCanExecuteChanged();
            });
        }

        private void MessageReceived(string msgR)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                ChatlogObCol.Add(msgR);
            });

        }
    }
}