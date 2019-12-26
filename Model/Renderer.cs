using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using yahtzee_1dv607.Model;
using yahtzee_1dv607.Model.Rules;
using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Players;
using yahtzee_1dv607.Model.Dices;
using yahtzee_1dv607.View;

namespace yahtzee_1dv607.Model
{
    public class Renderer
    {
        private ScoreView scoreView;
        private SettingsView settingsView;
        private RoundsView roundsView;
        private InterfaceRules rules;
        private Database database;
        private Renderer renderer;
        private List<Player> players;
        private int RoundNumber { get; set; }
        private DateTime Date { get; set; }

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
        public void RenderDiceToRoll(bool[] DiceToRoll, string decision)
        {
            roundsView.RenderDiceToRoll(DiceToRoll, decision);
            Thread.Sleep(2000);
        }

        public void RenderScoreOfRound(int roundScore, Variant.Type chosenVariant)
        {
            scoreView.RenderScoreOfRound(roundScore, chosenVariant);
        }
        public void RenderHighscore(List<Player> players, string date = null, bool fullList = true)
        {
            scoreView.RenderHighscore(players, date, fullList);
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

    }
}
