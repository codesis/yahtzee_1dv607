using System.Collections.Generic;
using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model.Players
{
    
    class Player
    {
        private List<Score> scoreList;
        public string Decision { get; protected set; }

        public Player()
        {
            this.scoreList = new List<Score>();
        }

        public Player(string name, bool ai = false): this()
        {
            Name = name;
            IsAI = ai;
        }
        public Player(string name, List<Score> scores,  bool ai = false) : this(name, ai)
        {
            foreach (Score score in scores)
            {
                scoreList.Add(score);
            }
        }

        public string Name { get; private set; }
        public bool IsAI { get; private set; }

        public void AddScoreToList(Variant.Type variant, int point)
        {
            scoreList.Add(new Score(variant, point));
        }

        public int GetScoreFromList(Variant.Type variant, out bool exist)
        {
            Score score = scoreList.Find(scoreObj => scoreObj.ChosenVariant == variant);

            if (score != null)
            {
                exist = true;
                return score.Points;
            }
            else
            {
                exist = false;
                return 0;
            }
        }
        public Score[] GetScoreList()
        {
            Score[] scoreListCopy = new Score[scoreList.Count];

            scoreList.CopyTo(scoreListCopy, 0);
            return scoreListCopy;
        }
        public int GetTotalScore()
        {
            int sum = 0;
            foreach(Score score in scoreList)
            {
                sum += score.Points;
            }
            return sum;
        }
        public bool GetVariantChosen(Variant.Type variant)
        {
            Score score = scoreList.Find(scoreObj => scoreObj.ChosenVariant == variant);
            
            if (score != null)
            {
                return true;
            }
            return false;
        }
        public List<Variant.Type> GetChosenVariants(Variant variant)
        {
            List<Variant.Type> unavailableVariants = new List<Variant.Type>();

            foreach (Variant.Type vari in variant.GetList())
            {
                Score score = scoreList.Find(scoreObj => scoreObj.ChosenVariant == vari);

                if (score != null)
                {
                    unavailableVariants.Add(vari);
                }
                
            }
            return unavailableVariants;
        }
    }
}
