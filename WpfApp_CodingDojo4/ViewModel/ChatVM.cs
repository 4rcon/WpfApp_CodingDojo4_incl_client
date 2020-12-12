using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using WpfApp_CodingDojo4.Classes;

namespace WpfApp_CodingDojo4.ViewModel
{
    public class ChatVm : ViewModelBase
    {
        public int MessageCounter { get; set; }
        public ObservableCollection<string> ConnectedUsers { get; set; }
        public ObservableCollection<Message> Messages { get; set; }

        public ChatVm()
        {
            ConnectedUsers = new ObservableCollection<string>();
            Messages = new ObservableCollection<Message>();
        }
    }
}
