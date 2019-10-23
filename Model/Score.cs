using yahtzee_1dv607.Model.Variants;

namespace yahtzee_1dv607.Model
{
    class Score
    {
        public Variant.Type UsedVariant { get; private set; }

        public int Points { get; private set; }

        public Score(Variant.Type Variant, int points)
        {
            UsedVariant = Variant;
            Points = points;
        }
    }
}
