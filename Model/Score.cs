using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model
{
    class Score
    {
        public Variant.Type ChosenVariant { get; private set; }

        public int Points { get; private set; }

        public Score(Variant.Type Variant, int points)
        {
            ChosenVariant = Variant;
            Points = points;
        }
    }
}
