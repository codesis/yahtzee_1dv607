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
        private readonly string welcome = "Welcome to ";
        public GameType RenderStartMenu()
        {
            GameType gameType;
            int index;
            Console.Clear();
            
            while (true)
            {
                Console.WriteLine("\n Choose your type of game!\n");
                Console.WriteLine("- Press 1 for Yahtzee");
                Console.WriteLine("- Press 2 for Yatzy");

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
                    PrintErrorMessage("Invalid input, try again");
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
