using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.Model.Factory;
using yahtzee_1dv607.View;

namespace yahtzee_1dv607.Controller
{
    class GameController
    {
        private Database database;
        private List<Player> players;
        private GameManufactory manufactory;
        private InterfaceRules rules;
        private DiceCollection diceCollection;
        private ViewController viewController;
        private Variant variant;
        private GameType gameType;

        private DateTime Date { get; set; }
        private int RoundNumber { get; set; }

        public GameController()
        {
            InitGame();
            RunGame();
        }
        
        private bool[] DiceToRoll { get; set; }

        private void InitGame()
        {
            gameType = new MainMenu().RenderStartMenu();

            diceCollection = new DiceCollection();
            manufactory = new GameManufactory(gameType);
            rules = manufactory.GetRules(diceCollection);
            variant = manufactory.GetVariant();
            database = new Database(variant, rules, gameType);
            viewController = new ViewController(variant, diceCollection);

            while (viewController.ViewGameResult())
            {
                FileInfo[] files = database.ListSavedGames();
                string viewGameFile = viewController.SelectGame(files);
                if (viewGameFile != "")
                {
                    ViewGameFile(viewGameFile);
                }
            }
            if (viewController.ResumeGame())
            {
                FileInfo[] files = database.ListSavedGames();
                string resumeGameFile = viewController.SelectGame(files);
                if (resumeGameFile != "")
                {
                    ResumeGameFile(resumeGameFile);
                }
            }
            else
            {
                Date = DateTime.Now;
                RoundNumber = 0;
                PlayerSetup();
            }
        }

        private void ViewGameFile(string viewGameFile)
        {
            List<string> items = new List<string>();
            bool fullList = viewController.ViewFullList();
            DateTime date = new DateTime();
            int roundNumber = 0;
            players = database.GetPlayersFromFile(rules, viewGameFile, out date, out roundNumber);

            Date = date;
            RoundNumber = roundNumber;
            viewController.RenderScoreBoard(players, date.ToString(), fullList);
        }
        private void ResumeGameFile(string resumeGameFile)
        {
            DateTime date = new DateTime();
            int roundNumber = 0;
            players = database.GetPlayersFromFile(rules, resumeGameFile, out date, out roundNumber);
            Date = date;
            RoundNumber = roundNumber;
        }

        private bool AnyDiceToRoll()
        {
            bool roll = false;
            for (int i=0; i< DiceToRoll.Length;i++)
            {
                if (DiceToRoll[i])
                    roll = true;
            }
            return roll;
        }

        private int GetNumberOfAis()
        {
            int numberOfAis = 0;
            foreach (Player player in players)
            {
                if (player.IsAI)
                {
                    numberOfAis++;
                }
            }
            return numberOfAis;
        }
                private void PlayerSetup()
        {
            bool ai;
            players = new List<Player>();
            int numberOfPlayers = viewController.NumberOfPlayers();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                string name = viewController.PlayerName(i, out ai);

                if (ai)
                {
                    players.Add(new Ai(GetNumberOfAis() + 1, rules, variant, gameType));
                }
                else
                {
                    players.Add(new Player(name));
                }
            }
        }

        private void RunGame()
        {
            string fileName = "";
            int startRound = RoundNumber+1;

            for (int i = startRound; i <= variant.Length(); i++)
            {
                if (i != startRound && !viewController.ContinueGame())
                {
                    fileName = database.SaveGameToFile(Date, RoundNumber, players);
                    viewController.GameSaved(fileName);
                    return;
                }

                RunRound(i);
                RoundNumber++;
            }
            EndGame();
        }

        private void RunRound(int roundNumber)
        {
            viewController.RenderRoundNumber(roundNumber);
            foreach (Player player in players)
            {
                DiceToRoll = new bool[] { true, true, true, true, true };
                PlayRound(player);
            }
            viewController.RenderScoreBoard(players);
        }
        private void PlayRound(Player player)
        {
            Ai ai = player as Ai;
            Variant.Type variantToUse = variant.Chance();

            viewController.RenderRound(player.Name);

            for (int rollNumber = 1; rollNumber <= 3; rollNumber++)
            {
                if (AnyDiceToRoll())
                {
                    diceCollection.Roll(DiceToRoll);
                    if (rollNumber < 3)
                    {
                        if (player.IsAI)
                        {
                            DiceToRoll = ai.DecideDiceToRoll(diceCollection.GetNumberOfDiceFaceValue(), diceCollection.GetDice());
                        }
                        else
                        {
                            if (rollNumber == 1)
                                
                            viewController.RenderUnavailableVariants(player.GetChosenVariants(variant));
                            DiceToRoll = viewController.GetDiceToRoll();
                        }
                        viewController.RenderDiceToRoll(DiceToRoll, player.Decision);
                    }
                }
            }
            if (player.IsAI)
            {
                variantToUse = ai.SelectVariantToUse();
            }
            else
            {
                variantToUse = viewController.RenderVariant(player.GetChosenVariants(variant));
            }
            player.AddScoreToList(variantToUse, rules.GetValueByVariant(variantToUse));

            bool exist = false;
            int roundScore = player.GetScoreFromList(variantToUse, out exist);

            if (exist)
            {
                viewController.RenderRoundScore(roundScore, variantToUse);
            }
        }

        private void EndGame()
        {
            string fileName = database.SaveGameToFile(Date, RoundNumber, players);
            int highScore = 0;
            string winner = "";
            foreach (Player player in players)
            {
                if (player.GetTotalScore() == highScore)
                {
                    winner = "We have a draw";
                    highScore = player.GetTotalScore();
                }
                if (player.GetTotalScore() > highScore)
                {
                    winner = player.Name;
                    highScore = player.GetTotalScore();
                }
            }
            viewController.GameFinished(winner, highScore);
            viewController.GameSaved(fileName);
        }
    }
}
