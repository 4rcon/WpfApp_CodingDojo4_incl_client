using System.Security.Cryptography.X509Certificates;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WpfApp_CodingDojo4.Classes;

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
        public RelayCommand StartServerBtnClicked { get; set; }
        public RelayCommand StopServerBtnClicked { get; set; }


        public MainViewModel()
        {
            StartServerBtnClicked = new RelayCommand(StartServer);
            StopServerBtnClicked = new RelayCommand(StopServer);


            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
            }
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