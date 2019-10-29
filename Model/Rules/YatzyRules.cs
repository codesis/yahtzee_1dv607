using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Dices;

namespace yahtzee_1dv607.Model.Rules
{
    class YatzyRules : InterfaceRules
    {
        private DiceCollection diceCollection;
        private const int yahtzeeValue = 50;
        private const int largeStraightValue = 20;
        private const int smallStraightValue = 15;

        public YatzyRules(DiceCollection diceCollection)
        {
            this.diceCollection = diceCollection;
            BasicRules = new BasicRules(diceCollection);
        }

        public BasicRules BasicRules { get; set; }

        public int GetValueByVariant(Variant.Type variant)
        {
            VariantYatzy.Type variantYatzy = (VariantYatzy.Type) variant;
            int retValueForVariant = 0; 

            switch (variantYatzy)
            {
                case VariantYatzy.Type.Aces:
                case VariantYatzy.Type.Twos:
                case VariantYatzy.Type.Threes:
                case VariantYatzy.Type.Fours:
                case VariantYatzy.Type.Fives:
                case VariantYatzy.Type.Sixes:

                    retValueForVariant = SumOfSameVariant(variantYatzy);
                    break;

                case VariantYatzy.Type.Pair:

                    if (BasicRules.HasPair())
                        retValueForVariant = Pair();
                    break;

                case VariantYatzy.Type.TwoPair:
                
                    if (BasicRules.HasTwoPair())
                        retValueForVariant = TwoPair();
                    break;

                case VariantYatzy.Type.ThreeOfAKind:

                    if (BasicRules.HasThreeOfAKind())
                        retValueForVariant = ThreeOfAKind();
                    break;

                case VariantYatzy.Type.FourOfAKind:

                    if (BasicRules.HasFourOfAKind())
                        retValueForVariant = FourOfAKind();
                    break;

                case VariantYatzy.Type.FullHouse:

                    if (BasicRules.HasFullHouse())
                        retValueForVariant = FullHouse();
                    break;

                case VariantYatzy.Type.SmallStraight:

                    if (HasSmallStraight())
                        retValueForVariant = SmallStraight();
                    break;

                case VariantYatzy.Type.LargeStraight:

                    if (HasLargeStraight())
                        retValueForVariant = LargeStraight();
                    break;

                case VariantYatzy.Type.Yahtzee:

                    if (BasicRules.HasYahtzee())
                        retValueForVariant = Yahtzee();
                    break;

                case VariantYatzy.Type.Chance:
                
                    retValueForVariant = diceCollection.GetSumOfDice();
                    break;
            }
            return retValueForVariant;
        }

        private int Pair()
        {
            int totalValue = 0;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            int dice = diceValue.Length;

            for (int i = dice; i > 0; i--)
            {
                if (diceValue[i - 1] > 1)
                    totalValue = i * 2;
            }
            return totalValue;
        }

        private int TwoPair()
        {
            int totalValue = 0;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            int dice = diceValue.Length;

            for (int i = dice; i > 0; i--)
            {
                if (diceValue[i - 1] > 1)
                    totalValue += i * 2;
            }
            return totalValue;
        }

        private int ThreeOfAKind()
        {
            int totalValue = 0;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            int dice = diceValue.Length;

            for (int i = dice; i > 0; i--)
            {
                if (diceValue[i - 1] > 2)
                    totalValue = i * 3;
            }
            return totalValue;
        }

        private int FourOfAKind()
        {
            int totalValue = 0;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            int dice = diceValue.Length;

            for (int i = dice; i > 0; i--)
            {
                if (diceValue[i - 1] > 3)
                    totalValue = i * 4;
            }
            return totalValue;
        }

        private int FullHouse()
        {
            return diceCollection.GetSumOfDice();
        }

        private int SmallStraight()
        {
            return smallStraightValue;
        }

        private int LargeStraight()
        {
            return largeStraightValue;
        }

        private int Yahtzee()
        {
            return yahtzeeValue;
        }

        private int SumOfSameVariant(VariantYatzy.Type Variant)
        {
            int faceValue = (int)Variant + 1;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            return diceValue[faceValue - 1] * (faceValue);
        }

        public bool HasLargeStraight()
        {
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            if (diceValue[1] == 1)
            {
                for (int i = 1; i < diceValue.Length; i++)
                {
                    if (diceValue[i] != 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool HasSmallStraight()
        {
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            if (diceValue[0] == 1)  
            {
                for (int i = 0; i < diceValue.Length - 1; i++)
                {
                    if (diceValue[i] != 1)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
