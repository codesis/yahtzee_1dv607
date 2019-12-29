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
        internal List<Player> addedplayers = new List<Player>();
        internal Variant variant;
        internal GameType gameType;

        public GameSetup()
        {
            this.gameType = new MainMenu().RenderStartMenu();
            this.manufactory = new GameManufactory(gameType);
            this.diceCollection = new DiceCollection();

            this.variant = manufactory.GetVariant();
            this.rules = manufactory.GetRules(diceCollection);
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
    }
}
