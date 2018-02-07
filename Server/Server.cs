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
        public Dictionary<int, ServerClient> userDictionary = new Dictionary<int, ServerClient>();
        public int userIdCounter;
        public ServerClient tempClient;
        private Queue<Message> messages;
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.128"), 9999);
            userIdCounter = 0; 
            server.Start();

        }
        public void Run()
        {
            Task.Run(() => AcceptClient());
            //string message = client.Receive();
            //Respond(message);
        }
        private void AcceptClient()
        {
            while (true)
            {
                TcpClient clientSocket = default(TcpClient);
                clientSocket = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                NetworkStream stream = clientSocket.GetStream();
                client = new ServerClient(stream, clientSocket);
                AddClientToDictionary(client);
                client.GetUserName();
                NewClientNotifications(client);
            }
        }
        public void AddClientToDictionary(ServerClient client)
        {
            userDictionary.Add(userIdCounter, client);
            client.UserId = userIdCounter;
            userIdCounter++;
        }
        private void Respond(string body)
        {
             client.Send(body);
        }
        public void NewClientNotifications(ServerClient client)
        {
            string notification = client.userName + " has joined the chatroom.";
            //add to queue
        }
        public void AddToQueue(string message, ServerClient client)
        {
            Message currentMessage = new Message(client, message);
            messages.Enqueue(currentMessage);
        }

    }
}
