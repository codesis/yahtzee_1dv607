using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model
{
    class Score
    {
        public int Points { get; private set; }
        public Variant.Type TakenChoice { get; private set; }

        public Score(Variant.Type variant, int points)
        {
            TakenChoice = variant;
            Points = points;
        }
    }
}
