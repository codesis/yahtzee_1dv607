using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.Model.Observer;
using yahtzee_1dv607.View;


namespace yahtzee_1dv607.Controller
{
    class ViewController : InterfaceDiceObserver
    {
        private ScoreView scoreView;
        private SettingsView settingsView;
        private RoundsView roundsView;

        public ViewController(Variant variant, DiceCollection diceCollection)
        {
            settingsView = new SettingsView();
            scoreView = new ScoreView(variant);
            roundsView = new RoundsView(variant);

            Subscribe(diceCollection);
        }

        private void Subscribe(DiceCollection diceCollection)
        {
            diceCollection.Subscribe(this);
        }

        public void DiceRolled(int[] diceValues, int[] dice)
        {
            roundsView.RenderDice(dice);
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

        public void RenderNumberOfRound(int roundNumber)
        {
            roundsView.RenderNumberOfRound(roundNumber);
        }
        
        public void RenderRound(string name)
        {
            roundsView.RenderRound(name);
           
        }

        public bool[] GetDiceToRoll()
        {
            return roundsView.GetDiceToRoll();
        }
        
        public void RenderUnavailableChoices(List<Variant.Type> unavailableChoices)
        {
            if (roundsView.SelectActivity(DisplayType.ViewAvailableChoices, false))
            {
                roundsView.RenderUnavailableChoices(unavailableChoices);
            }
        }

        public Variant.Type RenderChoices(List<Variant.Type> unavailableChoices)
        {
            return roundsView.RenderChoices(unavailableChoices);
        }

        public void RenderDiceToRoll(bool[] DiceToRoll, string decision = "")
        {
            roundsView.RenderDiceToRoll(DiceToRoll, decision);
            Thread.Sleep(2000);
        }

        public void RenderScoreOfRound(int roundScore, Variant.Type chosenVariant)
        {
            scoreView.RenderScoreOfRound(roundScore, chosenVariant);
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

        public void RenderHighscore(List<Player> players, string date = null, bool fullList = true)
        {
            scoreView.RenderHighscore(players, date, fullList);
        }

        public string SelectGame(FileInfo[] files)
        {
            return roundsView.GetSavedGame(files);
        }

    }
}
