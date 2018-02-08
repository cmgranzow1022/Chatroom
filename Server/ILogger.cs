using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
     public interface ILogger
    {
        void LogJoin(string notification);

        void LogMessage(string message);

        void LogLeave(string notification);

        void ServerClosed();
    }
}
