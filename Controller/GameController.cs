using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
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
        private Variant variant;
        private GameType gameType;

        private DateTime Date { get; set; }
        private int RoundNumber { get; set; }

        public GameController()
        {
        }
        
        private bool[] DiceToRoll { get; set; }


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

        private int GetNumberOfRobots()
        {
            int numberOfRobots = 0;
            foreach (Player player in players)
            {
                if (player.IsAI)
                {
                    numberOfRobots++;
                }
            }
            return numberOfRobots;
        }
    }
}
