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
        public Queue<Message> messages; 
        private Object messageLock = new object();
        public Server()
        {
            server = new TcpListener(IPAddress.Parse("192.168.0.128"), 9999);
            userIdCounter = 0; 
            server.Start();
            messages = new Queue<Message>();
        }
        public void Run()
        {
            Task.Run(() => AcceptClient());
            Task.Run(() => PostToChatroom());

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

                client.GetUserName();
                AddClientToDictionary(client);
                NewClientNotification(client);
                Task.Run(() => OpenNewChat(client));
            }
        }
        public void OpenNewChat(ServerClient client)
        {
            while (true)
            {
                client.Receive();
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
        public void NewClientNotification(ServerClient client)
        {
            string notification = client.userName + " has joined the chatroom.";
            AddToQueue(notification, client);
        }
        public void AddToQueue(string message, ServerClient client)
        {
            Message currentMessage = new Message(client, message);
            messages.Enqueue(currentMessage);
        }
        private Message RemoveFromQueue()
        {
            return messages.Dequeue();
        }
        public void PostToChatroom()
            {
            while (true)
            {
                if (messages != null)
                {
                    if (messages.Count > 0)
                    {
                        Message message = RemoveFromQueue();
                        lock (messageLock)
                        {
                            foreach (KeyValuePair<int, ServerClient> clients in userDictionary)
                            {
                                clients.Value.Send(message.Body);

                            }
                        }
                    }
                }
            }
            }
    }
}
