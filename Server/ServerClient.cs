using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ServerClient : Observer
    {
        NetworkStream stream;
        TcpClient client;
        Server server;
        public int UserId;
        public string userName;
        public ServerClient(NetworkStream Stream, TcpClient Client, Server Server)
        {
            stream = Stream;
            client = Client;
            UserId = 1;
            server = Server;
            GetUserName();
            Task.Run(() => ConstantReceive());
        }
        public void NotifyUser(string message)
        {
            Send(message);
        }
        public void Send(string Message)
        {
            byte[] message = Encoding.ASCII.GetBytes(Message);
            stream.Write(message, 0, message.Count());
        }
        public string Receive()
        {
            byte[] receivedMessage = new byte[256];
            stream.Read(receivedMessage, 0, receivedMessage.Length);
            string receivedMessageString = Encoding.ASCII.GetString(receivedMessage);
            Console.WriteLine(receivedMessageString);
            return receivedMessageString;
        }
        public string GetUserName()
        {
            Send("What is your screen name?");
            userName = Receive().Trim ('\0');
            return userName;
        }
        public void ConstantReceive()
        {
            while (true)
            {
                string incomingMessage = Receive().Trim('\0');
               
                server.AddToQueue(incomingMessage, this);
            }
        }


    }
}
