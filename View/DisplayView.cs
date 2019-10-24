using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yahtzee_1dv607.View
{
    class DisplayView
    {
        public void PrintMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
        }
        public void PrintErrorMessage(string errorMessage)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(errorMessage);
            
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
