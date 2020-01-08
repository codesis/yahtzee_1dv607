using System;
using System.Collections.Generic;
using System.IO;

using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.Model.Observer;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model;
using yahtzee_1dv607.View;


namespace yahtzee_1dv607.Controller
{
    class ViewController : InterfaceDiceObserver
    {
        private RoundsView roundsView;
        private InterfaceRules rules;
        private Database database;
        private Renderer renderer;
        private List<Player> playersfromfile { get; set; }
        public int RoundNumber { get; set; }
        public DateTime Date { get; set; }

        public ViewController(Variant variant, DiceCollection diceCollection)
        {
            roundsView = new RoundsView(variant);
            renderer = new Renderer(variant);

            Subscribe(diceCollection);
        }

        private void Subscribe(DiceCollection diceCollection)
        {
            diceCollection.Subscribe(this);
        }

        public string GetFiles (Database database, bool selection)  
        {
            string viewGameFile = "";

            FileInfo[] files = database.ListSavedGames();
            viewGameFile = SelectGame(files);

            if (viewGameFile != "")
            {
                DateTime date = new DateTime();
                int roundNumber = 0;
                playersfromfile = database.GetPlayersFromFile(rules, viewGameFile, out date, out roundNumber);
                Date = date;
                RoundNumber = roundNumber;

                if (selection == true)
                {
                    renderer.RenderHighscore(playersfromfile, date.ToString(), true);
                } 
            }

            return viewGameFile;
        }

        public List<Player> GetListOfPlayersFromFile()
        {
            return playersfromfile;
        }

        public void DiceRolled(int[] diceValues, int[] dice)
        {
            roundsView.RenderDice(dice);
        }

        public bool ContinueGame()
        {
            return roundsView.ContinueGame();
        }

        public bool ResumeGame()
        {
            return roundsView.SelectActivity(DisplayType.ResumeSavedGame);
        }
        public void SaveGame(string fileName)
        {
            roundsView.SaveGame(fileName);
        }

        public void GameCompleted(string winner, int score)
        {
            roundsView.GameCompleted(winner, score);
        }

        public bool ViewHighscore()
        {
            return roundsView.SelectActivity(DisplayType.ViewHighscore);
        }

        public string SelectGame(FileInfo[] files)
        {
            return roundsView.GetSavedGame(files);
        }

    }
}
