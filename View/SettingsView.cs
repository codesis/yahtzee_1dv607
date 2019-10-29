using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;


namespace yahtzee_1dv607.View
{
    class SettingsView : DisplayView
    {
        public int NumberOfPlayers()
        {
            int value = 0;
            Console.WriteLine("\n Choose how many players, 1-5: ");

            while (true)
            {
                if (Int32.TryParse(Console.ReadLine(), out value) && value <= 5 && value >= 1)
                {
                    return value;
                } 
                
                Console.Clear();
                Console.WriteLine("Invalid input value, you need to give a value between 1 and 5.");
            }
        }
        public string PlayerName(int number)
        {
            Console.Clear();

            do{
                Console.WriteLine("Player number " + number + ": Player please choose an alias, 3-8 characters: ");
                string input = Console.ReadLine().ToLower();

                if (input.Length <= 8 && input.Length >= 3)
                {
                    return input;
                }

                Console.Clear();
                PrintErrorMessage("Invalid input.");

            } 
            while (true);
        }
        public bool IsAi(int number)
        {
            do
            {
                Console.WriteLine(" Player number " + number + ": Is this player played by the computer (AI)? (y/n)");
                string input = Console.ReadLine().ToLower();

                if (input.CompareTo("y") == 0)
                {
                    PrintMessage("An AI player created successfully");
                    Thread.Sleep(1000);
                    return true;
                }
                else if (input.CompareTo("n") == 0)
                {
                    return false;
                }

                PrintErrorMessage("Invalid input, answer with (y/n).");
            } 
            while (true);
        }
    }
}
