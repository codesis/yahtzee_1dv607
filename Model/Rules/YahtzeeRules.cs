using yahtzee_1dv607.Model.Variants;
using yahtzee_1dv607.Model.Dices;

namespace yahtzee_1dv607.Model.Rules
{
    class YahtzeeRules : InterfaceRules
    {
        private DiceCollection diceCollection;
        private const int yahtzeeValue = 50;
        private const int largeStraightValue = 40;
        private const int smallStraightValue = 30;
        private const int fullHouseValue = 25;

        public YahtzeeRules(DiceCollection diceCollection)
        {
            this.diceCollection = diceCollection;
            BasicRules = new BasicRules(diceCollection);
        }

        public BasicRules BasicRules { get; set; }

        public int GetValueByVariant(Variant.Type variant)
        {
            int retValueForVariant = 0; 
            VariantYahtzee.Type variantYahtzee = (VariantYahtzee.Type)variant;

            switch (variantYahtzee)
            {
                case VariantYahtzee.Type.Aces:
                case VariantYahtzee.Type.Twos:
                case VariantYahtzee.Type.Threes:
                case VariantYahtzee.Type.Fours:
                case VariantYahtzee.Type.Fives:
                case VariantYahtzee.Type.Sixes:

                    retValueForVariant = SumOfSameVariant(variantYahtzee);
                    break;

                case VariantYahtzee.Type.ThreeOfAKind:

                    if (BasicRules.HasThreeOfAKind())
                        retValueForVariant = ThreeOfAKind();
                    break;

                case VariantYahtzee.Type.FourOfAKind:

                    if (BasicRules.HasFourOfAKind())
                        retValueForVariant = FourOfAKind();
                    break;

                case VariantYahtzee.Type.FullHouse:

                    if (BasicRules.HasFullHouse())
                        retValueForVariant = FullHouse();
                    break;

                case VariantYahtzee.Type.SmallStraight:

                    if (HasSmallStraight())
                        retValueForVariant = SmallStraight();
                    break;

                case VariantYahtzee.Type.LargeStraight:

                    if (HasLargeStraight())
                        retValueForVariant = LargeStraight();
                    break;

                case VariantYahtzee.Type.Yahtzee:

                    if (BasicRules.HasYahtzee())
                        retValueForVariant = Yahtzee();
                    break;

                case VariantYahtzee.Type.Chance:

                    retValueForVariant = diceCollection.GetSumOfDice();
                    break;
            }
            return retValueForVariant;
        }

        public bool HasLargeStraight()
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
            else if (diceValue[1] == 1)
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
            int[] diceVal = diceCollection.GetNumberOfDiceFaceValue();
            bool straight = false;
            if (diceVal[0] == 1 || diceVal[0] == 2)
            {
                straight = true;
                for (int i = 1; i < 4; i++)
                {
                    if (diceVal[i] != 1 && diceVal[i] != 2)
                    {
                        straight = false;
                    }
                }

            }
            if (!straight && (diceVal[1] == 1 || diceVal[1] == 2))
            {
                straight = true;
                for (int i = 2; i < diceVal.Length; i++)
                {
                    if (diceVal[i] != 1 && diceVal[i] != 2)
                    {
                        straight = false;
                    }
                }
            }
            if (!straight && (diceVal[2] == 1 || diceVal[2] == 2))
            {
                straight = true;
                for (int i = 3; i < diceVal.Length; i++)
                {
                    if (diceVal[i] != 1 && diceVal[i] != 2)
                    {
                        straight = false;
                    }
                }
            }
            return straight;
        }

        private int ThreeOfAKind()
        {
            return diceCollection.GetSumOfDice();
        }

        private int FourOfAKind()
        {
            return diceCollection.GetSumOfDice();
        }

        private int FullHouse()
        {
            return fullHouseValue;
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

        private int SumOfSameVariant(VariantYahtzee.Type variant)
        {
            int faceValue = (int)variant + 1;
            int[] diceValue = diceCollection.GetNumberOfDiceFaceValue();
            return diceValue[faceValue - 1] * (faceValue);
        }
    }
}
