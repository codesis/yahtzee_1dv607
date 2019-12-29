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
        private ViewController viewController;
        private Database database;
        private Renderer renderer;
        private GameManufactory manufactory;
        private InterfaceRules rules;
        private DiceCollection diceCollection;
        private GameSetup gameSetup;
        private List<Player> players { get; set; }
        private Variant variant;
        private GameType gameType;
        private DateTime Date { get; set; }
        private int RoundNumber { get; set; }
        private bool[] DiceToRoll { get; set; }

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
            manufactory = gameSetup.manufactory;

            variant = gameSetup.variant;
            rules = gameSetup.rules;
            database = new Database(variant, rules, gameType);
            renderer = new Renderer(variant);
            viewController = new ViewController(variant, diceCollection);

            if (viewController.ViewHighscore() || viewController.ResumeGame())
            {
                viewController.GetFiles();
            }

            if (viewController.ViewGameResult())
            {
                viewController.SetViewGameFile();
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

                RunRound(i);
                RoundNumber++;
            }
            GameCompleted();
        }
        private void RunRound(int roundNumber)
        {
            renderer.RenderNumberOfRound(roundNumber);

            foreach (Player player in players)
            {
                DiceToRoll = new bool[] { true, true, true, true, true };
                PlayRound(player);
            }
            
            renderer.RenderHighscore(players);
        }

        private void PlayRound(Player player)
        {
            Ai ai = player as Ai;
            Variant.Type choiceToPick = variant.Chance();

            renderer.RenderRound(player.Name);

            for (int rollNumber = 1; rollNumber <= 3; rollNumber++)
            {
                RolledDice(rollNumber, player, ai);
            }
            if (player.IsAI)
            {
                choiceToPick = ai.SelectBestAvailableChoice();
            }
            else
            {
                choiceToPick = renderer.RenderChoices(player.GetTakenChoices(variant));
            }

            AddingScoreToList(player, choiceToPick);

        }
        private void AddingScoreToList(Player player, Variant.Type choiceToPick)
        {
            player.AddScoreToList(choiceToPick, rules.GetValueByVariant(choiceToPick));

            bool exist = false;
            int roundScore = player.GetScoreFromList(choiceToPick, out exist);

            if (exist)
            {
                renderer.RenderScoreOfRound(roundScore, choiceToPick);
            }
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

        private void RolledDice(int rollNumber, Player player, Ai ai)
        {
            if (AnyDiceToRoll())
            {
                diceCollection.Roll(DiceToRoll);

                if (rollNumber < 3)
                {
                    if (player.IsAI)
                    {
                        DiceToRoll = ai.SelectDiceToRoll(diceCollection.GetNumberOfDiceFaceValue(), diceCollection.GetDice());
                    }
                    else
                    {
                        if (rollNumber == 1)
                                
                        renderer.RenderUnavailableChoices(player.GetTakenChoices(variant));
                        DiceToRoll = renderer.GetDiceToRoll();
                    }
                    renderer.RenderDiceToRoll(DiceToRoll, player.Decision);
                }
            }
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
