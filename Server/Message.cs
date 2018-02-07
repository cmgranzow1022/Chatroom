using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Message
    {
        public ServerClient sender;
        public string Body;
        public string userName;
        public Message(ServerClient Sender, string Body)
        {
            sender = Sender;
            this.Body = Body;
            userName = sender?.userName;
        }
    }
}
