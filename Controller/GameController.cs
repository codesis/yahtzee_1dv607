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
        private GameManufactory manufactory;
        private InterfaceRules rules;
        private DiceCollection diceCollection;
        private GameSetup gameSetup;
        private List<Player> players;
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
            diceCollection = new DiceCollection();
            gameType = new MainMenu().RenderStartMenu();
            manufactory = new GameManufactory(gameType);

            variant = manufactory.GetVariant();
            rules = manufactory.GetRules(diceCollection);
            database = new Database(variant, rules, gameType);
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

        // private bool AnyDiceToRoll()
        // {
        //     bool roll = false;
        //     for (int i=0; i< DiceToRoll.Length;i++)
        //     {
        //         if (DiceToRoll[i])
        //             roll = true;
        //     }
        //     return roll;
        // }

        // private int GetNumberOfAis()
        // {
        //     int numberOfAis = 0;
        //     foreach (Player player in players)
        //     {
        //         if (player.IsAI)
        //         {
        //             numberOfAis++;
        //         }
        //     }
        //     return numberOfAis;
        // }
        // private void PlayerSetup()
        // {
        //     bool ai;
        //     players = new List<Player>();
        //     int numberOfPlayers = viewController.NumberOfPlayers();

        //     for (int i = 1; i <= numberOfPlayers; i++)
        //     {
        //         string name = viewController.PlayerName(i, out ai);

        //         if (ai)
        //         {
        //             players.Add(new Ai(GetNumberOfAis() + 1, rules, variant, gameType));
        //         }
        //         else
        //         {
        //             players.Add(new Player(name));
        //         }
        //     }
        // }

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

        // private void RunRound(int roundNumber)
        // {
        //     viewController.RenderNumberOfRound(roundNumber);

        //     foreach (Player player in players)
        //     {
        //         DiceToRoll = new bool[] { true, true, true, true, true };
        //         PlayRound(player);
        //     }
            
        //     viewController.RenderHighscore(players);
        // }

        // private void PlayRound(Player player)
        // {
        //     Ai ai = player as Ai;
        //     Variant.Type choiceToPick = variant.Chance();

        //     viewController.RenderRound(player.Name);

        //     for (int rollNumber = 1; rollNumber <= 3; rollNumber++)
        //     {
        //         RolledDice(rollNumber, player, ai);
        //     }
        //     if (player.IsAI)
        //     {
        //         choiceToPick = ai.SelectBestAvailableChoice();
        //     }
        //     else
        //     {
        //         choiceToPick = viewController.RenderChoices(player.GetTakenChoices(variant));
        //     }

        //     AddingScoreToList(player, choiceToPick);

        // }

        // private void RolledDice(int rollNumber, Player player, Ai ai)
        // {
        //     if (AnyDiceToRoll())
        //     {
        //         diceCollection.Roll(DiceToRoll);

        //         if (rollNumber < 3)
        //         {
        //             if (player.IsAI)
        //             {
        //                 DiceToRoll = ai.SelectDiceToRoll(diceCollection.GetNumberOfDiceFaceValue(), diceCollection.GetDice());
        //             }
        //             else
        //             {
        //                 if (rollNumber == 1)
                                
        //                 viewController.RenderUnavailableChoices(player.GetTakenChoices(variant));
        //                 DiceToRoll = viewController.GetDiceToRoll();
        //             }
        //             viewController.RenderDiceToRoll(DiceToRoll, player.Decision);
        //         }
        //     }
        // }

        // private void AddingScoreToList(Player player, Variant.Type choiceToPick)
        // {
        //     player.AddScoreToList(choiceToPick, rules.GetValueByVariant(choiceToPick));

        //     bool exist = false;
        //     int roundScore = player.GetScoreFromList(choiceToPick, out exist);

        //     if (exist)
        //     {
        //         viewController.RenderScoreOfRound(roundScore, choiceToPick);
        //     }
        // }

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
