using System.Collections.ObjectModel;
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
        private string currentSelectedUser;

        public string CurrentSelectedUser
        {
            get { return currentSelectedUser; }
            set { currentSelectedUser = value; }
        }


        public RelayCommand StartServerBtnClicked { get; set; }
        public RelayCommand StopServerBtnClicked { get; set; }
        public RelayCommand KickUserBtnClicked { get; set; }
        public ObservableCollection<ChatVm> Chat { get; set; }




        public MainViewModel()
        {
            StartServerBtnClicked = new RelayCommand(StartServer);
            StopServerBtnClicked = new RelayCommand(StopServer);
            KickUserBtnClicked = new RelayCommand(() =>
            {
                Chat[0].ConnectedUsers.Remove(CurrentSelectedUser);
                //TODO Disconnect selected User
                RaisePropertyChanged();
            });

            
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
                GenerateTestUserData();
            }
            else
            {
                // Code runs "for real"
                GenerateTestUserData();
            }
        }


        private void GenerateTestUserData()
        {
            Chat = new ObservableCollection<ChatVm>();
            Chat.Add(new ChatVm());
            Chat[0].ConnectedUsers.Add("Nigglas");
            string connUser = Chat[0].ConnectedUsers[0];
            Chat[0].Messages.Add(new Message(connUser,"Man schreibt mich mit double-G"));
        }

        public void StartServer()
        {
            Server.IsStarted = true;
        }

        public void StopServer()
        {
            Server.IsStarted = false;
        }
    }
}