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
            IsStarted = true;
        }

        public void Accept()
        {
            while (acceptingThread.IsAlive)
            {
                try
                {
                    clients?.Add(new ClientHandler(serverSocket.Accept(), this.guiInformerAction));
                }
                catch (Exception)
                {
                    
                }
                
            }
        }

        public void StopAccepting()
        {
            // Alle Verbundenen Clients schließen
            foreach (var userClient in clients)
            {
                userClient.Close();
            }

            try
            {
                serverSocket.Close(1000); //Socket schließen um neuanmeldungen zu schließen
                acceptingThread.Abort(); // Thread annahme abbrechen
                //Entferne alle Clients aus der Liste
                clients.Clear();

            }
            catch (Exception)
            {
                Console.WriteLine("Server Crashed!");
            }
            finally
            {
                IsStarted = false;
            }

        }

        public void Broadcast(string message)
        {
            foreach (ClientHandler client in clients)
            {
                client.Send(message);
            }
        }

        public void DiscSelUser(string username)
        {
            foreach (var item in clients)
            {
                if (item.Username.Equals(username))
                {
                    item.Close();
                    clients.Remove(item); 
                    break;
                }
            }
        }
    }
}
