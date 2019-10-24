using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yahtzee_1dv607.Model.Rules;

namespace yahtzee_1dv607.View
{
    class MainMenu : DisplayView
    {
        private readonly string welcome = "\t\t\t Welcome to ";
        public GameType RenderStartMenu()
        {
            GameType gameType;
            int index;
            Console.Clear();
            while (true)
            {
                Console.WriteLine("\n\t Which type of Yahtzy do you want to play?\n");
                Console.WriteLine("\t - Press 1 for Yahtzee");
                Console.WriteLine("\t - Press 2 for Yatzy");

                string input = Console.ReadLine();

                if (Int32.TryParse(input, out index) && index >= 1 && index <= 2)
                {

                    if (index == 1)
                        gameType = GameType.Yahtzee;
                    else
                        gameType = GameType.Yatzy;
                    break;
                }
                
                else
                {
                    Console.Clear();
                    PrintErrorMessage("\t Invalid input, try again");
                }
            }

            Welcome(gameType);
            return gameType;

        }

        public void Welcome(GameType gameType)
        {
            Console.Clear();
            Console.WriteLine(welcome + gameType.ToString());
        }
    }
}
