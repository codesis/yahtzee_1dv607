using System;

namespace yahtzee_1dv607.View
{
    class DisplayView
    {
        public void PrintMessage(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.WriteLine(message);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Magenta;
        }
        
        public void PrintErrorMessage(string errorMessage)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.WriteLine(errorMessage);
            
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
