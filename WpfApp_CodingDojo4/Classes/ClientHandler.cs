using System;
using System.Collections.Generic;
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
        private Socket socket;
        private byte[] _buffer = new byte[512];
        private Thread clientReceiveThread;
        private const string quitMessage = "@quit";

        private readonly Action<string> message;

        public string Username { get; private set; }

        


        public ClientHandler(Socket socket, Action<string> message)
        {
            this.socket = socket;
            this.message = message;

            //Neuer Thread
            clientReceiveThread = new Thread(Receive);
            clientReceiveThread.Start();
        }

        private void Receive()
        {
            string msgR = "";
            while (!msgR.Contains(quitMessage))
            {
                while (Server.IsStarted)
                {
                    int length = socket.Receive(_buffer);
                    msgR = Encoding.UTF8.GetString(_buffer, 0, length);
                    if (Username != null && msgR.Contains(":"))
                    {
                        Username = msgR.Split(':')[0];
                    }
                    //send that to gui
                    message(msgR);
                }
            }
            Close();
        }

        public void Send(string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }

        public void Close()
        {
             Send(quitMessage);
             socket.Close();
             clientReceiveThread.Abort();
        }
    }
}
