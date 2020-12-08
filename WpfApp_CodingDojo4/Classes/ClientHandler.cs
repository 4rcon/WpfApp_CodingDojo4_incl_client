using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_CodingDojo4.Classes
{
    class ClientHandler
    {
        private Socket socket;
        private byte[] _buffer = new byte[512];

        private Action<string> guiInformer;

        public ClientHandler(Socket socket, Action<string> guiInformer)
        {
            this.socket = socket;
            this.guiInformer = guiInformer;
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            while (Server.IsStarted)
            {
                int length;
                length = socket.Receive(_buffer);
                guiInformer(Encoding.UTF8.GetString(_buffer, 0, length));
            }
        }

        
    }
}
