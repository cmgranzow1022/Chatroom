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
    class Server : Observed
    {
        public static ServerClient client;
        public TcpListener server;
        public Dictionary<int, ServerClient> userDictionary = new Dictionary<int, ServerClient>();
        public int userIdCounter;
        public Queue<Message> messages; 
        private Object messageLock = new object();
        ILogger logger;

        public Server(ILogger logger)
        {
            server = new TcpListener(IPAddress.Any, 9999);
            userIdCounter = 0; 
            server.Start();
            messages = new Queue<Message>();
            this.logger = logger;
        }
        public void Run()
        {
            Task.Run(() => AcceptClient());

            Task.Run(() => PostToChatroom());
            Task.Run(() => CheckCurrentUsers());
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
                client = new ServerClient(stream, clientSocket, this);
                AddClientToDictionary(client);
                NewClientNotification(client);
            }
        }
        public void OpenNewChat(ServerClient client)
        {
            while (true)
            {
              string incomingMessage =  client.Receive();
                AddToQueue(incomingMessage, client);
            }
        }
        public void AddClientToDictionary(ServerClient client)
        {
            userDictionary.Add(userIdCounter, client);
            client.UserId = userIdCounter;
            userIdCounter++;
        }
        public void NewClientNotification(ServerClient client)
        {
            string notification = client.userName + " has joined the chatroom.";
            Message messNotification = new Message(client, notification);
            logger.LogJoin(notification);
            lock (messageLock)
            {
                foreach (KeyValuePair<int, ServerClient> clients in userDictionary)
                {
                    clients.Value.Send(messNotification.Body);

                }
            }
        }
        public void ClientLeftNotification(ServerClient client)
        {
            string notification = client.userName + " has left the chatroom.";
            logger.LogLeave(notification);
        }
        public void CheckCurrentUsers()
        {
            foreach (KeyValuePair<int, ServerClient> clients in userDictionary)
            {
                if (client == null)
                {
                    userDictionary.Remove(client.UserId);
                }
            }
        }
        public void AddToQueue(string message, ServerClient client)
        {
            Message currentMessage = new Message(client, message);
            currentMessage.Body = currentMessage.userName + ": " + currentMessage.Body.Trim('\0');
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
                        logger.LogMessage(message.Body);
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
