using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp_Client.Classes
{
    class Handler
    {
       
        public Socket ClientSocket { get; private set; }  
        private byte[] _buffer = new byte[512];
        private Action<string> MessageInformer;
        public Action AbortInformer;


        public Handler(int port, Action<string> messageInformer, Action abortInformer)
        {
            try
            {
                this.AbortInformer = abortInformer;
                this.MessageInformer = messageInformer;
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Loopback, port);
                ClientSocket = client.Client;
                StartReceiving();
            }
            catch (Exception)
            {
                MessageInformer("Server not Reachable. Please call your Hotline.");
                AbortInformer(); //Reset Client Connection Communication
            }
            
        }

        private void StartReceiving()
        {
            //Start receiving in a new Thread
            Task.Factory.StartNew(MessageReceiver);
        }

        public void SendMessage(string message)
        {
            ClientSocket.Send(Encoding.UTF8.GetBytes(message));
        }


        public void MessageReceiver()
        {
            string message = "";
            while (!message.Contains("@quit"))
            {
                int length = ClientSocket.Receive(_buffer);
                message = Encoding.UTF8.GetString(_buffer,0,length); //bei Receive = getString
                MessageInformer(message); // Übergabe der Message als delegate
            }
            Close();
        }

        private void Close()
        {
            ClientSocket.Close();
            MessageInformer("Haha you got kicked from the Server. No reason given.");
            AbortInformer();
        }
    }
}
