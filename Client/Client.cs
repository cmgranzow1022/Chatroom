using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        TcpClient clientSocket;
        NetworkStream stream;
        public Client(string IP, int port)
        {
            clientSocket = new TcpClient();
            clientSocket.Connect(IPAddress.Parse(IP), port);
            stream = clientSocket.GetStream();
        }
        public void Send()
        {
            while (true)
            {
                string messageString = UI.GetInput();
                byte[] message = Encoding.ASCII.GetBytes(messageString);
                stream.Write(message, 0, message.Count());
            }
        }
        public void Receive()
        {
            while (true)
            {
                byte[] receivedMessage = new byte[256];
                stream.Read(receivedMessage, 0, receivedMessage.Length);
                UI.DisplayMessage(Encoding.ASCII.GetString(receivedMessage));
        }
    }
        public void Start()
        {
            Task.Run(() => Send());
            Task value = Task.Run(() => Receive());
            value.Wait();
        }
    }
}
