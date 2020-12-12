using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp_CodingDojo4.Classes
{
    class Server
    {
        public static bool IsStarted { get; set; }

        private Socket serverSocket;
        
        private List<ClientHandler> clients = new List<ClientHandler>();

        public Thread acceptingThread;

        public Action<string> guiInformerAction { get; set; }

        public Server(string ip, int port, Action<string> guiInformerAction)
        {
            this.guiInformerAction = guiInformerAction;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(ip), port));
            serverSocket.Listen(5);
            StartAccepting();
        }

        public void StartAccepting()
        {
            acceptingThread = new Thread(new ThreadStart(Accept));
            acceptingThread.IsBackground = true;
            acceptingThread.Start();
        }

        public void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                clients.Add(new ClientHandler(serverSocket.Accept(), this.guiInformerAction));
            }
        }

        public void StopAccepting()
        {
            serverSocket.Close(); //Socket schließen um neuanmeldungen zu schließen
            acceptingThread.Abort(); // Thread annahme abbrechen
            // Alle Verbundenen Clients schließen
            foreach (var userClient in clients)
            {
                userClient.Close();
            }
            //Entferne alle Clients aus der Liste
            clients.Clear();

        }
    }
}
