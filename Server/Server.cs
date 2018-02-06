using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Server
    {
        public static ServerClient client;
        public TcpListener server;
        public Dictionary<string, ServerClient> userList = new Dictionary<string, ServerClient>();
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.128"), 9999);
            server.Start();
        }
        public void Run()
        {
            ServerClient tempClient = AcceptClient();
            //StoreClient(userName, tempClient);
            string message = client.Receive();
            Respond(message);
        }
        private ServerClient AcceptClient()
        {
            TcpClient clientSocket = default(TcpClient);
            clientSocket = server.AcceptTcpClient();          
            Console.WriteLine("Connected");
            NetworkStream stream = clientSocket.GetStream();
            client = new ServerClient(stream, clientSocket);

            return client;
        }
        public void StoreClient(string userName, ServerClient tempClient)
        {
            userList.Add(userName, tempClient);
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
    }
}
