using System;
using System.Collections.Generic;

using yahtzee_1dv607.Model.Factory;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.View;

namespace yahtzee_1dv607.Model
{
    
    public class GameSetup
    {
        internal InterfaceRules rules;
        internal DiceCollection diceCollection;
        private SettingsView settingsView;
        internal GameManufactory manufactory;
        internal Renderer renderer;
        internal Database database;
        internal List<Player> addedplayers = new List<Player>();
        private List<Player> playersfromfile = new List<Player>();
        internal Variant variant;
        internal GameType gameType;
        private bool[] DiceToRoll { get; set; }

        public GameSetup()
        {
            this.gameType = new MainMenu().RenderStartMenu();
            this.manufactory = new GameManufactory(gameType);
            this.diceCollection = new DiceCollection();
            this.database = new Database(variant, rules, gameType);

            this.variant = manufactory.GetVariant();
            this.rules = manufactory.GetRules(diceCollection);
            this.renderer = new Renderer(variant);
            settingsView = new SettingsView();
            this.addedplayers = new List<Player>();
        }

        private int GetNumberOfAis()
        {
            int numberOfAis = 0;
            foreach (Player player in addedplayers)
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
            this.addedplayers = new List<Player>();
            int numberOfPlayers = NumberOfPlayers();

            for (int i = 1; i <= numberOfPlayers; i++)
            {
                string name = PlayerName(i, out ai);

                if (ai)
                {
                    addedplayers.Add(new Ai(GetNumberOfAis() + 1, rules, variant, gameType));
                }
                else
                {
                    addedplayers.Add(new Player(name));
                }
            }
        }

        private int NumberOfPlayers()
        {
            return settingsView.NumberOfPlayers();
        }

        private string PlayerName(int number, out bool ai)
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

        public void RunRound(int roundNumber, List<Player> addedplayers) 
        {
            renderer.RenderNumberOfRound(roundNumber);

            foreach (Player player in addedplayers)
            {
                DiceToRoll = new bool[] { true, true, true, true, true };
                PlayRound(player);
            }
            
            renderer.RenderHighscore(addedplayers);
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
    }
}
