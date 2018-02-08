using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Server
{
   public class Logger : ILogger
    {
        private Object lockLog = new object();

        public void LogJoin(string notification)
        {
            lock (lockLog)
            {
                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    w.WriteLine(notification + "\r");
                }
            }
        }
        public void LogMessage(string message)
        {
            lock (lockLog)
            {
                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    w.WriteLine(message + "\r");
                }
            }
        }
        public void LogLeave(string notification)
        {
            lock (lockLog)
            {
                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    w.WriteLine(notification + "\r");
                }
            }
        }   
    }
}
