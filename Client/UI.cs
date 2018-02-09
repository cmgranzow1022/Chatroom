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
          int top = Console.CursorTop;
          int left = Console.CursorLeft;
            message = message.Trim('\0');
            if (top == Console.BufferHeight - 1)
            {
            Console.WriteLine();
            Console.SetCursorPosition(0, --top);
            }
            Console.MoveBufferArea(0, top, Console.WindowWidth, 1, 0, top + 1);
                Console.SetCursorPosition(0, top);
              Console.WriteLine(message);
            Console.SetCursorPosition(left, top + 1);
        }
        public static string GetInput()
        {
            return Console.ReadLine();
        }
    }
}


