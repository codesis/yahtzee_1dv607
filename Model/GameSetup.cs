using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.View;

namespace yahtzee_1dv607.Model
{
    
    public class GameSetup
    {
        private InterfaceRules rules;
        private DiceCollection diceCollection;
        private SettingsView settingsView;
        private List<Player> players;
        private Variant variant;
        private GameType gameType;
        private bool[] DiceToRoll { get; set; }

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
        public void PlayerSetup()
        {
            bool ai;
            players = new List<Player>();
            int numberOfPlayers = NumberOfPlayers();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                string name = PlayerName(i, out ai);

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
        public int NumberOfPlayers()
        {
            return settingsView.NumberOfPlayers();
        }

        public string PlayerName(int number, out bool ai)
        {
            string name = "";

            ai = settingsView.IsAi(number);
            if (!ai)
            {
                name = settingsView.PlayerName(number);
            }
            Console.Clear();
            return name;
        }

        public void RunRound(int roundNumber)
        {
            viewController.RenderNumberOfRound(roundNumber);

            foreach (Player player in players)
            {
                DiceToRoll = new bool[] { true, true, true, true, true };
                PlayRound(player);
            }
            
            viewController.RenderHighscore(players);
        }

        private void PlayRound(Player player)
        {
            Ai ai = player as Ai;
            Variant.Type choiceToPick = variant.Chance();

            viewController.RenderRound(player.Name);

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
                choiceToPick = viewController.RenderChoices(player.GetTakenChoices(variant));
            }

            AddingScoreToList(player, choiceToPick);

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
                                
                        viewController.RenderUnavailableChoices(player.GetTakenChoices(variant));
                        DiceToRoll = viewController.GetDiceToRoll();
                    }
                    viewController.RenderDiceToRoll(DiceToRoll, player.Decision);
                }
            }
        }

        private void AddingScoreToList(Player player, Variant.Type choiceToPick)
        {
            player.AddScoreToList(choiceToPick, rules.GetValueByVariant(choiceToPick));

            bool exist = false;
            int roundScore = player.GetScoreFromList(choiceToPick, out exist);

            if (exist)
            {
                viewController.RenderScoreOfRound(roundScore, choiceToPick);
            }
        }
    }
}
