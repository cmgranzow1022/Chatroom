using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class UI
    {
        public static void DisplayMessage(string message)
        {
            message = message.Trim('\0');
            Console.WriteLine(message + "\r");

        }
        public static string GetInput()
        {
            return Console.ReadLine();
        }
    }
}
