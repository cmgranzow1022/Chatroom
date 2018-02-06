using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //new Server().Run();
            Server chad = new Server();
            for (int x = 1; x < 10; x++)
            {
                chad.Run();
            }
            Console.ReadLine();

        }
    }
}
