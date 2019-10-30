using System;
using System.Collections.Generic;
using System.Linq;

using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;

namespace yahtzee_1dv607.View
{
    class ScoreView : DisplayView
    {
        private Variant variant;
        public ScoreView(Variant variant)
        {
            this.variant = variant;
        }
        public void RenderScoreOfRound(int roundScore, Variant.Type chosenVariant)
        {
            PrintMessage("Received " + roundScore + " points for type " + variant.GetName(chosenVariant) + "\n");
        }
        public void RenderHighscore(List<Player> players, string date, bool fullList)
        {
            if (date != null)
                Console.WriteLine("\nGame played " + date);

            string divider = "|";
            string end = "";
            Console.ForegroundColor = ConsoleColor.Green;

            for (int i = 0; i < players.Count() + 2; i++)
            {
                divider += "--------";
                end += "********";
            }

            divider += "|";
            Console.WriteLine("\nHIGHSCORE");
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("|"+end+"|");
            Console.Write("|\t\t");

            foreach (Player player in players)
            {
                Console.Write(player.Name + "\t");
            }
            Console.WriteLine(" |\n" + divider);

            if (fullList)
            {
                foreach (Variant.Type vari in variant.GetList())
                {
                    string name = String.Format("|{0,-14}\t", variant.GetName((int)vari));
                    Console.Write(name);

                    foreach (Player player in players)
                    {
                        bool exist;
                        Console.Write(player.GetScoreFromList(vari, out exist) + "\t");
                    }
                    Console.WriteLine(" |\n" + divider);

                }
                Console.Write("|");
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Sum\t\t");

            foreach (Player player in players)
            {
                Console.Write(player.GetTotalScore() + "\t");
            }
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(" |\n^" + end+"^");
            Console.ResetColor();
        }
    }
}
