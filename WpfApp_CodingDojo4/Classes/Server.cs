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
        
        private List<ClientHandler> clientsHandlersLists = new List<ClientHandler>();

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
                    clientsHandlersLists?.Add(new ClientHandler(serverSocket.Accept(), new Action<string>(BroadcastAndGuiInform)));
                    
                }
                catch (Exception)
                {
                    
                }
                
            }
        }

        public void StopAccepting()
        {
            // Alle Verbundenen Clients schließen
            foreach (var userClient in clientsHandlersLists)
            {
                userClient.Close();
            }

            try
            {
                serverSocket.Close(1000); //Socket schließen um neuanmeldungen zu schließen
                acceptingThread.Abort(); // Thread annahme abbrechen
                //Entferne alle Clients aus der Liste
                clientsHandlersLists.Clear();

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

        public void BroadcastAndGuiInform(string message)
        {
            foreach (ClientHandler client in clientsHandlersLists)
            {
                client.Send(message);
            }

            guiInformerAction(message);
        }

        public void DiscSelUser(string username)
        {
            foreach (ClientHandler clientHandler in clientsHandlersLists)
            {
                if (username.Equals(clientHandler.Username))
                {
                    clientHandler.Close();
                    clientsHandlersLists.Remove(clientHandler); 
                    break;
                }
            }
        }
    }
}
