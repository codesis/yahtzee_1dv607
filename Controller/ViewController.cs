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
        private List<Player> players;
        private GameController gameController;
        private int RoundNumber { get; set; }
        private DateTime Date { get; set; }

        public ViewController(Variant variant, DiceCollection diceCollection)
        {
            roundsView = new RoundsView(variant);

            Subscribe(diceCollection);
        }

        private void Subscribe(DiceCollection diceCollection)
        {
            diceCollection.Subscribe(this);
        }

        public string GetFiles (Database database)
        {
            string viewGameFile = "";

            FileInfo[] files = database.ListSavedGames();
            viewGameFile = SelectGame(files);

            if (viewGameFile != "")
            {
                gameController.ResumeGame(viewGameFile);
            }

            return viewGameFile;
        }

        public void SetViewGameFile (Database database)
        {
            if (GetFiles(database) != "")
            {
                ViewGameFile(GetFiles(database));
            }
        }

        public void ViewGameFile(string viewGameFile)
        {
            List<string> items = new List<string>();
            DateTime date = new DateTime();

            bool highscore = ViewHighscore();
            int roundNumber = 0;
            players = database.GetPlayersFromFile(rules, viewGameFile, out date, out roundNumber);

            Date = date;
            RoundNumber = roundNumber;
            renderer.RenderHighscore(players, date.ToString(), highscore);
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


        public bool ViewGameResult()
        {
            return roundsView.SelectActivity(DisplayType.ViewSavedGame);
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
