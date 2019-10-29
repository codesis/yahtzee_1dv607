using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.View
{
    enum DisplayType {ViewFullScoreBord = 0, InspectSavedGame, ResumeSavedGame, ViewAvailableVariants }
    class RoundsView : DisplayView
    {
        private readonly string viewFullScoreBord = "\n Do you wish to see the general highscore (y) or the individual highscore (n) of the game (y/n)";
        private readonly string inspectSavedGame = "\n Do you wish to inspect a saved game (y/n)";
        private readonly string resumeSavedGame = "\n Do you wish to resume a saved game (y/n)";
        private readonly string viewAvailableVariants = "\n Do you wish to view available variants (y/n)";

        private Variant variant;

        public RoundsView(Variant variant)
        {
            this.variant = variant;
        }

        public void RenderRoundNumber(int roundNumber)
        {
            PrintMessage("\nRound number " + roundNumber);
        }

        public void RenderRound(string name)
        {
            PrintMessage("\n" + name + " time to play!");
        }

        public void RenderDice(int[] dice)
        {
            string idAndValueOutput = "";
            Console.WriteLine("");
            for (int i=1; i<=dice.Length;i++)
            {
                idAndValueOutput += "Dice number: " + i + "     Value: " + dice[i-1] + "\n";
            }
            Console.Write(idAndValueOutput);
        }

        public void RenderDiceToRoll(bool[] diceToRoll, string decision="")
        {
            bool stand = true;
            for (int i = 0; i < diceToRoll.Length; i++)
            {
                if (diceToRoll[i])
                    stand = false;
            }
            if (stand)
                Console.Write("\n" + decision + "\n");
            else
            {
                Console.Write("\n" + decision + " - Decided to reroll: ");
                for (int i = 0; i < diceToRoll.Length; i++)
                {
                    if (diceToRoll[i])
                    {
                        Console.Write(i + 1 + " ");
                    }
                }
                Console.WriteLine("");
            }
        }

        public bool[] GetDiceToRoll()
        {
            bool[] diceToRoll = { };
            bool getInput = true;
            int val;
            int index;

            while (getInput)
            {
                diceToRoll = new bool[] { false, false, false, false, false };
                Console.WriteLine("Select die to roll by entering the id numbers of your choosen die separated by a space e.g.(1 2 3 5), or (0) to stand");
                string input = Console.ReadLine();
                string[] diceNumbers = input.Split(' ');
                getInput = false;

                //Check if player stand
                if (Int32.TryParse(diceNumbers[0], out val) && val == 0)
                {
                    return diceToRoll;
                }

                for (int i = 0; i < diceNumbers.Length; i++)
                {
                    if (Int32.TryParse(diceNumbers[i], out index) && index >= 1 && index <= 5)
                    {
                        diceToRoll[index - 1] = true;
                    }
                    else
                    {
                        PrintErrorMessage("Invalid input");
                        getInput = true;
                        break;
                    }
                }
            }
            return diceToRoll;
        }

        public void RenderUnavailableVariants(List<Variant.Type> unavailableVariants)
        {
            RenderVariantList(unavailableVariants);
        }

        public Variant.Type RenderVariant(List<Variant.Type> unavailableVariants)
        {
            int enumLength = variant.Length();

            while (true)
            {
                int value = 0;
                PrintMessage("\nSelect green marked number Variant from this list e.g.(3): ");
                RenderVariantList(unavailableVariants);

                if (Int32.TryParse(Console.ReadLine(), out value) && value >= 1 && value < enumLength+1)
                {
                    bool exist = unavailableVariants.Contains(variant.GetVariant(value -1));

                    if (!exist)
                        return variant.GetVariant(value-1);
                }
                PrintErrorMessage("Invalid input");
            }
        }

        private void RenderVariantList(List<Variant.Type> unavailableVariants)
        {
            string output = "";
            foreach (Variant.Type vari in variant.GetValues())
            {

                bool exist = unavailableVariants.Contains(vari);

                if (exist)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                output = "(" + variant.GetValue(vari+ 1) + ") " + variant.GetName(vari);

                Console.WriteLine(output);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        
        public bool ContinueGame()
        {
            do
            {
                PrintMessage("\nContinue game (y/n)");
                
                string input = Console.ReadLine().ToLower();
                if (input.CompareTo("y") == 0)
                {
                    Console.Clear();
                    return true;
                }
                else if (input.CompareTo("n") == 0)
                {
                    Console.Clear();
                    return false;
                }
                PrintErrorMessage("Invalid input, answer with (y/n).");
            } while (true);
        }

        public bool SelectActivity(DisplayType displayType, bool ClearAtNo=true)
        {
            string message = "";
            
            switch (displayType)
            {
                case DisplayType.InspectSavedGame:
                    message = inspectSavedGame;
                    break;
                case DisplayType.ResumeSavedGame:
                    message = resumeSavedGame;
                    break;
                case DisplayType.ViewFullScoreBord:
                    message = viewFullScoreBord;
                    break;
                case DisplayType.ViewAvailableVariants:
                    message = viewAvailableVariants;
                    break;
                default:
                    break; 
            }
            do
            {
                PrintMessage(message);

                string input = Console.ReadLine().ToLower();

                if (input.CompareTo("y") == 0)
                {
                    return true;
                }
                else if (input.CompareTo("n") == 0)
                {
                    if (ClearAtNo)
                        Console.Clear();
                    return false;
                }

                Console.Clear();
                PrintErrorMessage("Invalid input, answer with (y/n).");

            } while (true);
        }

        public void GameSaved(string fileName)
        {
            PrintMessage("Game has been saved: " + fileName);
        }

        public void GameFinished(string winner, int score)
        {
            PrintMessage("*************************************************");
            PrintMessage(" The winner is " + winner + " at score " + score);
            PrintMessage("*************************************************");
        }

        public string SelectGame(FileInfo[] files)
        {
            Console.Clear();
            string selectedFile = ""; 

            PrintMessage("\nSelect file from list, enter number before selected file. \nPress ANY other key to return");
            
            for (int i = 0; i < files.Length; i++)
            {
                Console.WriteLine("(" + i + ") " + files[i].Name);
            }

            string input = Console.ReadLine();
            int index = 0;

            if (Int32.TryParse(input, out index) && (index >= 0) && (index < files.Length))
            {
                Console.Clear();
                PrintMessage("\nGame " + files[index].Name + " selected");
                selectedFile = files[index].Name;
            }

            return selectedFile;
        }
    }
}
