using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp_CodingDojo4.Classes
{
    class ClientHandler
    {
        private readonly Socket _socket;
        private readonly byte[] _buffer = new byte[512];
        private readonly Thread _clientReceiveThread;
        private const string quitMessage = "@quit";

        private readonly Action<string> _message;

        public string Username { get; private set; }

        


        public ClientHandler(Socket socket, Action<string> message)
        {
            this._socket = socket;
            this._message = message;

            //Neuer Thread
            _clientReceiveThread = new Thread(Receive);
            _clientReceiveThread.Start();
        }

        private void Receive()
        {
            string msgR = "";
            while (!msgR.Contains(quitMessage))
            {
                if (Server.IsStarted)
                {
                    int length = _socket.Receive(_buffer);
                    msgR = Encoding.UTF8.GetString(_buffer, 0, length);
                    if (Username != null && msgR.Contains(":"))
                    {
                        Username = msgR.Split(':')[0];
                    }
                    //send that to gui
                    _message(msgR);
                }
            }
            Close();
        }

        public void Send(string message)
        {
            _socket.Send(Encoding.UTF8.GetBytes(message));
        }

        public void Close()
        {
            try
            {
                Send(quitMessage);
            }
            catch (Exception )
            {
                Console.WriteLine("Users not Listed anymore");
            }
            _socket.Close(1000);
            _clientReceiveThread.Abort();
        }
    }
}
