using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_CodingDojo4.Classes
{
    class Server
    {
        public static bool IsStarted { get; set; }

        private Socket serverSocket;

        private List<ClientHandler> clients = new List<ClientHandler>();

        public Action<string> informerAction { get; set; }

        public Server(Action<string> informerAction)
        {
            this.informerAction = informerAction;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, 10200));
            serverSocket.Listen(5);
            Task.Factory.StartNew(StartAccepting);
        }

        private void StartAccepting()
        {
            while (Server.IsStarted)
            {
                clients.Add(new ClientHandler(serverSocket.Accept(), informerAction));
            }
        }
    }
}
