using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_CodingDojo4.Classes
{
    public class Message
    {
        public string User { get; set; }

        public string MessageText { get; set; }

        public Message(string user, string messageText)
        {
            this.User = user;
            this.MessageText = messageText;
        }

        public override string ToString()
        {
            return User+": "+MessageText;
        }
    }
}
