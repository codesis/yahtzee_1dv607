using System;
using System.Collections.Generic;

using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Dices;

namespace yahtzee_1dv607.Controller
{
    class GameController
    {
        private ViewController viewController;
        private Database database;
        private InterfaceRules rules;
        private DiceCollection diceCollection;
        private GameSetup gameSetup;
        private List<Player> players { get; set; }
        private Variant variant;
        private GameType gameType;
        private DateTime Date { get; set; }
        private int RoundNumber { get; set; }

        public GameController()
        {
            StartGame();
            RunGame();
        }
        
        private void StartGame()
        {
            gameSetup = new GameSetup();
            diceCollection = gameSetup.diceCollection;
            gameType = gameSetup.gameType;

            variant = gameSetup.variant;
            rules = gameSetup.rules;
            database = new Database(variant, rules, gameType);
            viewController = new ViewController(variant, diceCollection);

            if (viewController.ViewHighscore() || viewController.ResumeGame())
            {
                viewController.GetFiles(database);
            }

            if (viewController.ViewGameResult())
            {
                viewController.SetViewGameFile(database);
            }

            else
            {
                Date = DateTime.Now;
                RoundNumber = 0;
                gameSetup.PlayerSetup();
                players = gameSetup.addedplayers;
            }
        }

        public void ResumeGame(string resumeGameFile)
        {
            DateTime date = new DateTime();
            int roundNumber = 0;
            players = database.GetPlayersFromFile(rules, resumeGameFile, out date, out roundNumber);
            Date = date;
            RoundNumber = roundNumber;

            
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
                    viewController.SaveGame(fileName);
                    return;
                }

                gameSetup.RunRound(i);
                RoundNumber++;
            }
            GameCompleted();
        }

        private void GameCompleted()
        {
            string fileName = database.SaveGameToFile(Date, RoundNumber, players);
            int highScore = 0;
            string winner = "";

            foreach (Player player in players)
            {
                if (player.GetTotalScore() == highScore)
                {
                    winner = "We seems to have a draw...";
                    highScore = player.GetTotalScore();
                }
                if (player.GetTotalScore() > highScore)
                {
                    winner = player.Name;
                    highScore = player.GetTotalScore();
                }
            }
            viewController.GameCompleted(winner, highScore);
            viewController.SaveGame(fileName);
        }
    }
}
